using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CTokenizer;
using DataRepository;
using GSTAppLogic.app.model;

namespace GSTAppLogic.app
{
    public class AppLogicImpl : IAppLogic
    {
        //private IRepository DataRepository = Repository.GetRepository();
        private ComparisonModel Comparison = null;

        public int MaximumSimilarity
        {
            get { return null != Comparison ? Comparison.MaximumSimilarity : 0; }
        }

        public void Start(string student, string assignment, string source)
        {
            var repo = Repository.GetRepository();

            var tokens = LexerHelper.CreateLexerFromSource(source).GetTokenWrappers().ToList();

            Comparison = new ComparisonModel(tokens, repo.LoadByAssignment(assignment));

            Comparison.Start();

            var sourceEntityData = new SourceEntityData(student, assignment, tokens.GetTokensAsStrings(), source);

            repo.Store(sourceEntityData, MaximumSimilarity > 50);
        }
    }
}
