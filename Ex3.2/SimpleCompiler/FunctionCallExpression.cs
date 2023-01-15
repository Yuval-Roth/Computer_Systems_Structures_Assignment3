using System;
using System.Collections.Generic;

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
            if (t is Identifier == false)
                throw new SyntaxErrorException("Expected identifier, received " + t, t);

            FunctionName = t.ToString();

            t = sTokens.Pop(); // (
            if (t is Parentheses == false || t.ToString() != "(")
                throw new SyntaxErrorException("Expected (, received " + t, t);

            Args = new List<Expression>();
            while (sTokens.Peek() is Identifier || (sTokens.Peek() is Separator && sTokens.Peek().ToString() == ","))
            {
                if (sTokens.Peek() is Identifier)
                {
                    Expression e = Create(sTokens);
                    e.Parse(sTokens);
                    Args.Add(e);
                }
                else
                {
                    sTokens.Pop();
                }
            }
        
            t = sTokens.Pop(); // )
            if (t is Parentheses == false || t.ToString() != ")")
                throw new SyntaxErrorException("Expected ), received " + t, t);

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