namespace JdSharp.Core.Types
{
    public abstract class Operand
    {
        public abstract string Pointer { get; }

        public abstract object? Execute(string[] left, string[] right);
    }
}