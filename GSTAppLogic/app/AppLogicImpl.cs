using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTokenizer;
using DataRepository;
using GSTAppLogic.app.model;
using log4net;
using Tokenizer;

namespace GSTAppLogic.app
{
    /// <summary>
    /// implementation of the IAppLogic interface
    /// </summary>
    public class AppLogicImpl : IAppLogic
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof(AppLogicImpl).Name);
        private ComparisonModel comparisonModel;

        /// <summary>
        /// the maximum similarity found
        /// </summary>
        public int MaximumSimilarity
        {
            get { return null != comparisonModel ? comparisonModel.MaximumSimilarity : 0; }
        }

        /// <summary>
        /// the student identifier for the maximum match found
        /// </summary>
        public string MaxSimilarityStudentIdentifier
        {
            get { return null != comparisonModel ? comparisonModel.MaxSimilarityStudentID : string.Empty; }
        }

        /// <summary>
        /// the threshold above which a source is considered to be plagiarism
        /// </summary>
        public int Threshold { get; set; }
        
        
        /// <summary>
        /// calculates the maximum similarity of the provided source against
        /// all sources for the given asignment in the reference database
        /// </summary>
        /// <param name="student"></param>
        /// <param name="assignment"></param>
        /// <param name="source"></param>
        public void Start(string student, string assignment, string source)
        {
            cLogger.DebugFormat("starting with threshold {0}", Threshold);
            TokenFactory factory = new MutexTokenFactory();

            var tokens = factory.GetTokenWrapperListFromSource(source); 
            //LexerHelper.CreateLexerFromSource(source).GetTokenWrappers().ToList();
            cLogger.Debug("tokenized source... loading Data Repository");
            var repo = Repository.GetRepository();

            comparisonModel = new ComparisonModel(tokens, repo.LoadByAssignment(assignment), factory);

            comparisonModel.Calculate();

            var sourceEntityData = new SourceEntityData(student, assignment, tokens.ToStringEnumerable(), source);

            repo.Store(sourceEntityData, MaximumSimilarity < Threshold);
        }
    }
}
