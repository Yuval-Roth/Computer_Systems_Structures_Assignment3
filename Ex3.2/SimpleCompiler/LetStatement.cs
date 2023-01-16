using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class LetStatement : StatetmentBase
    {
        public string Variable { get; set; }
        public Expression Value { get; set; }

        public override string ToString()
        {
            return "let " + Variable + " = " + Value + ";";
        }

        public override void Parse(TokensStack sTokens)
        {
            Token t;

            sTokens.Pop(); // pop the let

            t = sTokens.Pop(); 
            if (t is Identifier i1 == false)
                throw new SyntaxErrorException("Expected identifier, received " + t, t);

            Variable = i1.Name;

            t = sTokens.Pop(); // =
            if (t is Operator o1 == false || o1.Name != '=')
                throw new SyntaxErrorException("Expected =, received " + t, t);

            Value = Expression.Create(sTokens);
            Value.Parse(sTokens);

            t = sTokens.Pop(); // ;
            if (t is Separator s1 == false || s1.Name != ';')
                throw new SyntaxErrorException("Expected ;, received " + t, t);

        }
    }
}
