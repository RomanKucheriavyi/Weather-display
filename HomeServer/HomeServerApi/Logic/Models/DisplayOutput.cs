using System;
using System.Diagnostics.Contracts;
using Abstract.Helpful.Lib;

namespace HomeServerApi.Logic
{
    public sealed record DisplayOutput
    {
        public const int ROW_LENGTH = 16;

        public string Line1 { get; }
        public string Line2 { get; }
        
        public DisplayOutput(string line1, string line2)
        {
            Line1 = NormalizeString(line1);
            Line2 = NormalizeString(line2);
        }

        [Pure]
        [Safe]
        private string NormalizeString(string line)
        {
            if (line.IsNullOrEmpty())
                return new string(' ', ROW_LENGTH);
            
            if (line.Length < ROW_LENGTH)
                return line + new string(' ', ROW_LENGTH - line.Length);
            
            if (line.Length == ROW_LENGTH)
                return line;
            
            return line.Substring(0, ROW_LENGTH);
        }

        [Pure]
        [Safe]
        public string ToSingleString()
        {
            return $"{Line1}{Line2}";
        }
        
        public override string ToString()
        {
            return $"{Line1}{Environment.NewLine}{Line2}";
        }
    }
}