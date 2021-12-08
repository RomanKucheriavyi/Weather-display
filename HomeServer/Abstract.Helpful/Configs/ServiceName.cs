using System;

namespace Abstract.Helpful.Lib.Configs
{
    public struct ServiceName : IEquatable<ServiceName>
    {
        public string Value { get; }
        public ServiceName(string value)
        {
            Value = value;
        }
        public static ServiceName From(string value) => new(value);
        public static implicit operator string(ServiceName key) => key.Value;
        public static implicit operator ServiceName(string value) => new(value);

        public override string ToString()
        {
            return Value;
        }

        #region Equals

        public bool Equals(ServiceName other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is ServiceName other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public static bool operator ==(ServiceName left, ServiceName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ServiceName left, ServiceName right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}