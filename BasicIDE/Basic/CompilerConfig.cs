namespace BasicIDE.Basic
{
    /// <summary>
    /// Controls behavior of the compiler
    /// </summary>
    public class CompilerConfig
    {
        /// <summary>
        /// Gets or sets the number to begin numbering BASIC lines at
        /// </summary>
        public int StartLine { get; set; }

        /// <summary>
        /// Gets or sets the line number increment for BASIC lines
        /// </summary>
        public int LineIncrement { get; set; }

        /// <summary>
        /// Gets or sets the variable used to handle function returns
        /// </summary>
        public string ReturnVar { get; set; }

        /// <summary>
        /// Gets or sets whether to strip comments
        /// </summary>
        /// <remarks>Only commands entirely consisting of a comment will be stripped</remarks>
        public bool StripComments { get; set; }

        /// <summary>
        /// Gets or sets whether to strip debug instructions
        /// </summary>
        public bool StripDebug { get; set; }

        /// <summary>
        /// Gets or sets whether to treat warnings as errors
        /// </summary>
        public bool TreatWarningsAsErrors { get; set; }

        /// <summary>
        /// Creates a default instance
        /// </summary>
        public CompilerConfig()
        {
            StartLine = 10;
            LineIncrement = 10;
            ReturnVar = "ZZ";
            StripComments = true;
            StripDebug = false;
            TreatWarningsAsErrors = false;
        }

        /// <summary>
        /// Gets a configuration suitable for release mode builds
        /// </summary>
        public static CompilerConfig Release
        {
            get
            {
                return new CompilerConfig()
                {
                    LineIncrement = 1,
                    StartLine = 1,
                    StripDebug = true,
                    StripComments = true,
                    TreatWarningsAsErrors = true
                };
            }
        }

        /// <summary>
        /// Gets a configuration suitable for debug mode builds
        /// </summary>
        public static CompilerConfig Debug
        {
            get
            {
                return new CompilerConfig()
                {
                    LineIncrement = 10,
                    StartLine = 10,
                    StripComments = false,
                    StripDebug = false,
                    TreatWarningsAsErrors = false
                };
            }
        }
    }
}
