using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace BasicIDE
{
    /// <summary>
    /// Represents a project file
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Project metadata
        /// </summary>
        [Serializable]
        public class ProjectData
        {
            /// <summary>
            /// Project name
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// Name of main function
            /// </summary>
            public string MainFunction { get; set; }
        }

        /// <summary>
        /// Code file extension
        /// </summary>
        private const string Extension = ".bas";
        /// <summary>
        /// Inidial code for main function
        /// </summary>
        private const string DefaultCode = "PRINT \"Hello, World!\"";

        /// <summary>
        /// Pattern for valid function names
        /// </summary>
        private static readonly Regex Pattern = new Regex(@"^\w+$");

        /// <summary>
        /// Name of main function
        /// </summary>
        private string main;

        /// <summary>
        /// Gets or sets the project name
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets the project file directory
        /// </summary>
        [XmlIgnore]
        public string ProjectDirectory { get; private set; }

        /// <summary>
        /// Gets the name of the main function
        /// </summary>
        public string MainFunction
        {
            get { return main; }
        }

        /// <summary>
        /// Creates a new, empty project
        /// </summary>
        public Project()
        {
            Title = "New project";
            main = "main";
        }

        /// <summary>
        /// Loads a project from file
        /// </summary>
        /// <param name="FileName">Project file name</param>
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

        /// <summary>
        /// Checks if the function name is valid
        /// </summary>
        /// <param name="FunctionName">Function name</param>
        /// <returns>true, if valid</returns>
        public static bool IsValidFunctionName(string FunctionName)
        {
            return !string.IsNullOrWhiteSpace(FunctionName) && Pattern.IsMatch(FunctionName);
        }

        /// <summary>
        /// Rename the main function
        /// </summary>
        /// <param name="FunctionName">New function name</param>
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

        /// <summary>
        /// Adds a new function to the project
        /// </summary>
        /// <param name="FunctionName">Function name</param>
        public void AddFunction(string FunctionName)
        {
            if (HasFunction(FunctionName))
            {
                throw new InvalidOperationException("Function already exists");
            }
            SaveFunction(FunctionName, DefaultCode);
        }

        /// <summary>
        /// Gets the full file name that contains the given function
        /// </summary>
        /// <param name="FunctionName">Function name</param>
        /// <returns>File name</returns>
        /// <remarks>File does not needs to exist. Name is not checked for validity</remarks>
        private string GetName(string FunctionName)
        {
            return Path.Combine(ProjectDirectory, FunctionName) + Extension;
        }

        /// <summary>
        /// Checks if main function exists, and if not, creates it
        /// </summary>
        private void CheckMain()
        {
            var P = Path.Combine(ProjectDirectory, main + Extension);
            if (!File.Exists(P))
            {
                File.WriteAllText(P, DefaultCode);
            }
        }

        /// <summary>
        /// Gets all functions
        /// </summary>
        /// <returns>Function declarations</returns>
        public IEnumerable<Basic.FunctionDeclaration> GetFunctions()
        {
            foreach (var S in Directory.EnumerateFiles(ProjectDirectory, "*.bas"))
            {
                var Lines = File.ReadAllLines(S);
                yield return new Basic.FunctionDeclaration(Path.GetFileNameWithoutExtension(S), Lines);
            }
        }

        /// <summary>
        /// Checks if a given function exists
        /// </summary>
        /// <param name="FunctionName">Function name</param>
        /// <returns>true, if it exists</returns>
        public bool HasFunction(string FunctionName)
        {
            if (!IsValidFunctionName(FunctionName) || string.IsNullOrEmpty(ProjectDirectory))
            {
                return false;
            }
            return File.Exists(GetName(FunctionName));
        }

        /// <summary>
        /// Gets the code of a function
        /// </summary>
        /// <param name="FunctionName">Function name</param>
        /// <returns>Code lines</returns>
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

        /// <summary>
        /// Saves code for a function
        /// </summary>
        /// <param name="FunctionName">Function name</param>
        /// <param name="Code">Function code</param>
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

        /// <summary>
        /// Renames a function
        /// </summary>
        /// <param name="FunctionName">Old function name</param>
        /// <param name="NewName">New function name</param>
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

        /// <summary>
        /// Deletes a function
        /// </summary>
        /// <param name="FunctionName">Function name</param>
        public void DeleteFunction(string FunctionName)
        {
            if (!IsValidFunctionName(FunctionName))
            {
                throw new ArgumentException("Invalid function name format");
            }
            if (FunctionName.ToLower() == MainFunction.ToLower())
            {
                throw new ArgumentException("You cannot delete the main function");
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

        /// <summary>
        /// Gets all code from all functions
        /// </summary>
        /// <returns>All code</returns>
        /// <remarks>
        /// Code of main function always comes first.
        /// Order of other code is undefined.
        /// </remarks>
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
                if (F.FunctionName.ToLower() == MainFunction.ToLower())
                {
                    continue;
                }
                Lines.Add("");
                Lines.Add($"@{F}");
                Lines.AddRange(GetFunctionCode(F.FunctionName));
                var LastLine = Basic.Compiler.SplitLine(Lines[Lines.Count - 1]);
                if (!LastLine.Any(Basic.Compiler.IsReturn))
                {
                    Lines.Add("RETURN");
                }
            }
            return Lines.ToArray();
        }

        /// <summary>
        /// Save project file under new name
        /// </summary>
        /// <param name="FileName">Project file name</param>
        /// <remarks>This will also copy all code if this is an existing project</remarks>
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
                    File.Copy(GetName(F.FunctionName), Path.Combine(NewDirectory, $"{F}{Extension}"));
                }
            }
            ProjectDirectory = NewDirectory;
            if (string.IsNullOrEmpty(Title))
            {
                Title = Path.GetFileNameWithoutExtension(FileName);
            }
            CheckMain();
        }

        /// <summary>
        /// Save project with existing file name
        /// </summary>
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
