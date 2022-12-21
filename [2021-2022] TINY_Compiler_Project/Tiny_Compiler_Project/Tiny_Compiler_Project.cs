using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiny_Compiler_Project
{
    public static class Tiny_Compiler_Project
    {
        public static Scanner Tiny_Scanner = new Scanner();
        public static Parser Tiny_Parser = new Parser();
        public static List<Token> TokenStream = new List<Token>();
        public static Node treeroot;


        public static void Start_Compiling(string SourceCode) //character by character
        {
            //Scanner
            Tiny_Scanner.StartScanning(SourceCode);
            //Parser
            //Sematic Analysis
            Tiny_Parser.StartParsing(TokenStream);
            treeroot = Tiny_Parser.root;
        }


        
    }
}
