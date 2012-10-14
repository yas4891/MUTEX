using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using NUnit.Framework;

namespace CTokenizer.test
{
    [TestFixture]
    public class LexerHelperTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void TokenStringOnDefault()
        {
            /*
            var lexer = ToLexer(
                "void main(int argc, char** argv) {\r\n" +
                "printf(\"Hello World!\");\r\n" +
                "}");

            Assert.AreEqual(
                "T__92 IDENTIFIER T__30 T__79 IDENTIFIER T__37 T__66 T__32 T__32 IDENTIFIER T__31 T__95 IDENTIFIER T__30 STRING_LITERAL T__31 T__47 T__99", 
                lexer);
            /* */
        }


        private static Lexer ToLexer(string str)
        {
            return new CLexer(new ANTLRStringStream(str));
        }
    }
}
