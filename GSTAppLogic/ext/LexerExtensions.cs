using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CTokenizer;
using GSTLibrary.tile;
using GSTLibrary.token;
using Tokenizer;
using System.Diagnostics;

namespace GSTAppLogic.ext
{
    public static class LexerExtensions
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
        public static GSTTokenList<GSTToken<TokenWrapper>> ToGSTTokenList(this IEnumerable<string> tokens, TokenFactory factory)
        {
            var enumTokens = factory.GetTokenWrapperEnumerable(tokens);
            return new GSTTokenList<GSTToken<TokenWrapper>>(enumTokens.Select(token => new GSTToken<TokenWrapper>(token)));
        }

        /// <summary>
        /// converts the enumerable into a GSTTokenList
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public static GSTTokenList<GSTToken<TE>> ToGSTTokenList<TE>(this IEnumerable<TE> tokens)
        {
            return new GSTTokenList<GSTToken<TE>>(tokens.Select(token => new GSTToken<TE>(token)));
        }

        
    }
}
