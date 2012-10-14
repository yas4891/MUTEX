using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr.Runtime;
using Tokenizer;

namespace CTokenizer
{
    /// <summary>
    /// factory for the MutexCLexer
    /// </summary>
    public class MutexTokenFactory : TokenFactory
    {
        private static readonly Type mutexLexerType = typeof (MutexCLexer);

        protected override Type LexerType
        {
            get { return mutexLexerType; }
        }
        
        public override IToken GetToken(string tokenName)
        {
            return new MutexTokenImpl(tokenName);
        }

        public override List<TokenWrapper> GetTokenWrapperListFromSource(string source)
        {
            var lexer = new MutexCLexer(new ANTLRStringStream(source));

            var gst = lexer.GetTokens().Select(token => new TokenWrapper(token, GetTokenName(token.Type, mutexLexerType)));
            return gst.ToList();
        }
    }
}
