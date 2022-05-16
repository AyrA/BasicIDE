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

    /// <summary>
    /// Represents a validation problem
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Gets the name of the parameter that failed validation
        /// </summary>
        public string ParamName { get; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="ParamName">Parameter name</param>
        /// <param name="Message">Validation error</param>
        /// <param name="InnerException">Base exception</param>
        public ValidationException(string ParamName, string Message, Exception InnerException) : base(Message, InnerException)
        {
            if (string.IsNullOrWhiteSpace(ParamName))
            {
                throw new ArgumentException($"'{nameof(ParamName)}' cannot be null or whitespace.", nameof(ParamName));
            }

            this.ParamName = ParamName;
        }

        /// <summary>
        /// Creates a new instance without an exception
        /// </summary>
        /// <param name="ParamName">Parameter name</param>
        /// <param name="Message">Validation error</param>
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
