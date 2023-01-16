using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SimpleCompiler
{
    public class FunctionCallExpression : Expression
    {
        public string FunctionName { get; private set; }
        public List<Expression> Args { get; private set; }

        public override void Parse(TokensStack sTokens)
        {
            Token t;
            t = sTokens.Pop(); // funcId
            if (t is Identifier i1 == false)
                throw new SyntaxErrorException("Expected identifier, received " + t, t);

            FunctionName = i1.Name;
            
            t = sTokens.Pop(); // (
            if (t is Parentheses p1 == false || p1.Name != '(')
                throw new SyntaxErrorException("Expected (, received " + t, t);

            Args = new List<Expression>();

            Expression e = Create(sTokens);
            e.Parse(sTokens);
            Args.Add(e);

            while (sTokens.Peek() is Separator s1 && s1.Name == ',') 
            {
                sTokens.Pop(); // pop the ,
                e = Create(sTokens);
                e.Parse(sTokens);
                Args.Add(e);
            }

            t = sTokens.Pop(); // )
            if (t is Parentheses p2 == false || p2.Name != ')')
                throw new SyntaxErrorException("Expected ), received " + t, t);


            // not sure this is needed here

            //t = sTokens.Pop(); // ;
            //if (t is Separator s2 == false || s2.Name != ';')
            //    throw new SyntaxErrorException("Expected ;, received " + t, t);

        }

        public override string ToString()
        {
            string sFunction = FunctionName + "(";
            for (int i = 0; i < Args.Count - 1; i++)
                sFunction += Args[i] + ",";
            if (Args.Count > 0)
                sFunction += Args[Args.Count - 1];
            sFunction += ")";
            return sFunction;
        }
    }
}