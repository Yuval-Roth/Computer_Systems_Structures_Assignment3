﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class JackProgram : JackProgramElement
    {
        public List<VarDeclaration> Globals { get; private set; }
        public List<Function> Functions { get; private set; }
        public Function Main { get; private set; }


        public override void Parse(TokensStack sTokens)
        {
            try
            {
                Globals = new List<VarDeclaration>();
                while ((sTokens.Peek() is Statement) && ((Statement)sTokens.Peek()).Name == "global")
                {
                    VarDeclaration global = new VarDeclaration(true);
                    global.Parse(sTokens);
                    Globals.Add(global);
                }
                Functions = new List<Function>();
                while (sTokens.Count > 0)
                {
                    if (!(sTokens.Peek() is Statement) || ((Statement)sTokens.Peek()).Name != "function")
                        throw new SyntaxErrorException("Expected function", sTokens.Peek());
                    Function f = new Function();
                    f.Parse(sTokens);
                    Functions.Add(f);
                }
                Main = Functions.Last();
                Functions.Remove(Main);
            }
            catch (InvalidOperationException)
            {
                if (sTokens.LastPop == null)
                    throw new SyntaxErrorException("Unexpected end of file", new Token());
                else throw new SyntaxErrorException("Unexpected end of file", sTokens.LastPop);
            }
            catch (SyntaxErrorException e)
            {
                Token t = e.Token;
                if (t == null)
                    throw new SyntaxErrorException(e.Message, new Token());
                else throw e;
            }
        }

        public override string ToString()
        {
            string sProgram = "";
            foreach (VarDeclaration v in Globals)
                sProgram += "\t" + v + "\n";
            foreach (Function f in Functions)
                sProgram += "\t" + f + "\n";
            sProgram += "\t" + Main + "\n";
            return sProgram;
        }
    }
}
