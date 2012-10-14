using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CTokenizer;
using GSTAppLogic.ext;
using GSTAppLogic.templating;
using GSTEvaluation.storage;
using GSTLibrary.tile;
using GSTLibrary.token;
using Tokenizer;
using log4net;

namespace GSTEvaluation.model
{
    /// <summary>
    /// model representing a comparison between two source files
    /// </summary>
    class ComparisonModel
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof(ComparisonModel));
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
        /// <param name="sourcePath1"></param>
        /// <param name="sourcePath2"></param>
        public ComparisonModel(string name, Int64 evalRunID, string sourcePath1, string sourcePath2)
        {
            Name = name;
            EvaluationRunID = evalRunID;
            
            var directory = Path.GetDirectoryName(Path.GetFullPath(sourcePath1));
            bool tmplFileExists = File.Exists(Path.Combine(directory, "template.c"));

            var watch = Stopwatch.StartNew();

            var tmplFile = Directory.GetFiles(directory, "template.c").FirstOrDefault();
            var path1 = tmplFileExists ? TemplatingHelper.StripTemplateFromSourceFile(sourcePath1, tmplFile) : sourcePath1;
            var path2 = tmplFileExists ? TemplatingHelper.StripTemplateFromSourceFile(sourcePath2, tmplFile) : sourcePath2;
            var factory = new MutexTokenFactory();
            var tokens1 = factory.GetTokenWrapperListFromFile(path1);
            var tokens2 = factory.GetTokenWrapperListFromFile(path2);
            

            cLogger.DebugFormat("TokenStream Length: {0} -- {1}", tokens1.Count(), tokens2.Count());
            var algo = new GSTAlgorithm<GSTToken<TokenWrapper>>(
                tokens1.ToGSTTokenList<TokenWrapper>(),
                tokens2.ToGSTTokenList<TokenWrapper>());

            algo.RunToCompletion();
            Result = algo.Similarity;

            Source1 = new SourceModel(Path.GetFileNameWithoutExtension(sourcePath1), factory.GetJoinedTokenString(tokens1));
            Source2 = new SourceModel(Path.GetFileNameWithoutExtension(sourcePath2), factory.GetJoinedTokenString(tokens2));
            SQLFacade.Instance.CreateComparison(name, Result, watch.ElapsedMilliseconds, evalRunID, Source1.ID, Source2.ID);
        }
    }
}
