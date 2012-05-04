using System.Collections.Generic;
using System.Linq;
using CTokenizer;
using GSTLibrary.token;
using Tokenizer;

namespace GSTAppLogic.ext
{
    static class LexerExtensions
    {
        public static GSTTokenList<GSTToken<TokenWrapper>> ToGSTTokenList(this IEnumerable<TokenWrapper> tokens)
        {
            return new GSTTokenList<GSTToken<TokenWrapper>>(tokens.Select(token => new GSTToken<TokenWrapper>(token)));
        }

        /// <summary>
        /// converts the string representations of the tokens into appropriately wrapped objects
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static GSTTokenList<GSTToken<TokenWrapper>> ToGSTTokenList(this IEnumerable<string> tokens)
        {
            var enumTokens = tokens.GetTokens();
            return new GSTTokenList<GSTToken<TokenWrapper>>(enumTokens.Select(token => new GSTToken<TokenWrapper>(token)));
        }
    }
}
