using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GSTEvaluation.model;

namespace GSTEvaluation.export
{
    /// <summary>
    /// defines the interface for an export function.
    /// </summary>
    interface IExport
    {
        /// <summary>
        /// the name of the export. 
        /// this property usually doubles as the name of the directory
        /// where the different revisions of the export are stored
        /// </summary>
        string Name { get; }

        /// <summary>
        /// runs the export
        /// </summary>
        void Run(EvaluationRunModel model);

        /// <summary>
        /// runs the report on multiple evaluation runs
        /// </summary>
        /// <param name="data"></param>
        void Run(IList<ComparisonHistoryModel> data);
    }
}
