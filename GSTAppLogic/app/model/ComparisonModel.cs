using System.Collections.Generic;
using CTokenizer;
using DataRepository;
using GSTAppLogic.ext;
using GSTLibrary.tile;
using GSTLibrary.token;
using Tokenizer;
using log4net;

namespace GSTAppLogic.app.model
{
    /// <summary>
    /// controls the comparison operation
    /// </summary>
    public class ComparisonModel
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof(ComparisonModel).Name);
        public static readonly int DEFAULT_MML = 8;

        private readonly IEnumerable<SourceEntityData> ReferenceData;

        /// <summary>
        /// returns the maximum found similarity
        /// </summary>
        public int MaximumSimilarity { get; private set; }

        /// <summary>
        /// contains the student identifier
        /// </summary>
        public string MaxSimilarityStudentID { get; private set; }
        
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<TokenWrapper> Tokens { get; private set; }

        /// <summary>
        /// the token factory used during comparison
        /// </summary>
        public TokenFactory Factory { get; private set; }

        /// <summary>
        /// stores the tokens and the referenceData for comparison
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="referenceData"></param>
        public ComparisonModel(IEnumerable<TokenWrapper> tokens, IEnumerable<SourceEntityData> referenceData, TokenFactory factory)
        {
            Tokens = tokens;
            ReferenceData = referenceData;
            Factory = factory;
        }

        /// <summary>
        /// creates the token list from the file at pathToFile
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <param name="referenceData"></param>
        public ComparisonModel(string pathToFile, IEnumerable<SourceEntityData> referenceData )
        {
            ReferenceData = referenceData;
            var factory = new MutexTokenFactory();
            Tokens = factory.GetTokenWrapperListFromSource(pathToFile);

        }

        /// <summary>
        /// starts the comparison. 
        /// After this method has completed you can retrieve the maximum found similarity 
        /// </summary>
        public int Calculate()
        {
            int max = 0;

            cLogger.DebugFormat("calculating similarity");

            foreach (var data in ReferenceData)
            {
                // create the list here, because this way it is local to this run
                // ==> more functional and separated
                var sourceTokens = Tokens.ToGSTTokenList(); 
                var referenceTokens = Factory.GetTokenWrapperEnumerable(data.Tokens).ToGSTTokenList();
                var algorithm = new HashingGSTAlgorithm<GSTToken<TokenWrapper>>(sourceTokens, referenceTokens)
                {
                    MinimumMatchLength = DEFAULT_MML
                };

                algorithm.RunToCompletion();

                cLogger.DebugFormat("similarity compared to {0}:{1}", data.StudentIdentifier, algorithm.Similarity);
                if (MaximumSimilarity < algorithm.Similarity)
                {
                    MaximumSimilarity = algorithm.Similarity;
                    MaxSimilarityStudentID = data.StudentIdentifier;
                }
            }

            return max;
        }
    }
}
