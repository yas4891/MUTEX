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
            var lexer = ToLexer(
                "void main(int argc, char** argv) {\r\n" +
                "printf(\"Hello World!\");\r\n" +
                "}");

            Assert.AreEqual("DATATYPE IDENTIFIER DATATYPE IDENTIFIER DATATYPE POINTER IDENTIFIER FUNCTION_CALL", lexer.GetJoinedTokenString());
        }

        [Test]
        public void IgnoresComments()
        {
            var lexer = ToLexer(
                "void main(int argc, char** argv) {\r\n" +
                "/*single line comment*/ \r\n" + 
                "printf(\"Hello World!\");\r\n" +
                "}");

            Assert.AreEqual("DATATYPE IDENTIFIER DATATYPE IDENTIFIER DATATYPE POINTER IDENTIFIER FUNCTION_CALL", lexer.GetJoinedTokenString());
        }

        private static Lexer ToLexer(string str)
        {
            return new CLexer(new ANTLRStringStream(str));
        }
    }
}
