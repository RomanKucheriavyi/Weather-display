using System;

namespace Abstract.Helpful.Lib.Configs
{
    public struct OrganizationName : IEquatable<OrganizationName>
    {
        public string Value { get; }
        public OrganizationName(string value)
        {
            Value = value;
        }
        public static OrganizationName From(string value) => new(value);
        public static implicit operator string(OrganizationName key) => key.Value;
        public static implicit operator OrganizationName(string value) => new(value);
        
        public override string ToString()
        {
            return Value;
        }

        #region Equals

        public bool Equals(OrganizationName other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is OrganizationName other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public static bool operator ==(OrganizationName left, OrganizationName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OrganizationName left, OrganizationName right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}