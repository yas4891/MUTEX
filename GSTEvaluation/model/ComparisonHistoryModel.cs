using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTEvaluation.storage;

namespace GSTEvaluation.model
{
    class ComparisonHistoryModel
    {
        public string Name { get; private set; }

        /// <summary>
        /// returns the data in tuples of [EvaluationRunID, Comparison-Result]
        /// </summary>
        public IEnumerable<Tuple<Int64, Int32>> Data { get; private set; }

        public ComparisonHistoryModel(string comparisonName)
        {
            Name = comparisonName;
            Data = SQLFacade.Instance.GetComparisonHistory(comparisonName);
        }

    }
}
