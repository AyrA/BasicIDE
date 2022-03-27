namespace BasicIDE.Basic
{
    public class CompilerConfig
    {
        public int StartLine { get; set; }

        public int LineIncrement { get; set; }

        public string ReturnVar { get; set; }

        public bool StripComments { get; set; }

        public bool StripDebug { get; set; }

        public bool TreatWarningsAsErrors { get; set; }

        public CompilerConfig()
        {
            StartLine = 10;
            LineIncrement = 10;
            ReturnVar = "ZZ";
            StripComments = true;
            StripDebug = false;
            TreatWarningsAsErrors = false;
        }

        public static CompilerConfig Release
        {
            get
            {
                return new CompilerConfig()
                {
                    LineIncrement = 1,
                    StartLine = 1,
                    StripDebug = true
                };
            }
        }

        public static CompilerConfig Debug
        {
            get
            {
                return new CompilerConfig()
                {
                    LineIncrement = 10,
                    StartLine = 10,
                    StripComments = false
                };
            }
        }
    }
}
