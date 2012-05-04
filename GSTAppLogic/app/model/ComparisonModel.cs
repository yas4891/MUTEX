using System.Collections.Generic;
using CTokenizer;
using DataRepository;
using GSTAppLogic.ext;
using GSTLibrary.tile;
using GSTLibrary.token;
using Tokenizer;

namespace GSTAppLogic.app.model
{
    /// <summary>
    /// controls the comparison operation
    /// </summary>
    public class ComparisonModel
    {
        public static readonly int DEFAULT_MML = 4;
        public int MaximumSimilarity { get; private set; }
        
        private readonly IEnumerable<SourceEntityData> ReferenceData;

        public IEnumerable<TokenWrapper> Tokens { get; private set; } 

        /// <summary>
        /// stores the tokens and the referenceData for comparison
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="referenceData"></param>
        public ComparisonModel(IEnumerable<TokenWrapper> tokens, IEnumerable<SourceEntityData> referenceData)
        {
            Tokens = tokens;
            ReferenceData = referenceData;
        }

        /// <summary>
        /// creates the token list from the file at pathToFile
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <param name="referenceData"></param>
        public ComparisonModel(string pathToFile, IEnumerable<SourceEntityData> referenceData ) : 
            this(LexerHelper.CreateLexer(pathToFile).GetTokenWrappers(), referenceData)
        {
        }

        /// <summary>
        /// starts the comparison. 
        /// After this method has completed you can retrieve the maximum found similarity 
        /// </summary>
        public int Start()
        {
            int max = 0;
            foreach (var data in ReferenceData)
            {
                // create the list here, because this way it is local to this run
                // ==> more functional and separated
                var gstTokenList = Tokens.ToGSTTokenList(); 
                var referenceTokens = data.Tokens.ToGSTTokenList();
                var algorithm = new GSTAlgorithm<GSTToken<TokenWrapper>>(gstTokenList, referenceTokens)
                {
                    MinimumMatchLength = DEFAULT_MML
                };

                algorithm.RunToCompletion();

                if (max < algorithm.Similarity)
                    max = algorithm.Similarity;
            }

            MaximumSimilarity = max;
            return max;
        }
    }
}
