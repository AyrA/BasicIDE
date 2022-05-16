using System;

namespace BasicIDE.Basic
{
    /// <summary>
    /// Represents a syntax message
    /// </summary>
    public class SyntaxError : IValidateable
    {
        /// <summary>
        /// Gets the source index of the line
        /// </summary>
        public int LineIndex { get; } = -1;

        /// <summary>
        /// Gets the type of message
        /// </summary>
        public SyntaxErrorType ErrorType { get; }

        /// <summary>
        /// Gets the message string
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the current function name
        /// </summary>
        public string FunctionName { get; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="Line">Line index</param>
        /// <param name="Type">Message type</param>
        /// <param name="Message">Message string</param>
        /// <param name="FunctionName">Current function name</param>
        public SyntaxError(int Line, SyntaxErrorType Type, string Message = null, string FunctionName = null)
        {
            if (Line < 0)
            {
                throw new ArgumentException("Negative line number is not allowed");
            }
            LineIndex = Line;
            ErrorType = Type;
            this.Message = Message;
            this.FunctionName = FunctionName;
            Validate();
        }

        /// <summary>
        /// Creates a new instance for when no line index is present
        /// </summary>
        /// <param name="Type">Message type</param>
        /// <param name="Message">Message string</param>
        /// <param name="FunctionName">Current function name</param>
        public SyntaxError(SyntaxErrorType Type, string Message = null, string FunctionName = null)
        {
            ErrorType = Type;
            this.Message = Message;
            this.FunctionName = FunctionName;
            Validate();
        }

        /// <summary>
        /// Validates this instance
        /// </summary>
        public void Validate()
        {
            if (!Enum.IsDefined(typeof(SyntaxErrorType), ErrorType))
            {
                throw new ValidationException(nameof(ErrorType), "Invalid enumeration value");
            }
        }

        /// <summary>
        /// Gets a display string
        /// </summary>
        /// <returns>Display string</returns>
        public override string ToString()
        {
            return $"Line {LineIndex + 1}: [{ErrorType}] {Message}";
        }
    }

    /// <summary>
    /// Possible syntax error message types
    /// </summary>
    public enum SyntaxErrorType
    {
        /// <summary>
        /// Informational message
        /// </summary>
        /// <remarks>These are usually improvements the programmer could make</remarks>
        Info = 0,
        /// <summary>
        /// Warning message
        /// </summary>
        /// <remarks>Highlights potential mistakes</remarks>
        Warning = 1,
        /// <summary>
        /// Error message
        /// </summary>
        /// <remarks>Problems that prevent compilation</remarks>
        Error = 2
    }
}
