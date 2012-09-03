using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using GSTEvaluation.storage;
using log4net;

namespace GSTEvaluation.model
{
    /// <summary>
    /// represents a whole evaluation run. 
    /// Also does the actual evaluation
    /// </summary>
    class EvaluationRunModel
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof(EvaluationRunModel));
        public Int64 ID { get; private set; }
        public IEnumerable<ComparisonModel> Comparisons { get; private set; } 

        /// <summary>
        /// this constructor creates the model in the database and organizes the 
        /// comparison of all files 
        /// </summary>
        /// <param name="directory"></param>
        public EvaluationRunModel(string directory)
        {
            ID = SQLFacade.Instance.CreateEvaluationRun();
            var list = new List<ComparisonModel>();

            foreach(var dir in new DirectoryInfo(directory).GetDirectories())
            {
                try
                {
                    var file1 = dir.GetFiles("main*")[0];
                    var file2 = dir.GetFiles("main*")[1];
                    var watch = Stopwatch.StartNew();
                    var comparison = new ComparisonModel(dir.Name, ID, file1.FullName, file2.FullName);
                    cLogger.DebugFormat("comparison {0} took {1}", comparison.Name, watch.ElapsedMilliseconds);
                    list.Add(comparison);
                }
                catch (Exception ex)
                {
                    cLogger.Debug(string.Format("could not make comparison: {0}", dir.FullName), ex);
                }
            }

            Comparisons = list;
        }
    }
}
