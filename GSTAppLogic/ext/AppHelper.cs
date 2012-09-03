using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTokenizer;
using GSTLibrary.tile;
using GSTLibrary.token;
using Tokenizer;

namespace GSTAppLogic.ext
{
    public static class AppHelper
    {

        /// <summary>
        /// Compares the two files and returns the Similarity
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static Int32 CompareFiles(string path1, string path2)
        {
            var tokens1 = LexerHelper.CreateLexer(path1).GetTokenWrappers();
            var tokens2 = LexerHelper.CreateLexer(path2).GetTokenWrappers();

            var algo = new GSTAlgorithm<GSTToken<TokenWrapper>>(
                tokens1.ToGSTTokenList<TokenWrapper>(),
                tokens2.ToGSTTokenList<TokenWrapper>());
            algo.RunToCompletion();


            return algo.Similarity;
        }
    }
}
