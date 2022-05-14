using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BasicIDE.Basic
{
    public class FunctionDeclaration
    {
        private readonly List<string> args;

        public string FunctionName { get; }

        public string[] Arguments => args.ToArray();

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

        public override string ToString()
        {
            var Params = string.Join(",", args);
            return $"{FunctionName}({Params})";
        }
    }
}
