using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleCompiler
{
    class Compiler
    {


        public Compiler()
        {
        }

        //reads a file into a list of strings, each string represents one line of code
        public List<string> ReadFile(string sFileName)
        {
            StreamReader sr = new StreamReader(sFileName);
            List<string> lCodeLines = new List<string>();
            while (!sr.EndOfStream)
            {
                lCodeLines.Add(sr.ReadLine());
            }
            sr.Close();
            return lCodeLines;
        }



        //Computes the next token in the string s, from the begining of s until a delimiter has been reached. 
        //Returns the string without the token.
        private string Next(string s, char[] aDelimiters, out string sToken, out int cChars)
        {
            cChars = 1;
            sToken = s[0] + "";
            if (aDelimiters.Contains(s[0]))
                return s.Substring(1);
            int i = 0;
            for (i = 1; i < s.Length; i++)
            {
                if (aDelimiters.Contains(s[i]))
                    return s.Substring(i);
                else
                    sToken += s[i];
                cChars++;
            }
            return null;
        }

        //Splits a string into a list of tokens, separated by delimiters
        private List<string> Split(string s, char[] aDelimiters)
        {
            List<string> lTokens = new List<string>();
            while (s.Length > 0)
            {
                string sToken = "";
                int i = 0;
                for (i = 0; i < s.Length; i++)
                {
                    if (aDelimiters.Contains(s[i]))
                    {
                        if (sToken.Length > 0)
                            lTokens.Add(sToken);
                        lTokens.Add(s[i] + "");
                        break;
                    }
                    else
                        sToken += s[i];
                }
                if (i == s.Length)
                {
                    lTokens.Add(sToken);
                    s = "";
                }
                else
                    s = s.Substring(i + 1);
            }
            return lTokens;
        }

        //This is the main method for the Tokenizing assignment. 
        //Takes a list of code lines, and returns a list of tokens.
        //For each token you must identify its type, and instantiate the correct subclass accordingly.
        //You need to identify the token position in the file (line, index within the line).
        //You also need to identify errors, in this assignement - illegal identifier names.
        public List<Token> Tokenize(List<string> lCodeLines)
        {

            List<Token> lTokens = new List<Token>();
            //your code here

            char[] delimitors = {
                '\t',' ',',',';','\''
                ,'(', ')', '{', '}',
                '[',']', '*', '+',
                '-', '/', '<', '>',
                '&', '=', '|', '!'
            };

            int ln = 0;
            int ch = 0;
            foreach(string lineRaw in lCodeLines)
            {
                string line = lineRaw;
                ch = 0;

                //line is a comment
                if (line.Contains("//"))
                {
                    ln++;
                    continue;
                } 

                Console.WriteLine("   Original line: "+line);
                    Console.Write("Interpreted line: ");

                string token = "";
                int cChars = 0;
                while (line != "" & line != null)
                {
                    line = Next(line, delimitors, out token, out cChars);
                    if (Token.Statements.Contains(token)) lTokens.Add(new Statement(token, ln, ch));
                    else if (token == " " | token == "\t") { }
                    else if (Token.VarTypes.Contains(token)) lTokens.Add(new VarType(token, ln, ch));
                    else if (cChars == 1 && Token.Separators.Contains(token[0])) lTokens.Add(new Separator(token[0], ln, ch));
                    else if (cChars == 1 && Token.Parentheses.Contains(token[0])) lTokens.Add(new Parentheses(token[0], ln, ch));
                    else if (cChars == 1 && Token.Operators.Contains(token[0])) lTokens.Add(new Operator(token[0], ln, ch));
                    else if (Token.Constants.Contains(token)) lTokens.Add(new Constant(token, ln, ch));
                    
                    //indentifiers and numbers
                    else 
                    {
                        //is an identifier
                        if ("0123456789".IndexOf(token[0]) == -1)
                        {

                            //checks for illegal characters
                            foreach (char c in token)
                            {
                                if ("0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM".IndexOf(c) == -1)
                                {
                                    throw new SyntaxErrorException("Error: Illegal identifier name: " + token, new Identifier(token, ln, ch));
                                }
                            }

                            
                            lTokens.Add(new Identifier(token, ln, ch));
                        }

                        //is potentially a number
                        else
                        {
                            //checks if it's really a number
                            foreach (char c in token)
                            {
                                if ("0123456789".IndexOf(c) == -1)
                                {
                                    throw new SyntaxErrorException("Error: Illegal identifier name: " + token, new Identifier(token, ln, ch));
                                }
                            }
                            
                            
                            lTokens.Add(new Number(token, ln, ch));
                        }      
                    }
                    ch += cChars;
                    Console.Write(token);
                }
                ln++;
                Console.WriteLine();
                Console.WriteLine("================================================================");
            }
            return lTokens;
        }

    }
}

