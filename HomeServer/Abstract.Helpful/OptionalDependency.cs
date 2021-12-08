namespace Abstract.Helpful.Lib
{
    public sealed class OptionalDependency<T>
    {
        public readonly T Value;
        public readonly bool HasValue;
        
        public OptionalDependency(T value)
        {
            HasValue = true;
            Value = value;
        }

        public OptionalDependency()
        {
            HasValue = false;
            Value = default;
        }

        public static implicit operator T(OptionalDependency<T> d) => d.Value;
    }
}