using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTokenizer;
using DataRepository;
using GSTAppLogic.app.model;
using log4net;

namespace GSTAppLogic.app
{
    /// <summary>
    /// implementation of the IAppLogic interface
    /// </summary>
    public class AppLogicImpl : IAppLogic
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof(AppLogicImpl).Name);
        /// <summary>
        /// the maximum similarity found
        /// </summary>
        public int MaximumSimilarity { get; private set; }

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
            var tokens = LexerHelper.CreateLexerFromSource(source).GetTokenWrappers().ToList();
            var repo = Repository.GetRepository();

            var comparisonModel = new ComparisonModel(tokens, repo.LoadByAssignment(assignment));

            MaximumSimilarity = comparisonModel.Calculate();

            var sourceEntityData = new SourceEntityData(student, assignment, tokens.ToStringEnumerable(), source);

            repo.Store(sourceEntityData, MaximumSimilarity < Threshold);
        }
    }
}
