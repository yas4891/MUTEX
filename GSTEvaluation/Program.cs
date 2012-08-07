using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GSTEvaluation.export;
using GSTEvaluation.model;
using GSTEvaluation.storage;
using System.Diagnostics;

namespace GSTEvaluation
{
    public class Program
    {
        public static readonly string TEST_SUITE_DIRECTORY = @"D:\test\tokenizerEval";

        public static void Main(string[] args)
        {
            try
            {
                var watch = Stopwatch.StartNew();
                var evalModel = new EvaluationRunModel(TEST_SUITE_DIRECTORY);
                Console.WriteLine("evaluation run finished in {0} ms", watch.ElapsedMilliseconds);
                new ListResultsExport().Run(evalModel);

                new ComparisonExcelExport("comparisons").Run(
                    new[]
                    {
                        new ComparisonHistoryModel("DupV01-S01-13"),       
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
