using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class BinaryOperationExpression : Expression
    {
        public string Operator { get;  set; }
        public Expression Operand1 { get;  set; }
        public Expression Operand2 { get;  set; }

        public override string ToString()
        {
            return "(" + Operand1 + " " + Operator + " " + Operand2 + ")";
        }

        public override void Parse(TokensStack sTokens)
        {
            Token t;
            
            t = sTokens.Pop(); // (
            if (t is Parentheses == false || ((Parentheses)t).Name != '(')
                throw new SyntaxErrorException("Expected (, received " + t, t);
            //operand 1
            Operand1 = Create(sTokens);
            Operand1.Parse(sTokens);

            //operator

            t = sTokens.Pop(); // Operator
            if (t is Operator == false)
                throw new SyntaxErrorException("Expected operator, received " + t, t);
            Operator = t.ToString();
            
            //operand 2
            Operand2 = Create(sTokens);
            Operand2.Parse(sTokens);
            
            t = sTokens.Pop(); // )
            if (t is Parentheses == false || ((Parentheses)t).Name != ')')
                throw new SyntaxErrorException("Expected ), received " + t, t);
        }
    }
}
