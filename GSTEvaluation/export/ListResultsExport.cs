using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTEvaluation.model;

namespace GSTEvaluation.export
{
    /// <summary>
    /// lists the comparison results on the Console
    /// </summary>
    class ListResultsExport : IExport
    {
        public string Name
        {
            get { return string.Empty; }
        }


        public void Run(EvaluationRunModel model)
        {
            Console.WriteLine("Evaluation Run {0}", model.ID);
            foreach (var comparison in model.Comparisons)
            {
                Console.WriteLine("{0}:\t\t{1}", comparison.Name, comparison.Result);
            }
        }

        public void Run(IList<ComparisonHistoryModel> data)
        {
            throw new NotImplementedException("this is not supported for this export");
        }
    }
}
