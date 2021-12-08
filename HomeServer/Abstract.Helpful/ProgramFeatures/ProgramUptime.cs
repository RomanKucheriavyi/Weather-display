using System;

namespace Abstract.Helpful.Lib.ProgramFeatures
{
    public static class ProgramUptime
    {
        public static readonly DateTime StartedAt = DateTime.UtcNow;

        public static string FullUptime => (DateTime.UtcNow - StartedAt).ToString("d\\.hh\\:mm\\:ss");
    }
}