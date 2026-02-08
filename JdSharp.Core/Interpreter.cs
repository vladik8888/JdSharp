using JdSharp.Core.Types;

namespace JdSharp.Core
{
    public class Interpreter(Context context)
    {
        public Context Context { get; } = context;

        public string[] Split(string code, char splitter = ';')
        {
            List<string> statements = [];
            void AddStatement(int start, int end)
            {
                string statement = code[start..end].Trim();
                if (statement.Length > 0)
                    statements.Add(statement);
            }

            int position = 0;
            for (int i = 0; i < code.Length; i++)
            {
                char c = code[i];
                if (c == splitter)
                {
                    AddStatement(position, i);
                    position = i + 1;
                }
                i = ResolveExpression(code, i);
            }

            if (position < code.Length)
                AddStatement(position, code.Length);
            return [.. statements];
        }

        public int ResolveExpression(string code, int position)
        {
            List<Expression> expressions = Context.Expressions;
            Expression? startingTarget = expressions.FirstOrDefault(x => x.StartingPointer == code[position]);
            Expression? endingTarget = expressions.FirstOrDefault(x => x.EndingPointer == code[position]);

            if (startingTarget != null)
                for (int i = position + 1; i < code.Length; i++)
                {
                    char c = code[i];
                    if (c == '\\')
                        continue;
                    if (c == startingTarget.EndingPointer)
                        return i;
                    if (!startingTarget.IsAtomic)
                        i = ResolveExpression(code, i);
                }
            else if (endingTarget != null)
                for (int i = code.Length - 1; i >= position; i--)
                {
                    char c = code[i];
                    if (c == '\\')
                        continue;
                    if (c == endingTarget.StartingPointer)
                        return i;
                    if (!endingTarget.IsAtomic)
                        i = ResolveExpression(code, i);
                }

            return position;
        }

        public string[] Tokenize(string code)
        {
            List<string> tokens = [];
            void AddToken(int start, int end)
            {
                string token = code[start..end].Trim();
                if (token.Length > 0)
                    tokens.Add(token);
            }

            int position = 0;
            for (int i = 0; i < code.Length; i++)
            {
                int t = ResolveExpression(code, i);
                if (i != t)
                {
                    AddToken(position, i);
                    AddToken(i, t + 1);
                    position = t + 1;
                    i = t;
                }
            }

            if (position < code.Length)
                AddToken(position, code.Length);
            return [.. tokens];
        }

        public object? Execute(string code)
        {
            if (code.Length >= 2)
                foreach (Expression expression in Context.Expressions)
                    if (code[0] == expression.StartingPointer && code[^1] == expression.EndingPointer)
                        return expression.Execute(code);
            foreach (Identifier identifier in Context.Identifiers)
                if (code == identifier.Pointer)
                    return identifier.Value;

            for (int i = 0; i < code.Length; i++)
            {
                Operator? current = Context.Operators.FirstOrDefault(x =>
                    code.Length - i >= x.Pointer.Length &&
                    code.Substring(i, x.Pointer.Length) == x.Pointer);
                if (current != null)
                {
                    string left = code[..i].Trim();
                    string right = code[(i + current.Pointer.Length)..].Trim();
                    return current.Execute(left.Length > 0 ? left : null, right.Length > 0 ? right : null);
                }
                i = ResolveExpression(code, i);
            }

            string[] tokens = Tokenize(code);
            for (int i = 0; i < tokens.Length; i++)
            {
                string t = tokens[i];
                Operand? operand = Context.Operands.FirstOrDefault(x => x.Pointer == t);
                if (operand != null)
                {
                    string[] left = tokens[..i];
                    string[] right = tokens[(i + 1)..];
                    return operand.Execute(left, right);
                }
            }

            return null;
        }
    }
}