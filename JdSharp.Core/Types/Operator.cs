namespace JdSharp.Core.Types
{
    public abstract class Operator
    {
        public abstract string Pointer { get; }

        public abstract bool IsReversed { get; }
        public abstract int Priority { get; }

        public abstract object? Execute(string? left = null, string? right = null);
    }
}