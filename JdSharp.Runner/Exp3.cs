using JdSharp.Core.Types;

namespace JdSharp.Runner
{
    public class Exp3 : Expression
    {
        public override char StartingPointer => '\'';
        public override char EndingPointer => '\'';

        public override bool IsAtomic => true;

        public override object? Execute(string body)
        {
            return null;
        }
    }
}