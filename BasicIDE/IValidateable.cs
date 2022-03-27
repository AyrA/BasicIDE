using System;

namespace BasicIDE
{
    public interface IValidateable
    {
        /// <summary>
        /// Validates the current instance and throws an exception on problems
        /// </summary>
        void Validate();
    }

    public class ValidationException : Exception
    {
        public string ParamName { get; }

        public ValidationException(string ParamName, string Message, Exception InnerException) : base(Message, InnerException)
        {
            if (string.IsNullOrWhiteSpace(ParamName))
            {
                throw new ArgumentException($"'{nameof(ParamName)}' cannot be null or whitespace.", nameof(ParamName));
            }

            this.ParamName = ParamName;
        }

        public ValidationException(string ParamName, string Message) : base(Message)
        {
            if (string.IsNullOrWhiteSpace(ParamName))
            {
                throw new ArgumentException($"'{nameof(ParamName)}' cannot be null or whitespace.", nameof(ParamName));
            }

            this.ParamName = ParamName;
        }
    }
}
