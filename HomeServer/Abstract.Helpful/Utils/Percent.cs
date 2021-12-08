using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Abstract.Helpful.Lib.Utils
{
    [Serializable]
    [DataContract]
    public readonly struct Percent : IEquatable<Percent>, IComparable<Percent>, IComparable
    {
        public static readonly Percent Zero = new(0);
        public static readonly Percent OneHundred = new(1);

        /// <summary>
        /// Value is in 0.01 format. Where 0.01 is 1%.
        /// </summary>
        [DataMember(Name = "Value")]
        public float Value { get; }

        public Percent(float value)
        {
            Value = value;
        }

        [Pure]
        public float ToOneHundredFormatFloat()
        {
            return Value * 100;
        }
        
        [Pure]
        public int ToOneHundredFormatInt()
        {
            return ToOneHundredFormatFloat().RoundToInt();
        }

        [Pure]
        public Percent Abs()
        {
            return new(Math.Abs(Value));
        } 
        
        [Pure]
        public Percent Multiply(Percent percent)
        {
            return new(Value * percent.Value);
        }
        
        [Pure]
        public Percent Multiply(float value)
        {
            return new(Value * value);
        }
        
        [Pure]
        public Percent Divide(Percent percent)
        {
            return new(Value / percent.Value);
        }

        [Pure]
        public float Apply(float value)
        {
            return value * Value;
        }

        public override string ToString()
        {
            return $"{ToOneHundredFormatInt()}%";
        }

        public bool Equals(Percent other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is Percent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static Percent operator -(Percent left, Percent right)
        {
            return new(left.Value - right.Value);
        }
        
        public static Percent operator +(Percent left, Percent right)
        {
            return new(left.Value + right.Value);
        }
        
        public static Percent operator /(Percent left, Percent right)
        {
            return new(left.Value / right.Value);
        }
        
        public static Percent operator *(Percent left, Percent right)
        {
            return new(left.Value * right.Value);
        }
        
        public static bool operator <(Percent left, Percent right)
        {
            return left.Value < right.Value;
        }

        public static bool operator >(Percent left, Percent right)
        {
            return left.Value > right.Value;
        }
        
        public static bool operator <=(Percent left, Percent right)
        {
            return left.Value <= right.Value;
        }

        public static bool operator >=(Percent left, Percent right)
        {
            return left.Value >= right.Value;
        }

        public static bool operator ==(Percent left, Percent right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Percent left, Percent right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(Percent other)
        {
            return Value.CompareTo(other.Value);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is Percent other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Percent)}");
        }

        [Pure]
        public static Percent FromOneHundredFormat(int percentValue)
        {
            return new(percentValue * 0.01f);
        }
        
        [Pure]
        public static Percent FromOneHundredFormat(float percentValue)
        {
            return new(percentValue * 0.01f);
        }
        
        [Pure]
        public static Percent From_ValueAndMax_In_100PercentRange(float value, float max)
        {
            if (value <= 0)
                return new Percent(0);

            if (value >= max)
                return new Percent(1);

            return new Percent(value / max);
        }

        [Pure]
        public Percent SubstractFrom100()
        {
            return new(1f - Value);
        }
        
        [Pure]
        public Percent PlusOne()
        {
            return new(Value + 0.01f);
        }
        
        [Pure]
        public Percent PlusOneHundred()
        {
            return new(Value + 1f);
        }
    }
}