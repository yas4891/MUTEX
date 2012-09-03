using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTEvaluation.model;
using log4net;

namespace GSTEvaluation.export
{
    /// <summary>
    /// lists the comparison results on the Console
    /// </summary>
    class ListResultsExport : IExport
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof(ListResultsExport));
        public string Name
        {
            get { return string.Empty; }
        }


        public void Run(EvaluationRunModel model)
        {
            cLogger.DebugFormat("Evaluation Run {0}", model.ID);
            foreach (var comparison in model.Comparisons)
            {
                cLogger.InfoFormat("{0}:\t\t{1}", comparison.Name, comparison.Result);
            }
        }

        public void Run(IList<ComparisonHistoryModel> data)
        {
            throw new NotImplementedException("this is not supported for this export");
        }
    }
}
