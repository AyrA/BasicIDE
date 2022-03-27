using System;

namespace BasicIDE.Basic
{
    public class Line
    {
        public int Number;
        public string Code;
        public string Label;
        public int LineIndex;
        public string FunctionName;

        public override string ToString()
        {
            return $"{FunctionName}:{Number} {Code}";
        }

        public string ToLine()
        {
            return $"{Number} {Code}";
        }
    }
}
