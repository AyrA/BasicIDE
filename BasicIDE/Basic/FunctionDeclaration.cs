using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BasicIDE.Basic
{
    /// <summary>
    /// Represents a BASIC function declaration
    /// </summary>
    public class FunctionDeclaration
    {
        /// <summary>
        /// List of function arguments
        /// </summary>
        private readonly List<string> args;

        /// <summary>
        /// Gets the name of the function
        /// </summary>
        public string FunctionName { get; }

        /// <summary>
        /// Gets function arguments
        /// </summary>
        public string[] Arguments => args.ToArray();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="FunctionName">Function name</param>
        /// <param name="Lines">Function code</param>
        public FunctionDeclaration(string FunctionName, string[] Lines)
        {
            if (string.IsNullOrWhiteSpace(FunctionName))
            {
                throw new ArgumentException($"'{nameof(FunctionName)}' cannot be null or whitespace.", nameof(FunctionName));
            }

            this.FunctionName = FunctionName;
            args = new List<string>();

            //Matches a line in the format ARG <variable> [comment]
            var Matcher = new Regex(@"^\s*ARG\s+(\w+[$#!%])\s*(?:'.*)?$", RegexOptions.IgnoreCase);
            foreach (var Line in Lines)
            {
                var M = Matcher.Match(Line);
                if (M.Success)
                {
                    var Arg = M.Groups[1].Value.ToUpper();
                    if (args.Contains(Arg))
                    {
                        throw new Exception("Duplicate argument: " + Arg);
                    }
                    args.Add(Arg);
                }
            }
        }

        /// <summary>
        /// Returns a display string in the format "func(arg, arg, ...)"
        /// </summary>
        /// <returns>Display string</returns>
        public override string ToString()
        {
            var Params = string.Join(", ", args);
            return $"{FunctionName}({Params})";
        }
    }
}
