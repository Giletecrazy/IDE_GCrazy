
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDE_GCrazy
{
    internal class GrammarParse
    {
        public class GrammarParser
        {
            private myCParser _parser;
            public IParseTree Parse(string teste)
            {
                // 1. Create an ANTLR input stream from the string
                var stream = new AntlrInputStream(teste);

                // 2. Create a lexer (assuming 'myCLexer' is your generated lexer class)
                var lexer = new myCLexer(stream);

                // 3. Create a token stream from the lexer
                var tokens = new CommonTokenStream(lexer);

                // 4. Create a parser (assuming 'myCParser' is your generated parser class)
                _parser = new myCParser(tokens);

                // 5. Start parsing from your grammar's start rule (replace 'compilationUnit' with your actual start rule name)
                return _parser.programa(); // Replace 'compilationUnit' with your actual start rule
           }

            public myCParser GetParser()
            {
                return _parser;
            }
        }
    }
}
