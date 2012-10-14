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
        /// Compares the two files and returns the Similarity. 
        /// Lexer used: MutexCLexer
        /// Algorithm used: HashingGSTAlgorithm (MML = 8)
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static Int32 CompareFiles(string path1, string path2)
        {
            var factory = new MutexTokenFactory();
            
            var tokens1 = factory.GetTokenWrapperListFromFile(path1);
            var tokens2 = factory.GetTokenWrapperListFromFile(path2);

            var algo = new HashingGSTAlgorithm<GSTToken<TokenWrapper>>(
                tokens1.ToGSTTokenList<TokenWrapper>(),
                tokens2.ToGSTTokenList<TokenWrapper>()) {MinimumMatchLength = 8};
            algo.RunToCompletion();


            return algo.Similarity;
        }



    }
}
