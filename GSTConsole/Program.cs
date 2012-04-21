using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using CTokenizer;
using GSTLibrary.tile;
using GSTLibrary.token;

namespace GSTConsole
{
    static class Program
    {
        private const string RelativePathToTestFiles = @"..\..\..\test";
        private static GSTAlgorithm<GSTToken<TokenWrapper>> Algorithm;
 
        static void Main(string[] args)
        {
            /*
            var listA = GetTokenList(new MutexCLexer(new ANTLRStringStream("void main(void) { }")));
            var listB = GetTokenList(new MutexCLexer(new ANTLRStringStream("void main(void) { }")));
            /* */

            
            var listA = GetTokenList(new MutexCLexer(new ANTLRFileStream(Path.Combine(RelativePathToTestFiles, "main-01.c"))));
            var listB = GetTokenList(new MutexCLexer(new ANTLRFileStream(Path.Combine(RelativePathToTestFiles, "main-13.c"))));
            /* */
            Algorithm = new GSTAlgorithm<GSTToken<TokenWrapper>>(listA, listB)
                {
                    MinimumMatchLength = 5
                };
            

            Algorithm.RunToCompletion();

            Console.WriteLine("Similarity: {0}", Algorithm.Similarity);
            /*
            var fileStream = new ANTLRFileStream(@"test\main-01.c");
            
            Lexer lexer = new MutexCLexer(fileStream);
            
            var lexerType = lexer.GetType();
            IToken myToken;

            while(-1 != (myToken = lexer.NextToken()).Type)
            {
                Console.WriteLine("Lexer-Token: {0} => {1}", 
                    myToken.Type.GetTokenName(lexerType), 
                    myToken.Text);
            }
            /* */
            Console.WriteLine("Finished Lexer run");
            Console.ReadLine();
        }

        internal static GSTTokenList<GSTToken<TokenWrapper>> GetTokenList(Lexer lexer)
        {
            var list = new GSTTokenList<GSTToken<TokenWrapper>>();
            IToken myToken;

            while(-1 != (myToken = lexer.NextToken()).Type)
            {
                list.Add(new GSTToken<TokenWrapper>(new TokenWrapper(myToken)));    
            }

            return list;
        }
    }
}
