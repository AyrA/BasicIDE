using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BasicIDE.Basic
{
    /// <summary>
    /// Compiles IDE formatted BASIC code into TRS-80 line based code
    /// </summary>
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
        public const string Types = "$#!%";
        /// <summary>
        /// Default type in use by BASIC
        /// </summary>
        private const string DefaultType = "#";

        /// <summary>
        /// Known instructions.
        /// They're generally alphabetically sorted but tweaked to be in regex order for longest match.
        /// </summary>
        public static readonly string[] Instructions = new string[]
        {
            "?",
            "ATN", "BEEP", "CALL", "CDBL", "CHR$", "CINT", "CLEAR", "CLOADM", "CLOAD", "CLOSE", "CLS",
            "COM", "CONT", "COS", "CSAVEM", "CSAVE", "CSNG", "CSRLIN", "DATA", "DATE$",
            "DAY$", "DEFDBL", "DEFINT", "DEFSNG", "DEFSTR", "DIM", "EDIT", "END", "EOF", "ERL", "ERROR", "ERR",
            "EXP", "FILES", "FIX", "FOR", "NEXT", "FRE", "GOSUB", "GOTO", "HIMEM", "IF", "INKEY$", "INPUT$",
            "INPUT", "INP", "INSTR", "INT", "IPL", "KEY", "KILL", "LCOPY",
            "LEFT$", "LEN", "LET", "LINE", "LLIST", "LIST", "LOAD", "LOADM", "LOG", "LPOS", "LPRINT",
            "MAXFILES", "MAXRAM", "MDM", "MENU", "MERGE", "MID$", "MOTOR", "NAME", "NEW",
            "ON", "OPEN", "OUT", "PEEK", "POKE", "POS", "POWER",
            "PRESET", "PRINT@", "PRINT", "PSET", "READ", "REM", "RESTORE", "RESUME", "RIGHT$", "RND", "RUNM", "RUN",
            "SAVEM", "SAVE", "SCREEN", "SGN", "SIN", "SOUND", "SPACE$", "SQR", "STEP", "STOP", "STR$", "STRING$", "TAB",
            "TAN", "THEN", "TIME$", "TO", "VAL", "VARPTR"
        };

        /// <summary>
        /// Gets the current configuration
        /// </summary>
        public CompilerConfig Config { get; }

        /// <summary>
        /// Creates a new instance with the given configuration
        /// </summary>
        /// <param name="Config">Configuration</param>
        public Compiler(CompilerConfig Config)
        {
            this.Config = Config ?? throw new ArgumentNullException(nameof(Config));
        }

        /// <summary>
        /// Compiles code
        /// </summary>
        /// <param name="Lines">Source code lines</param>
        /// <param name="Functions">
        /// Function definitions.
        /// See <see cref="Project.GetFunctions"/>
        /// </param>
        /// <returns>Compilation result</returns>
        public CompilerResult Compile(string[] Lines, FunctionDeclaration[] Functions)
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
            var FunctionNames = Functions.Select(m => m.FunctionName.ToUpper()).ToArray();

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
                Parts.RemoveAll(IsArg);
                if (Config.StripComments)
                {
                    Parts.RemoveAll(IsComment);
                }
                if (Config.StripDebug)
                {
                    Parts.RemoveAll(IsDebug);
                }
                else
                {
                    Parts = Parts.Select(StripDebugSymbol).ToList();
                }
                if (Parts.Count == 0)
                {
                    continue;
                }

                //Handle label. Labels are only allowed in the first part
                if (Parts.Skip(1).Any(m => IsLabel(m)))
                {
                    Ret.AddMessage(new SyntaxError(i - SourceOffset, SyntaxErrorType.Error, "Label only allowed at the start of a concatenated line", FunctionName));
                }
                if (IsLabel(Parts[0]))
                {
                    //Match label but without arguments
                    var LineLabel = Regex.Match(Parts[0], @"@\w+").Value.ToUpper().Trim();
                    var FunctionPreview = LineLabel.Substring(1);
                    if (Labels.ContainsKey(Parts[0]))
                    {
                        Ret.AddMessage(new SyntaxError(i - SourceOffset, SyntaxErrorType.Error, $"Label already defined: {Parts[0]}", FunctionName));
                    }
                    if (Label != null)
                    {
                        Ret.AddMessage(new SyntaxError(i - SourceOffset, SyntaxErrorType.Error, $"Duplicate label for same line. Label: {Parts[0]}", FunctionName));
                    }
                    //If the label is not in the known function list it's a manually created label.
                    if (FunctionNames.Contains(FunctionPreview.ToUpper()))
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
                        var CallName = GetCallLabel(Parts[j]).ToLower();
                        var CallFunc = Functions.FirstOrDefault(m => m.FunctionName.ToLower() == CallName);
                        try
                        {
                            if (CallFunc == null)
                            {
                                throw new Exception($"Function does not exist: {CallName}");
                            }
                            if (j < Parts.Count - 1 && !IsComment(Parts[j + 1]))
                            {
                                Ret.AddMessage(new SyntaxError(i - SourceOffset, SyntaxErrorType.Warning, $"Useless instruction after function call: {Parts[j + 1]}", FunctionName));
                            }
                            var instructions = FormatCall(Parts[j], Ret, i - SourceOffset, Functions.First(m => m.FunctionName.ToUpper() == FunctionName.ToUpper()), CallFunc);
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
                    Code = string.Join(" : ", Parts.Where(m => !string.IsNullOrWhiteSpace(m))),
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

        /// <summary>
        /// Formats a CALL statement and converts it into legacy code
        /// </summary>
        /// <param name="L">CALL line</param>
        /// <param name="Res">Existing compiler result object</param>
        /// <param name="LineNumber">Current line number</param>
        /// <param name="CurrentFunction">Function the line is located at</param>
        /// <param name="CalledFunction">Function the line calls</param>
        /// <returns>BASIC Lines</returns>
        private string[] FormatCall(string L, CompilerResult Res, int LineNumber, FunctionDeclaration CurrentFunction, FunctionDeclaration CalledFunction)
        {
            var Lines = new List<string>();
            string[] Arguments;
            if (!IsCall(L))
            {
                throw new ArgumentException($"Line not a label call statement: {L}");
            }
            //Simple call statement
            var M = Regex.Match(L, @"^\s*CALL\s*(@\w+)\s*(?:\((.+)\))?");
            if (M.Success)
            {
                var SimpleArgs = M.Groups[2].Value.Trim();
                if (!string.IsNullOrEmpty(SimpleArgs))
                {
                    Arguments = ParseArguments(SimpleArgs);
                    if (Arguments.Length != CalledFunction.Arguments.Length)
                    {
                        throw new ArgumentException($"Number of supplied arguments ({Arguments.Length}) doesn't matches requested number of arguments ({CalledFunction.Arguments.Length})");
                    }
                    Lines.Add(string.Join(" : ", Arguments.Select((v, i) => $"{CalledFunction.Arguments[i]}={v}")) + ":GOSUB " + M.Groups[1].Value);
                }
                else
                {
                    Lines.Add("GOSUB " + M.Groups[1].Value);
                }
                return Lines.ToArray();
            }
            //Extended call statement
            M = Regex.Match(L, @"^\s*([^=]+)=\s*CALL([" + Types + @"]?)\s*(@\w+)\s*(?:\((.+)\))?");
            if (!M.Success)
            {
                throw new ArgumentException("Not a valid extended CALL statement");
            }
            var Assignment = M.Groups[1].Value.TrimEnd();
            var Type = M.Groups[2].Value;
            var Label = M.Groups[3].Value;
            var ComplexArgs = M.Groups[4].Value.Trim();
            if (!string.IsNullOrEmpty(ComplexArgs))
            {
                Arguments = ParseArguments(ComplexArgs);
                if (Arguments.Length != CalledFunction.Arguments.Length)
                {
                    throw new ArgumentException($"Number of supplied arguments ({Arguments.Length}) doesn't matches requested number of arguments ({CalledFunction.Arguments.Length})");
                }
                Lines.Add(string.Join(" : ", Arguments.Select((v, i) => $"{CalledFunction.Arguments[i]}={v}")));
            }

            //Infer type from assignment
            if (string.IsNullOrEmpty(Type))
            {
                Type = Assignment[Assignment.Length - 1].ToString();
                if (!Types.Contains(Type))
                {
                    Type = DefaultType;
                    Res.AddMessage(new SyntaxError(LineNumber, SyntaxErrorType.Warning, $"Unable to infer type of extended CALL from assignment. Using default type: {DefaultType}", CurrentFunction.FunctionName));
                }
            }
            var IsString = Type == "$";
            var BaseValue = IsString ? "\"\"" : "0";
            var RetType = IsString ? '$' : '#';
            if (Lines.Count > 0)
            {
                Lines[0] += " : ";
            }
            else
            {
                Lines.Add(string.Empty);
            }
            Lines[0] += $"{Config.ReturnVar}{RetType}={BaseValue} : GOSUB {Label}";
            Lines.Add($"{Assignment}={Config.ReturnVar}{RetType}");
            return Lines.ToArray();
        }

        /// <summary>
        /// Formats a RETURN statement and converts it into legacy code
        /// </summary>
        /// <param name="L">CALL line</param>
        /// <param name="Res">Existing compiler result object</param>
        /// <param name="LineNumber">Current line number</param>
        /// <param name="FunctionName">Name of current function</param>
        /// <returns>BASIC line</returns>
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
                //Handle constant strings
                if (Type == "\"") { Type = "$"; }
                //Handle constant numbers
                if ("0123456789".Contains(Type)) { Type = "#"; }
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

        /// <summary>
        /// Strips the debug symbol (if present) from a command
        /// </summary>
        /// <param name="Command">Command</param>
        /// <returns>Command without debug symbol</returns>
        /// <remarks>This strips regardless of <see cref="CompilerConfig.StripDebug"/> setting</remarks>
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

        /// <summary>
        /// Returns all distinct labels found in the given lines
        /// </summary>
        /// <param name="Lines">Code lines</param>
        /// <returns>Labels</returns>
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

        /// <summary>
        /// Checks if the command is an ARG statement
        /// </summary>
        /// <param name="Command">Command</param>
        /// <returns>true if ARG command</returns>
        public static bool IsArg(string Command)
        {
            return Regex.IsMatch(Command.ToUpper(), @"^\s*ARG\s*\S");
        }

        /// <summary>
        /// Checks if the command is a RETURN statement
        /// </summary>
        /// <param name="L">BASIC Line</param>
        /// <returns>true, if RETURN</returns>
        public static bool IsReturn(string L)
        {
            return L.Trim().ToUpper().StartsWith("RETURN");
        }

        /// <summary>
        /// Checks if the entire instruction is a comment
        /// </summary>
        /// <param name="L">Command</param>
        /// <returns>true, if a comment (REM or single quote)</returns>
        public static bool IsComment(string L)
        {
            return L.Trim().ToUpper().StartsWith("REM") || L.Trim().StartsWith("'");
        }

        /// <summary>
        /// Gets if the instruction is only to be run in debug mode
        /// </summary>
        /// <param name="L">Instruction</param>
        /// <returns>true, if debug instruction</returns>
        public static bool IsDebug(string L)
        {
            return L.TrimStart().StartsWith("#");
        }

        /// <summary>
        /// Gets if the instruction is a function call
        /// </summary>
        /// <param name="L">Instruction</param>
        /// <returns>true, if function call</returns>
        /// <remarks>This will not consider GOSUB a function call</remarks>
        public static bool IsCall(string L)
        {
            L = L.Trim().ToUpper();
            if (IsComment(L))
            {
                return false;
            }
            return
                Regex.IsMatch(L, @"^\s*CALL\s*@\w+\s*(\(.*\))?", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(L, @"^\s*[^=]+=\s*CALL[" + Types + @"]?\s*@\w+\s*(\(.*\))?", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Gets the label component of a CALL instruction
        /// </summary>
        /// <param name="L">Command</param>
        /// <returns>Label component</returns>
        public static string GetCallLabel(string L)
        {
            L = L.Trim().ToUpper();
            var M = Regex.Match(L, @"^\s*CALL\s*@(\w+)", RegexOptions.IgnoreCase);
            if (M.Success)
            {
                return M.Groups[1].Value;
            }
            M = Regex.Match(L, @"^\s*[^=]+=\s*CALL[" + Types + @"]?\s*@(\w+)", RegexOptions.IgnoreCase);
            if (M.Success)
            {
                return M.Groups[1].Value;
            }
            throw new ArgumentException($"Not a valid call statement: {L}");
        }

        /// <summary>
        /// Checks if the line is a label
        /// </summary>
        /// <param name="L">Line</param>
        /// <returns>true, if label</returns>
        /// <remarks>Labels are only valid at the start of a line</remarks>
        public static bool IsLabel(string L)
        {
            return !string.IsNullOrWhiteSpace(L) && Regex.IsMatch(L, @"^\s*@\w+\s*(?:\(.*\))?\s*$");
        }

        /// <summary>
        /// Checks if the line has a label reference anywhere
        /// </summary>
        /// <param name="L">Line</param>
        /// <returns>true, if label reference present</returns>
        /// <remarks>This properly ignores strings</remarks>
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

        /// <summary>
        /// Parses function arguments into a list of values
        /// </summary>
        /// <param name="Args">Argument string (contents between parenthesis)</param>
        /// <returns>Argument list</returns>
        public static string[] ParseArguments(string Args)
        {
            if (string.IsNullOrWhiteSpace(Args))
            {
                return new string[0];
            }
            List<string> Parsed = new List<string>();
            int level = 0;
            bool InStr = false;
            string buffer = string.Empty;
            foreach (var C in Args)
            {
                //Add characters in a string as-is
                if (C != '"' && InStr)
                {
                    buffer += C;
                    continue;
                }
                //Handle string start and end
                if (C == '"')
                {
                    buffer += C;
                    InStr = !InStr;
                    continue;
                }
                //Handle argument comma if at level zero
                if (C == ',' && level == 0)
                {
                    Parsed.Add(buffer.Trim());
                    buffer = string.Empty;
                    continue;
                }
                if (C == '(')
                {
                    buffer += C;
                    ++level;
                    continue;
                }
                if (C == ')')
                {
                    if (--level < 0)
                    {
                        throw new ArgumentException("Too many closing parenthesis");
                    }
                    buffer += C;
                    continue;
                }
                //No case matched. Add character as-is
                buffer += C;
            }
            if (level != 0)
            {
                throw new ArgumentException("Too many opening parenthesis");
            }
            Parsed.Add(buffer.Trim());
            return Parsed.ToArray();
        }
    }
}
