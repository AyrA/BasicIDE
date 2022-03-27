using System;

namespace BasicIDE.Basic
{
    public class SyntaxError : IValidateable
    {
        public int LineIndex { get; } = -1;

        public SyntaxErrorType ErrorType { get; }

        public string Message { get; }

        public string FunctionName { get; }

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

        public SyntaxError(SyntaxErrorType Type, string Message = null, string FunctionName = null)
        {
            ErrorType = Type;
            this.Message = Message;
            this.FunctionName = FunctionName;
            Validate();
        }

        public void Validate()
        {
            if (!Enum.IsDefined(typeof(SyntaxErrorType), ErrorType))
            {
                throw new ValidationException(nameof(ErrorType), "Invalid enumeration value");
            }
        }

        public override string ToString()
        {
            return $"Line {LineIndex + 1}: [{ErrorType}] {Message}";
        }
    }

    public enum SyntaxErrorType
    {
        Info = 0,
        Warning = 1,
        Error = 2
    }
}
