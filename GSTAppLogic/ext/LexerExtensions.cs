using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTokenizer;
using GSTLibrary.token;

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
