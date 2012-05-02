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
        private MutexCLexer lexer;

        [SetUp]
        public void SetUp()
        {
            lexer = new MutexCLexer();
            lexer.CharStream = new ANTLRStringStream(
                "void main(int argc, char** argv) {\r\n" + 
                    "printf(argv, argc);\r\n" +
                "}"); // \"Hello World!\"
        }

        [Test]
        public void TokenStringOnDefault()
        {
            Assert.AreEqual("DATATYPE IDENTIFIER DATATYPE IDENTIFIER DATATYPE POINTER IDENTIFIER FUNCTION_CALL", lexer.GetJoinedTokenString());
        }
    }
}
