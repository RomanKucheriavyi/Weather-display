using System;

namespace HomeServerApi.Logic
{
    public readonly struct WeatherFormatVersion : IEquatable<WeatherFormatVersion>, IComparable<WeatherFormatVersion>, IComparable
    {
        public byte Version { get; }

        public WeatherFormatVersion(byte version)
        {
            Version = version;
        }

        public override string ToString()
        {
            return $"{nameof(Version)}: {Version}";
        }

        public static WeatherFormatVersion From(byte version) => new(version);
        
        public bool Equals(WeatherFormatVersion other)
        {
            return Version == other.Version;
        }

        public override bool Equals(object obj)
        {
            return obj is WeatherFormatVersion other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Version.GetHashCode();
        }

        public static bool operator ==(WeatherFormatVersion left, WeatherFormatVersion right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(WeatherFormatVersion left, WeatherFormatVersion right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(WeatherFormatVersion other)
        {
            return Version.CompareTo(other.Version);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is WeatherFormatVersion other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(WeatherFormatVersion)}");
        }

        public static bool operator <(WeatherFormatVersion left, WeatherFormatVersion right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(WeatherFormatVersion left, WeatherFormatVersion right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(WeatherFormatVersion left, WeatherFormatVersion right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(WeatherFormatVersion left, WeatherFormatVersion right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}