namespace JdSharp.Core.Types
{
    public abstract class Expression
    {
        public abstract char StartingPointer { get; }
        public abstract char EndingPointer { get; }

        public abstract bool IsAtomic { get; }

        public abstract object? Execute(string expression);
        public virtual string Extract(string expression)
        {
            string temp = expression;
            while (temp[0] == StartingPointer && temp[^1] == EndingPointer)
                temp = temp[1..^1];
            return temp;
        }
    }
}