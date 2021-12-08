using System;

namespace Abstract.Helpful.Lib.Configs
{
    public struct ApiPort : IEquatable<ApiPort>
    {
        public uint Value { get; }
        
        public ApiPort(uint value)
        {
            Value = value;
        }
        
        public ApiPort(int value)
        {
            Value = (uint) value;
        }

        public override string ToString()
        {
            return Value == 0 ? "NOT_SPECIFIED" : Value.ToString();
        }

        public static ApiPort From(uint value) => new(value);
        public static implicit operator uint(ApiPort key) => key.Value;
        public static implicit operator ApiPort(uint value) => new(value);
        public static implicit operator int(ApiPort key) => (int) key.Value;
        public static implicit operator ApiPort(int value) => new(value);
        
        #region Equals

        public bool Equals(ApiPort other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is ApiPort other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (int) Value;
        }

        public static bool operator ==(ApiPort left, ApiPort right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ApiPort left, ApiPort right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}