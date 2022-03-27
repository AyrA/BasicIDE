using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace BasicIDE
{
    public class Project
    {
        [Serializable]
        public class ProjectData
        {
            public string Title { get; set; }
            public string MainFunction { get; set; }
        }

        private const string Extension = ".bas";
        private const string DefaultCode = "PRINT \"Hello, World!\"";

        private static readonly Regex Pattern = new Regex(@"^\w+$");
        private string main;

        public string Title { get; set; }
        [XmlIgnore]
        public string ProjectDirectory { get; private set; }
        public string MainFunction
        {
            get { return main; }
        }

        public Project()
        {
            Title = "New project";
            main = "main";
        }

        public Project(string FileName)
        {
            if (string.IsNullOrWhiteSpace(FileName))
            {
                throw new ArgumentException($"'{nameof(FileName)}' cannot be null or whitespace.", nameof(FileName));
            }

            FileName = Path.GetFullPath(FileName);
            var Data = File.ReadAllText(FileName).FromXML<ProjectData>();
            ProjectDirectory = Path.Combine(
                Path.GetDirectoryName(FileName),
                Path.GetFileNameWithoutExtension(FileName));
            Title = Data.Title;
            main = Data.MainFunction;
            if (!IsValidFunctionName(main))
            {
                throw new Exception("Config load error. Main function property value not defined or invalid.");
            }
            CheckMain();
        }

        public static bool IsValidFunctionName(string FunctionName)
        {
            return !string.IsNullOrWhiteSpace(FunctionName) && Pattern.IsMatch(FunctionName);
        }

        public void SetMainFunction(string FunctionName)
        {
            if (string.IsNullOrEmpty(ProjectDirectory))
            {
                throw new InvalidOperationException("Cannot set main function if project directory is not defined yet");
            }
            if (!IsValidFunctionName(FunctionName))
            {
                throw new ArgumentException("Invalid function name");
            }
            //Don't do anything if the value is identical
            if (FunctionName.ToLower() == main?.ToLower())
            {
                return;
            }
            if (main == null && HasFunction(FunctionName))
            {
                throw new ArgumentException("Function already exists");
            }
            if (main != null)
            {
                RenameFunction(MainFunction, FunctionName);
            }
            main = FunctionName;
        }

        public void AddFunction(string FunctionName)
        {
            if (HasFunction(FunctionName))
            {
                throw new InvalidOperationException("Function already exists");
            }
            SaveFunction(FunctionName, DefaultCode);
        }

        private string GetName(string FunctionName)
        {
            return Path.Combine(ProjectDirectory, FunctionName) + Extension;
        }

        private void CheckMain()
        {
            var P = Path.Combine(ProjectDirectory, main + Extension);
            if (!File.Exists(P))
            {
                File.WriteAllText(P, DefaultCode);
            }
        }

        public IEnumerable<string> GetFunctions()
        {
            foreach (var S in Directory.EnumerateFiles(ProjectDirectory, "*.bas"))
            {
                yield return Path.GetFileNameWithoutExtension(S);
            }
        }

        public bool HasFunction(string FunctionName)
        {
            if (!IsValidFunctionName(FunctionName) || string.IsNullOrEmpty(ProjectDirectory))
            {
                return false;
            }
            return File.Exists(GetName(FunctionName));
        }

        public string[] GetFunctionCode(string FunctionName)
        {
            if (!IsValidFunctionName(FunctionName))
            {
                throw new ArgumentException("Invalid function name format");
            }
            try
            {
                return File.ReadAllLines(GetName(FunctionName));
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to read {GetName(FunctionName)}", ex);
            }
        }

        public void SaveFunction(string FunctionName, string Code)
        {
            if (!IsValidFunctionName(FunctionName))
            {
                throw new ArgumentException("Invalid function name format");
            }
            try
            {
                File.WriteAllText(GetName(FunctionName), Code);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to write {GetName(FunctionName)}", ex);
            }
        }

        public void RenameFunction(string FunctionName, string NewName)
        {
            if (!IsValidFunctionName(FunctionName))
            {
                throw new ArgumentException("Invalid function name format for old name");
            }
            if (!IsValidFunctionName(NewName))
            {
                throw new ArgumentException("Invalid function name format for new name");
            }
            var Old = GetName(FunctionName);
            var New = GetName(NewName);
            try
            {
                File.Move(Old, New);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to rename {FunctionName} to {NewName}", ex);
            }
        }

        public void DeleteFunction(string FunctionName)
        {
            if (!IsValidFunctionName(FunctionName))
            {
                throw new ArgumentException("Invalid function name format");
            }
            try
            {
                File.Delete(GetName(FunctionName));
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete the function {FunctionName}", ex);
            }
        }

        public string[] GetAllCode()
        {
            if (!File.Exists(GetName(MainFunction)))
            {
                throw new InvalidOperationException("Main function does not exist");
            }
            //Main function always comes first
            List<string> Lines = new List<string>
            {
                $"@{MainFunction}"
            };
            Lines.AddRange(GetFunctionCode(MainFunction));
            Lines.Add("END");

            foreach (var F in GetFunctions())
            {
                //Main function will be in the list again and needs to be skipped
                if (F.ToLower() == MainFunction.ToLower())
                {
                    continue;
                }
                Lines.Add("");
                Lines.Add($"@{F}");
                Lines.AddRange(GetFunctionCode(F));
                var LastLine = Basic.Compiler.SplitLine(Lines[Lines.Count - 1]);
                if (!LastLine.Any(Basic.Compiler.IsReturn))
                {
                    Lines.Add("RETURN");
                }
            }
            return Lines.ToArray();
        }

        public void SaveAs(string FileName)
        {
            if (string.IsNullOrWhiteSpace(FileName))
            {
                throw new ArgumentException($"'{nameof(FileName)}' cannot be null or whitespace.", nameof(FileName));
            }
            if (!FileName.ToLower().EndsWith(".t80"))
            {
                throw new ArgumentException("Project file must have .t80 extension");
            }
            var PD = new ProjectData()
            {
                MainFunction = main,
                Title = Title
            };
            FileName = Path.GetFullPath(FileName);
            File.WriteAllText(FileName, PD.ToXML());
            var NewDirectory =
                Path.Combine(
                Path.GetDirectoryName(FileName),
                Path.GetFileNameWithoutExtension(FileName));
            try
            {
                Directory.CreateDirectory(NewDirectory);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot create project directory", ex);
            }
            //Copy all files to the new destination if this is an existing project
            if (!string.IsNullOrEmpty(ProjectDirectory))
            {
                foreach (var F in GetFunctions())
                {
                    File.Copy(GetName(F), Path.Combine(NewDirectory, $"{F}{Extension}"));
                }
            }
            ProjectDirectory = NewDirectory;
            if (string.IsNullOrEmpty(Title))
            {
                Title = Path.GetFileNameWithoutExtension(FileName);
            }
            CheckMain();
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(ProjectDirectory))
            {
                throw new InvalidOperationException("Project directory is not set. Set it manually or use \"Save As\" first.");
            }
            var PD = new ProjectData()
            {
                MainFunction = main,
                Title = Title
            };
            File.WriteAllText(ProjectDirectory + ".t80", PD.ToXML());
            CheckMain();
        }
    }
}
