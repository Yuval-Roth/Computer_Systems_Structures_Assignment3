using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class Function : JackProgramElement
    {
        //The various elements of the grammar are maintained in the fields of the object
        public VarDeclaration.VarTypeEnum ReturnType { get; private set; }
        public string Name { get; private set; }
        public List<VarDeclaration> Args { get; private set; }
        public List<VarDeclaration> Locals { get; private set; }
        public List<StatetmentBase> Body { get; private set; }
        public ReturnStatement Return { get; private set; }

        public Function()
        {
            Args = new List<VarDeclaration>();
            Locals = new List<VarDeclaration>();
            Body = new List<StatetmentBase>();
        }

        public override void Parse(TokensStack sTokens)
        {
            Token t;
            
            t = sTokens.Pop(); //function token
            if (t is Statement s1 == false || s1.Name != "function")
                throw new SyntaxErrorException("Expected function", t);
            
            t = sTokens.Pop(); // return type
            if (t is VarType == false)
                throw new SyntaxErrorException("Expected var type, received," + t, t);
            
            ReturnType = VarDeclaration.GetVarType(t);
                     
            t = sTokens.Pop(); //function name
            if (t is Identifier i1 == false)
                throw new SyntaxErrorException("Expected identifier, received," + t, t);

            Name = i1.Name;
            
            t = sTokens.Pop(); //(
            if (t is Parentheses p1 == false || p1.Name != '(')
                throw new SyntaxErrorException("Expected (, received," + t, t);

            //handle the first var in the declaration

            Token tArgType = sTokens.Pop(); // var type
            if (tArgType is VarType == false)
                throw new SyntaxErrorException("Expected var type, received," + tArgType, tArgType);

            Token tArgName = sTokens.Pop(); // var name
            if (tArgName is Identifier == false)
                throw new SyntaxErrorException("Expected identifier, received," + tArgName, tArgName);

            //handle the rest of the vars in the declaration, if there are any.

            while (sTokens.Peek() is Separator sep && sep.Name == ',')
            {
                sTokens.Pop(); // pop the comma

                tArgType = sTokens.Pop(); // var type
                if (tArgType is VarType == false)
                    throw new SyntaxErrorException("Expected var type, received," + tArgType, tArgType);

                tArgName = sTokens.Pop(); // var name
                if (tArgName is Identifier == false)
                    throw new SyntaxErrorException("Expected identifier, received," + tArgName, tArgName);

                VarDeclaration vc = new VarDeclaration(tArgType, tArgName, false);
                Args.Add(vc);
            }

            t = sTokens.Pop(); // )
            if (t is Parentheses p2 == false || p2.Name != ')')
                throw new SyntaxErrorException("Expected ), received," + t, t);

            t = sTokens.Pop(); // {
            if(t is Parentheses p3 == false || p3.Name != '{')
                throw new SyntaxErrorException("Expected {, received," + t, t);

            // list of local variable declarations
            while (sTokens.Peek() is Statement s2 && s2.Name == "var")
            {
                VarDeclaration local = new VarDeclaration(false);
                local.Parse(sTokens);
                Locals.Add(local);
            }
            
            // list of statements
            while(sTokens.Peek() is Statement s3)
            {
                StatetmentBase s = StatetmentBase.Create(s3); 
                s.Parse(sTokens);
                Body.Add(s);
            }
            
            t = sTokens.Pop(); // }
            if(t is Parentheses p4 == false || p4.Name != '}')
                throw new SyntaxErrorException("Expected } received " + t, t);
        }

        public override string ToString()
        {
            string sFunction = "function " + ReturnType + " " + Name + "(";
            for (int i = 0; i < Args.Count - 1; i++)
                sFunction += Args[i].Type + " " + Args[i].Name + ",";
            if(Args.Count > 0)
                sFunction += Args[Args.Count - 1].Type + " " + Args[Args.Count - 1].Name;
            sFunction += "){\n";
            foreach (VarDeclaration v in Locals)
                sFunction += "\t\t" + v + "\n";
            foreach (StatetmentBase s in Body)
                sFunction += "\t\t" + s + "\n";
            sFunction += "\t}";
            return sFunction;
        }
    }
}
