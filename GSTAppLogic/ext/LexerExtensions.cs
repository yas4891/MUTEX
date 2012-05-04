using System.Collections.Generic;
using System.Linq;
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

        
    }
}
