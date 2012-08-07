using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CTokenizer;
using GSTAppLogic.ext;
using GSTEvaluation.storage;
using GSTLibrary.tile;
using GSTLibrary.token;
using Tokenizer;

namespace GSTEvaluation.model
{
    /// <summary>
    /// model representing a comparison between two source files
    /// </summary>
    class ComparisonModel
    {
        public Int64 EvaluationRunID { get; private set; }

        public string Name { get; private set; }

        
        public SourceModel Source1 { get; private set; }
        public SourceModel Source2 { get; private set; }

        /// <summary>
        /// the result of the comparison
        /// </summary>
        public Int32 Result { get; private set; }

        /// <summary>
        /// takes the two sources and performs the GST 
        /// </summary>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        public ComparisonModel(string name, Int64 evalRunID, string sourcePath1, string sourcePath2)
        {
            Name = name;
            EvaluationRunID = evalRunID;

            var tokens1 = LexerHelper.CreateLexer(sourcePath1).GetTokenWrappers();
            var tokens2 = LexerHelper.CreateLexer(sourcePath2).GetTokenWrappers();
            var algo = new GSTAlgorithm<GSTToken<TokenWrapper>>(
                tokens1.ToGSTTokenList<TokenWrapper>(),
                tokens2.ToGSTTokenList<TokenWrapper>());

            algo.RunToCompletion();
            Result = algo.Similarity;

            Source1 = new SourceModel(Path.GetFileNameWithoutExtension(sourcePath1), tokens1.GetJoinedTokenString());
            Source2 = new SourceModel(Path.GetFileNameWithoutExtension(sourcePath2), tokens2.GetJoinedTokenString());
            SQLFacade.Instance.CreateComparison(name, Result, evalRunID, Source1.ID, Source2.ID);
        }
    }
}
