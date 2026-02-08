using JdSharp.Core.Types;

namespace JdSharp.Core
{
    public class Context
    {
        public List<Operator> Operators { get; } = [];
        public List<Operand> Operands { get; } = [];
        public List<Expression> Expressions { get; } = [];
        public List<Identifier> Identifiers { get; } = [];
    }
}