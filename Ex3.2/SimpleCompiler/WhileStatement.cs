using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class WhileStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> Body { get; private set; }

        public override void Parse(TokensStack sTokens)
        {
            Token t;

            sTokens.Pop(); // pop the while

            t = sTokens.Pop();
            if (t is Parentheses p1 == false || p1.Name != '(')
                throw new SyntaxErrorException("Expected (, received " + t, t);

            Term = Expression.Create(sTokens);
            Term.Parse(sTokens);

            t = sTokens.Pop();
            if (t is Parentheses p2 == false || p2.Name != ')')
                throw new SyntaxErrorException("Expected ), received " + t, t);

            t = sTokens.Pop();
            if (t is Parentheses p3 == false || p3.Name != '{')
                throw new SyntaxErrorException("Expected {, received " + t, t);

            Body = new List<StatetmentBase>();
            while (sTokens.Peek() is Statement s1)
            {
                StatetmentBase s = Create(s1);
                s.Parse(sTokens);
                Body.Add(s);
            }

            t = sTokens.Pop();
            if (t is Parentheses p4 == false || p4.Name != '}')
                throw new SyntaxErrorException("Expected }, received " + t, t);
            
        }

        public override string ToString()
        {
            string sWhile = "while(" + Term + "){\n";
            foreach (StatetmentBase s in Body)
                sWhile += "\t\t\t" + s + "\n";
            sWhile += "\t\t}";
            return sWhile;
        }

    }
}
