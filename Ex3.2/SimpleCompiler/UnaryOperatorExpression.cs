using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    class UnaryOperatorExpression : Expression
    {
        public string Operator { get; set; }
        public Expression Operand { get; set; }

        public override string ToString()
        {
            return Operator + Operand;
        }

        public override void Parse(TokensStack sTokens)
        {
            Token t;

            t = sTokens.Pop(); // Operator
            if (t is Operator o1 == false)
                throw new SyntaxErrorException("Expected operator, received " + t, t);

            Operator = ""+o1.Name;

            Operand = Create(sTokens);
            Operand.Parse(sTokens);
        }
    }
}
