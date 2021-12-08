using System;

namespace Abstract.Helpful.Lib.Configs
{
    public struct EnvironmentValue : IEquatable<EnvironmentValue>
    {
        public string Value { get; }
        public EnvironmentValue(string value)
        {
            Value = value;
        }
        public static EnvironmentValue From(string value) => new(value);
        public static implicit operator string(EnvironmentValue key) => key.Value;
        public static implicit operator EnvironmentValue(string value) => new(value);

        public static EnvironmentValue Current(EnvironmentKey key = default)
        {
            return Environment.GetEnvironmentVariable(
                key.IsDefault() ? EnvironmentKey.AspnetcoreEnvironment : key,
                EnvironmentVariableTarget.Process) ?? string.Empty;
        }

        public override string ToString()
        {
            if (Value == null)
                return "null";
            return Value;
        }

        #region Equals

        public bool Equals(EnvironmentValue other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is EnvironmentValue other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public static bool operator ==(EnvironmentValue left, EnvironmentValue right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EnvironmentValue left, EnvironmentValue right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}