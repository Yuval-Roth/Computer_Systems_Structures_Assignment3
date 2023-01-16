using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class IfStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> DoIfTrue { get; private set; }
        public List<StatetmentBase> DoIfFalse { get; private set; }

        public override void Parse(TokensStack sTokens)
        {
            Token t;

            t = sTokens.Pop();
            if (t is Keyword k1 == false || k1.Name != "if")
                throw new SyntaxErrorException("Expected if, received " + t, t);

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

            DoIfTrue = new List<StatetmentBase>();
            
            while (sTokens.Peek() is Statement s1)
            {
                StatetmentBase s = Create(s1);
                s.Parse(sTokens);
                DoIfTrue.Add(s);
            }

            t = sTokens.Pop();
            if (t is Parentheses p4 == false || p4.Name != '}')
                throw new SyntaxErrorException("Expected }, received " + t, t);

            DoIfFalse = new List<StatetmentBase>();

            t = sTokens.Peek();
     
            if (t is Keyword == false && ((Keyword)t).Name == "else")
            {
                sTokens.Pop(); // pop the else
                
                t = sTokens.Pop();

                if (t is Parentheses p5 == false || p5.Name != '{')
                    throw new SyntaxErrorException("Expected {, received " + t, t);

                while (sTokens.Peek() is Statement)
                {
                    t = sTokens.Pop();
                    StatetmentBase s = Create(t);
                    s.Parse(sTokens);
                    DoIfFalse.Add(s);
                }

                t = sTokens.Pop();

                if (t is Parentheses p6 == false || p6.Name != '}')
                    throw new SyntaxErrorException("Expected }, received " + t, t);
            }
                
        }
        public override string ToString()
        {
            string sIf = "if(" + Term + "){\n";
            foreach (StatetmentBase s in DoIfTrue)
                sIf += "\t\t\t" + s + "\n";
            sIf += "\t\t}";
            if (DoIfFalse.Count > 0)
            {
                sIf += "else{";
                foreach (StatetmentBase s in DoIfFalse)
                    sIf += "\t\t\t" + s + "\n";
                sIf += "\t\t}";
            }
            return sIf;
        }

    }
}
