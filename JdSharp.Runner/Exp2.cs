using JdSharp.Core.Types;

namespace JdSharp.Runner
{
    public class Exp2 : Expression
    {
        public override char StartingPointer => '{';
        public override char EndingPointer => '}';

        public override bool IsAtomic => false;

        public override object? Execute(string body)
        {
            return null;
        }
    }
}