using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BasicIDE.Basic
{
    public class Compiler
    {
        /// <summary>
        /// Highest possible line number
        /// </summary>
        private const int MaxLine = 65529;
        /// <summary>
        /// Supported types.
        /// $=string
        /// #=double
        /// !=single
        /// %=int
        /// </summary>
        private const string Types = "$#!%";
        /// <summary>
        /// Default type in use by BASIC
        /// </summary>
        private const string DefaultType = "#";

        /// <summary>
        /// Known instructions
        /// </summary>
        private static readonly string[] Instructions = new string[]
        {
            "?",
            "ATN", "BEEP", "CALL", "CDBL", "CHR$", "CINT", "CLEAR", "CLOAD", "CLOADM", "CLOSE", "CLS",
            "COM", "CONT", "COS", "CSAVE", "CSAVEM", "CSNG", "CSRLIN", "DATA", "DATE$",
            "DAY$", "DEFDBL", "DEFINT", "DEFSNG", "DEFSTR", "DIM", "EDIT", "END", "EOF", "ERL", "ERR", "ERROR",
            "EXP", "FILES", "FIX", "FOR", "NEXT", "FRE", "GOSUB", "GOTO", "HIMEM", "IF", "INKEY$", "INP",
            "INPUT", "INPUT$", "INSTR", "INT", "IPL", "KEY", "KILL", "LCOPY",
            "LEFT$", "LEN", "LET", "LINE", "LIST", "LLIST", "LOAD", "LOADM", "LOG", "LPOS", "LPRINT",
            "MAXFILES", "MAXRAM", "MDM", "MENU", "MERGE", "MID$", "MOTOR", "NAME", "NEW",
            "ON", "OPEN", "OUT", "PEEK", "POKE", "POS", "POWER",
            "PRESET", "PRINT", "PSET", "READ", "REM", "RESTORE", "RESUME", "RIGHT$", "RND", "RUN", "RUNM",
            "SAVE", "SAVEM", "SCREEN", "SGN", "SIN", "SOUND", "SPACE$", "SQR", "STOP", "STR$", "STRING$", "TAB",
            "TAN", "TIME$", "VAL", "VARPTR"
        };

        public CompilerConfig Config { get; }

        public Compiler(CompilerConfig Config)
        {
            this.Config = Config ?? throw new ArgumentNullException(nameof(Config));
        }

        public CompilerResult Compile(string[] Lines, string[] Functions)
        {
            if (Lines == null)
            {
                throw new ArgumentNullException(nameof(Lines));
            }

            if (Functions == null)
            {
                throw new ArgumentNullException(nameof(Functions));
            }
            //Convert names to uppercase for case insensitive comparison
            Functions = Functions.Select(m => m.ToUpper()).ToArray();

            var SourceOffset = 0;
            var Source = new List<string>(Lines);
            var Ret = new CompilerResult(Config.TreatWarningsAsErrors);
            var CompiledLines = new List<Line>();
            int LineNumber = Config.StartLine;
            string Label = null;
            var Labels = new Dictionary<string, int>();
            string FunctionName = null;
            for (var i = 0; i < Source.Count; i++)
            {
                if (LineNumber < 1 || LineNumber > MaxLine)
                {
                    throw new OutOfMemoryException("Program line overflow");
                }

                var L = Source[i].Trim();
                if (string.IsNullOrWhiteSpace(L))
                {
                    continue;
                }
                var Parts = new List<string>(SplitLine(L));
                if (Config.StripComments)
                {
                    Parts.RemoveAll(IsComment);
                    if (Parts.Count == 0)
                    {
                        continue;
                    }
                }
                if (Config.StripDebug)
                {
                    Parts.RemoveAll(IsDebug);
                    if (Parts.Count == 0)
                    {
                        continue;
                    }
                }
                else
                {
                    Parts = Parts.Select(StripDebugSymbol).ToList();
                }

                //Handle label. Labels are only allowed in the first part
                if (Parts.Skip(1).Any(m => IsLabel(m)))
                {
                    Ret.AddMessage(new SyntaxError(i-SourceOffset, SyntaxErrorType.Error, "Label only allowed at the start of a concatenated line", FunctionName));
                }
                if (IsLabel(Parts[0]))
                {
                    var LineLabel = Parts[0].ToUpper().Trim();
                    var FunctionPreview = Parts[0].Substring(1);
                    if (Labels.ContainsKey(Parts[0]))
                    {
                        Ret.AddMessage(new SyntaxError(i - SourceOffset, SyntaxErrorType.Error, $"Label already defined: {Parts[0]}", FunctionName));
                    }
                    if (Label != null)
                    {
                        Ret.AddMessage(new SyntaxError(i - SourceOffset, SyntaxErrorType.Error, $"Duplicate label for same line. Label: {Parts[0]}", FunctionName));
                    }
                    //If the label is not in the known function list it's a manually created label.
                    if (Functions.Contains(FunctionPreview.ToUpper()))
                    {
                        FunctionName = FunctionPreview;
                    }
                    Label = LineLabel;
                    Parts.RemoveAt(0);
                    if (Parts.Count == 0)
                    {
                        continue;
                    }
                }

                for (var j = 0; j < Parts.Count; j++)
                {
                    if (IsCall(Parts[j]))
                    {
                        try
                        {
                            var instructions = FormatCall(Parts[j], Ret, i - SourceOffset, FunctionName);
                            Parts[j] = instructions[0];
                            if (instructions.Length > 1)
                            {
                                SourceOffset += instructions.Length - 1;
                                Source.InsertRange(i + 1, instructions.Skip(1));
                            }
                        }
                        catch (Exception ex)
                        {
                            Ret.AddMessage(new SyntaxError(i - SourceOffset, SyntaxErrorType.Error, ex.Message, FunctionName));
                        }
                    }
                    else if (IsReturn(Parts[j]))
                    {
                        if (j < Parts.Count - 1 && !IsComment(Parts[j + 1]))
                        {
                            Ret.AddMessage(new SyntaxError(i - SourceOffset, SyntaxErrorType.Warning, $"Useless instruction after return: {Parts[j + 1]}", FunctionName));
                        }
                        try
                        {
                            Parts[j] = FormatReturn(Parts[j], Ret, i - SourceOffset, FunctionName);
                        }
                        catch (Exception ex)
                        {
                            Ret.AddMessage(new SyntaxError(i - SourceOffset, SyntaxErrorType.Error, ex.Message, FunctionName));
                        }
                    }
                }
                CompiledLines.Add(new Line()
                {
                    Code = string.Join(":", Parts.Where(m => !string.IsNullOrWhiteSpace(m))),
                    Label = Label,
                    Number = LineNumber,
                    LineIndex = i,
                    FunctionName = FunctionName
                });
                if (Label != null)
                {
                    Labels[Label] = LineNumber;
                    Label = null;
                }
                LineNumber += Config.LineIncrement;
            }
            //Replace all label references
            for (var i = 0; i < CompiledLines.Count; i++)
            {
                var Parts = CompiledLines[i].Code.Split('"');
                for (var j = 0; j < Parts.Length; j += 2)
                {
                    foreach (var KV in Labels)
                    {
                        int index = Parts[j].ToUpper().IndexOf(KV.Key);
                        while (index >= 0)
                        {
                            Parts[j] = Parts[j].Substring(0, index) + KV.Value + Parts[j].Substring(index + KV.Key.Length);
                            index = Parts[j].ToUpper().IndexOf(KV.Key);
                        }
                    }
                    //If the line still contains a label it's invalid
                    if (HasLabelRef(Parts[j]))
                    {
                        Ret.AddMessage(new SyntaxError(CompiledLines[i].LineIndex, SyntaxErrorType.Error, "Label not found", CompiledLines[i].FunctionName));
                    }
                }
                CompiledLines[i].Code = string.Join("\"", Parts);
            }
            Ret.Lines = CompiledLines.ToArray();
            return Ret;
        }

        private string[] FormatCall(string L, CompilerResult Res, int LineNumber, string FunctionName)
        {
            if (!IsCall(L))
            {
                throw new ArgumentException($"Line not a label call statement: {L}");
            }
            //Simple call statement
            var M = Regex.Match(L, @"^\s*CALL\s*(@\w+)");
            if (M.Success)
            {
                return new string[] { "GOSUB " + M.Groups[1].Value };
            }
            M = Regex.Match(L, @"^\s*([^=]+)=\s*CALL([" + Types + @"]?)\s*(@\w+)");
            if (!M.Success)
            {
                throw new ArgumentException("Not a valid extended CALL statement");
            }
            var Assignment = M.Groups[1].Value.TrimEnd();
            var Type = M.Groups[2].Value;
            var Label = M.Groups[3].Value;

            //Infer type from assignment
            if (string.IsNullOrEmpty(Type))
            {
                Type = Assignment[Assignment.Length - 1].ToString();
                if (!Types.Contains(Type))
                {
                    Type = DefaultType;
                    Res.AddMessage(new SyntaxError(LineNumber, SyntaxErrorType.Warning, $"Unable to infer type of extended CALL from assignment. Using default type: {DefaultType}", FunctionName));
                }
                /*
                else
                {
                    Res.AddMessage(new SyntaxError(LineNumber, SyntaxErrorType.Info, $"extended CALL with unspecified type. Using detected type: {Type}", FunctionName));
                }
                //*/
            }
            var IsString = Type == "$";
            var BaseValue = IsString ? "\"\"" : "0";
            var RetType = IsString ? '$' : '#';
            return new string[]{
                $"{Config.ReturnVar}{RetType}={BaseValue} : GOSUB {Label}",
                $"{Assignment}={Config.ReturnVar}{RetType}"
            };
        }

        private string FormatReturn(string L, CompilerResult Res, int LineNumber, string FunctionName)
        {
            if (!IsReturn(L))
            {
                throw new ArgumentException($"Line not a return statement: {L}");
            }
            var M = Regex.Match(L, @"^\s*RETURN([" + Types + @"]?)\s*(.*)$", RegexOptions.IgnoreCase);
            if (!M.Success)
            {
                throw new ArgumentException($"Line not a valid return statement: {L}");
            }
            var Type = M.Groups[1].Value;
            var Statement = M.Groups[2].Value.Trim();
            if (Type.Length == 0 && Statement.Length > 0)
            {
                Type = Statement[Statement.Length - 1].ToString();
                if (Types.Contains(Type))
                {
                    Res.AddMessage(new SyntaxError(LineNumber, SyntaxErrorType.Info, $"Return type not specified. Guessing from argument instead. Using: {Type}", FunctionName));
                }
                else
                {
                    Type = DefaultType;
                    Res.AddMessage(new SyntaxError(LineNumber, SyntaxErrorType.Warning, $"Unable to determine return type. Using BASIC default of {DefaultType}", FunctionName));
                }
            }
            if (Type.Length == 0 && Statement.Length == 0)
            {
                return "RETURN";
            }
            var IsString = Type == "$";
            var RetType = IsString ? '$' : '#';
            return $"{Config.ReturnVar}{RetType}={Statement} : RETURN";
        }

        private static string StripDebugSymbol(string Command)
        {
            var pos = Command.IndexOf('#');
            if (pos >= 0 && Regex.IsMatch(Command, @"^\s*#"))
            {
                return Command.Substring(0, pos) + Command.Substring(pos + 1);
            }
            return Command;
        }

        /// <summary>
        /// Slits a line that may be multiple instructions (using colon)
        /// into individual instructions
        /// </summary>
        /// <param name="L">Line</param>
        /// <returns>Lines</returns>
        public static string[] SplitLine(string L)
        {
            var Segments = new List<string>();
            var buffer = new StringBuilder();
            var instr = false;
            var comment = false;
            foreach (var C in L)
            {
                if (!comment && C == '"')
                {
                    instr = !instr;
                    buffer.Append(C);
                }
                else if (!comment && !instr && C == ':')
                {
                    Segments.Add(buffer.ToString());
                    comment |= IsComment(Segments[Segments.Count - 1]);
                    buffer = new StringBuilder();
                }
                else
                {
                    buffer.Append(C);
                }
            }
            Segments.Add(buffer.ToString());
            return Segments.ToArray();
        }

        public static string[] GetLabels(string[] Lines)
        {
            var Ret = new List<string>();
            foreach (var L in Lines)
            {
                var Label = SplitLine(L)[0].ToUpper();
                if (IsLabel(Label))
                {
                    Ret.Add(Label.Substring(1));
                }
            }
            return Ret.Distinct().ToArray();
        }

        public static bool IsReturn(string L)
        {
            return L.Trim().ToUpper().StartsWith("RETURN");
        }

        public static bool IsComment(string L)
        {
            return L.Trim().ToUpper().StartsWith("REM") || L.Trim().StartsWith("'");
        }

        public static bool IsDebug(string L)
        {
            return L.TrimStart().StartsWith("#");
        }

        public static bool IsCall(string L)
        {
            L = L.Trim().ToUpper();
            if (IsComment(L))
            {
                return false;
            }
            return
                Regex.IsMatch(L, @"^\s*CALL\s*@\w+") ||
                Regex.IsMatch(L, @"^\s*[^=]+=\s*CALL[" + Types + @"]?\s*@\w+");
            //The type argument in the second regex is optional to catch syntax errors later.
        }

        public static bool IsInstruction(string L)
        {
            L = L.Trim().ToUpper();
            if (Regex.IsMatch(L, @"^[A-Z]\w*[" + Types + @"]?\s*="))
            {
                //Assignment to a variable
                return true;
            }
            foreach (var I in Instructions)
            {
                if (L.StartsWith(I))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsLabel(string L)
        {
            return !string.IsNullOrWhiteSpace(L) && Regex.IsMatch(L, @"^\s*@\w+\s*$");
        }

        public static bool HasLabelRef(string L)
        {
            if (string.IsNullOrWhiteSpace(L))
            {
                return false;
            }
            //We only want to find labels outside of strings.
            //Since strings are double quoted, we can simply split by double quote,
            //then look at all even indexes.
            //This works because TRS-80 BASIC doesn't has the "" escaping inside of strings
            var Segments = L.Split('"');
            for (var i = 0; i < Segments.Length; i += 2)
            {
                if (Regex.IsMatch(Segments[i], @"@\w+"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
