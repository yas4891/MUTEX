using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTEvaluation.storage;

namespace GSTEvaluation.model
{
    class ComparisonHistoryModel
    {
        internal struct HistoryDataPoint
        {
            public Int64 EvaluationRunID;
            public string EvaluationRunLabel;
            public Int32 Result;
            public Int64 Runtime;
        }

        public string Name { get; private set; }

        /// <summary>
        /// returns the data in tuples of [EvaluationRunID, Comparison-Result]
        /// </summary>
        public IEnumerable<HistoryDataPoint> Data { get; private set; }

        public ComparisonHistoryModel(string comparisonName)
        {
            Name = comparisonName;
            Data = SQLFacade.Instance.GetComparisonHistory(comparisonName);
        }


        public static IList<ComparisonHistoryModel> AllHistories 
        { 
            get
            {
                var list = new List<ComparisonHistoryModel>();

                foreach(string name in SQLFacade.Instance.GetComparisonNames())
                    list.Add(new ComparisonHistoryModel(name));
                return list;
            }
        }
    }
}
