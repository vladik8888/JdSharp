using System.Linq.Expressions;
using JdSharp.Core;
using JdSharp.Runner;

string code =
@"
            int parenDepth = 0;
            int braceDepth = 0;
            int position = 0;
            for (int i = 0; i < code.Length; i++)
            {
                char c = code[i];
                if (c == '(') parenDepth++;
                else if (c == ')') parenDepth--;
                if (parenDepth == 0)
                {
                    if (c == '{') braceDepth++;
                    else if (c == '}' && --braceDepth == 0)
                    {
                        string exp = code[position..(i+1)].Trim();
                        SingleExecute(exp);
                        position = i + 1;
                    }
                    else if (c == ';' && braceDepth == 0)
                    {
                        string exp = code[position..i].Trim();
                        SingleExecute(exp);
                        position = i + 1;
                    }
                }
            };
            if (position == 0) SingleExecute(code);
";

Exp1 exp1 = new();
Exp2 exp2 = new();
Exp3 exp3 = new();
Context context = new();
for (int i = 0; i < 100000; i++)
{
    context.Expressions.AddRange(exp1, exp2, exp3);
}

Interpreter runner = new(context);
string[] sts = runner.Split(code);
foreach (string st in sts) Console.WriteLine("Выражение: " + st);

string c = "if(position == 0) SingleExecute (code) 1213()  5";
string[] t = runner.Tokenize(c);
foreach (string st in t) Console.WriteLine("Токен: " + st);
