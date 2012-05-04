using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTokenizer;
using DataRepository;
using GSTAppLogic.app.model;

namespace GSTAppLogic.app
{
    /// <summary>
    /// implementation of the IAppLogic interface
    /// </summary>
    public class AppLogicImpl : IAppLogic
    {
        /// <summary>
        /// the maximum similarity found
        /// </summary>
        public int MaximumSimilarity { get; private set; }

        public void Start(string student, string assignment, string source)
        {
            var repo = Repository.GetRepository();

            var tokens = LexerHelper.CreateLexerFromSource(source).GetTokenWrappers().ToList();

            var comparisonModel = new ComparisonModel(tokens, repo.LoadByAssignment(assignment));

            MaximumSimilarity = comparisonModel.Calculate();

            var sourceEntityData = new SourceEntityData(student, assignment, tokens.ToStringEnumerable(), source);

            repo.Store(sourceEntityData, MaximumSimilarity > 50);
        }
    }
}
