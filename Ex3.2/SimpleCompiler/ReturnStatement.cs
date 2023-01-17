using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class ReturnStatement : StatetmentBase
    {
        public Expression Expression { get; private set; }

        //This is an example of the implementation of the Parse method
        //You need to add here correctness checks. 
        public override void Parse(TokensStack sTokens)
        {
            Token t;

            sTokens.Pop(); // pop the return

            Expression = Expression.Create(sTokens);
            Expression.Parse(sTokens);
            
            t = sTokens.Pop();//;
            if (t is Separator s1 == false || s1.Name != ';')
                throw new SyntaxErrorException("expected ;, received " + t, t);
        }

        public override string ToString()
        {
            return "return " + Expression + ";";
        }
    }
}
