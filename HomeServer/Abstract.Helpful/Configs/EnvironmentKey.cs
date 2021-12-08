using System;

namespace Abstract.Helpful.Lib.Configs
{
    public struct EnvironmentKey : IEquatable<EnvironmentKey>
    {
        public static readonly EnvironmentKey AspnetcoreEnvironment = new("ASPNETCORE_ENVIRONMENT");
        
        public string Value { get; }
        public EnvironmentKey(string value)
        {
            Value = value;
        }
        public static EnvironmentKey From(string value) => new(value);
        public static implicit operator string(EnvironmentKey key) => key.Value;
        public static implicit operator EnvironmentKey(string value) => new(value);

        #region Equals

        public bool Equals(EnvironmentKey other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is EnvironmentKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public static bool operator ==(EnvironmentKey left, EnvironmentKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EnvironmentKey left, EnvironmentKey right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}