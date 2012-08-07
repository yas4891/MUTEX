using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GSTEvaluation.storage;

namespace GSTEvaluation.model
{
    /// <summary>
    /// represents a whole evaluation run. 
    /// Also does the actual evaluation
    /// </summary>
    class EvaluationRunModel
    {
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
                    var file1 = dir.GetFiles()[0];
                    var file2 = dir.GetFiles()[1];
                    var comparison = new ComparisonModel(dir.Name, ID, file1.FullName, file2.FullName);
                    list.Add(comparison);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("could not make comparison: {0}", dir.FullName);
                    Console.WriteLine(ex.ToString());
                }
            }

            Comparisons = list;
        }
    }
}
