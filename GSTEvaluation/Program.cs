using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GSTEvaluation.export;
using GSTEvaluation.model;
using GSTEvaluation.storage;
using System.Diagnostics;
using CTokenizer;
using GSTLibrary.tile;
using GSTLibrary.token;
using Tokenizer;
using log4net;
using log4net.Config;

namespace GSTEvaluation
{
    public class Program
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof(Program));

        public static readonly string TEST_SUITE_DIRECTORY = @"D:\test\MUTEX\tokenizerEval";

        public static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));

            
            /*
            CompleteComparisonReport.PerformTest("01_01");
            Console.ReadLine();
            Environment.Exit(0);
            /* */
            try
            {
                LexerHelper.UsedLexer = typeof (MutexCLexer);
                var watch = Stopwatch.StartNew();
                var evalModel = new EvaluationRunModel(TEST_SUITE_DIRECTORY);
                cLogger.DebugFormat("evaluation run finished in {0} ms", watch.ElapsedMilliseconds);
                new ListResultsExport().Run(evalModel);

                File.WriteAllLines(@"D:\test\testfile.token.txt", LexerHelper.CreateLexer(@"D:\test\test.c").GetDebugTokenStrings());

                File.WriteAllLines(@"D:\test\tok1.txt", LexerHelper.CreateLexer(@"D:\test\main-01.c").GetDebugTokenStrings());
                File.WriteAllLines(@"D:\test\tok2.txt", LexerHelper.CreateLexer(@"D:\test\main-03.c").GetDebugTokenStrings());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //if ("y" == decision)
            {

                //new CompleteComparisonReport("01_01").Run();
                

                new ComparisonExcelExport("comparisons").Run(
                   ComparisonHistoryModel.AllHistories);

                new RuntimeExcelExport("runtime").Run(
                    new[]
                    {
                        new ComparisonHistoryModel("TPLV04-S01-02"), 
                    });
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Done!");
            Console.WriteLine("Type [y] if you want to create excel spreadsheets");
            var decision = Console.ReadLine();
        }
    }
}
