namespace BasicIDE.Basic
{
    /// <summary>
    /// Represents a compiler code line
    /// </summary>
    public class Line
    {
        /// <summary>
        /// Gets or sets raditional BASIC ine number
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets compiled code line
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the label of the line (if any)
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the source line index
        /// </summary>
        public int LineIndex { get; set; }

        /// <summary>
        /// Gets or sets the current function name
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// Gets a formatted display string
        /// </summary>
        /// <returns>Formatted display string</returns>
        public override string ToString()
        {
            return $"{FunctionName}:{Number} {Code}";
        }

        /// <summary>
        /// Converts this instance into a raw compiled BASIC line
        /// </summary>
        /// <returns>Code line</returns>
        public string ToLine()
        {
            return $"{Number} {Code}";
        }
    }
}
