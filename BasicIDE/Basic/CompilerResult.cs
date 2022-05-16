using System.Collections.Generic;
using System.Linq;

namespace BasicIDE.Basic
{
    /// <summary>
    /// Represents the result of <see cref="Compiler.Compile(string[], FunctionDeclaration[])"/>
    /// </summary>
    public class CompilerResult
    {
        /// <summary>
        /// Message types
        /// </summary>
        private readonly Dictionary<SyntaxErrorType, List<SyntaxError>> messages =
            new Dictionary<SyntaxErrorType, List<SyntaxError>>()
            {
                { SyntaxErrorType.Info,    new List<SyntaxError>() },
                { SyntaxErrorType.Warning, new List<SyntaxError>() },
                { SyntaxErrorType.Error,   new List<SyntaxError>() },
            };

        /// <summary>
        /// Gets all error messages
        /// </summary>
        public SyntaxError[] Errors { get => messages[SyntaxErrorType.Error].ToArray(); }

        /// <summary>
        /// Gets all warning messages
        /// </summary>
        public SyntaxError[] Warnings { get => messages[SyntaxErrorType.Warning].ToArray(); }

        /// <summary>
        /// Gets all informational messages
        /// </summary>
        public SyntaxError[] Infos { get => messages[SyntaxErrorType.Info].ToArray(); }

        /// <summary>
        /// Gets or sets compiled lines
        /// </summary>
        public Line[] Lines { get; set; }

        /// <summary>
        /// Gets whether warnings are treated as errors
        /// </summary>
        public bool TreatWarningsAsErrors { get; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="TreatWarningsAsErrors">Treat warnings as errors</param>
        public CompilerResult(bool TreatWarningsAsErrors)
        {
            this.TreatWarningsAsErrors = TreatWarningsAsErrors;
        }

        /// <summary>
        /// Adds a message based on a syntax error type
        /// </summary>
        /// <param name="ex">Syntax error</param>
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

        /// <summary>
        /// Gets raw compiled lines
        /// </summary>
        /// <returns>Compiled lines</returns>
        public string[] GetLines()
        {
            return Lines.Select(m => m.ToLine()).ToArray();
        }
    }
}
