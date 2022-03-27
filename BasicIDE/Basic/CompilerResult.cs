using System.Collections.Generic;
using System.Linq;

namespace BasicIDE.Basic
{
    public class CompilerResult
    {
        private readonly Dictionary<SyntaxErrorType, List<SyntaxError>> messages =
            new Dictionary<SyntaxErrorType, List<SyntaxError>>()
            {
                { SyntaxErrorType.Info,    new List<SyntaxError>() },
                { SyntaxErrorType.Warning, new List<SyntaxError>() },
                { SyntaxErrorType.Error,   new List<SyntaxError>() },
            };
        public SyntaxError[] Errors { get => messages[SyntaxErrorType.Error].ToArray(); }
        public SyntaxError[] Warnings { get => messages[SyntaxErrorType.Warning].ToArray(); }
        public SyntaxError[] Infos { get => messages[SyntaxErrorType.Info].ToArray(); }

        public Line[] Lines { get; set; }

        public bool TreatWarningsAsErrors { get; }

        public CompilerResult(bool TreatWarningsAsErrors)
        {
            this.TreatWarningsAsErrors = TreatWarningsAsErrors;
        }

        public void AddMessage(SyntaxError ex)
        {
            ex.Validate();
            if (TreatWarningsAsErrors && ex.ErrorType == SyntaxErrorType.Warning)
            {
                messages[SyntaxErrorType.Error].Add(ex);
            }
            else
            {
                messages[ex.ErrorType].Add(ex);
            }
        }

        public string[] GetLines()
        {
            return Lines.Select(m => m.ToLine()).ToArray();
        }
    }
}
