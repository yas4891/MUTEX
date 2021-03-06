﻿using System;
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
using System.Threading;
using GSTLibrary.test.tile;
using System.Globalization;
using ComparisonModel = GSTAppLogic.app.model.ComparisonModel;

namespace GSTEvaluation
{
    public class Program
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof(Program));

        public static readonly string TEST_SUITE_DIRECTORY = @"test\default_set";
        private const int testRuns = 1;
        private static string src1;
        private static string src2;
        private static int[] Length1, Length2, Length3;
        private static readonly List<string> ResultList = new List<string>();

        #region helper for EvaluateSpeed
        private static void Calculate(int len)
        {
            var string1 = src1.Substring(0, len);
            var string2 = src2.Substring(0, len);
            //len += 200; // double length in size
            var watch = Stopwatch.StartNew();

            //Console.WriteLine("String length: A = {0}, B = {1}", string1.Length, string2.Length);
            for (int i = testRuns; i >= 0; i--)
            {
                var algo = new HashingGSTAlgorithm<GSTToken<char>>(
                    GSTHelper.FromString(string1),
                    GSTHelper.FromString(string2));

                algo.RunToCompletion();
                //Console.WriteLine("finished hashing run {0} in {1}", i, watch.Elapsed);
            }
            var runtimeHashing = watch.Elapsed;
            //Console.WriteLine("runtime hashing: {0}", ((int)runtimeHashing.TotalMilliseconds / testRuns));

            watch = Stopwatch.StartNew();

            for (int i = testRuns; i >= 0; i--)
            {
                var algo = new GSTAlgorithm<GSTToken<char>>(
                    GSTHelper.FromString(string1),
                    GSTHelper.FromString(string2));

                algo.RunToCompletion();

            }
            var ratio = (double)watch.ElapsedTicks / runtimeHashing.Ticks;
            Console.WriteLine("runtimes: {0}, {1}, ratio: {2} for {3}",
                watch.ElapsedMilliseconds / testRuns,
                runtimeHashing.TotalMilliseconds / testRuns,
                ratio, 
                len);

            ResultList.Sort(String.CompareOrdinal);
            ResultList.Add(string.Format("{3:000};{0};{1};{2}",
                (watch.Elapsed.TotalMilliseconds / testRuns),
                (runtimeHashing.TotalMilliseconds / testRuns),
                ratio,
                len));
        }

        public static void Start1()
        {
            Console.WriteLine("starting one with array: {0}", Length1.Length);

            foreach(var len in Length1)
                Calculate(len);

            Console.WriteLine("finished 1");
        }
        public static void Start2()
        {
            Console.WriteLine("starting 2 with array: {0}", Length2.Length);

            foreach (var len in Length2)
                Calculate(len);
            Console.WriteLine("finished 2");
        }
        public static void Start3()
        {
            Console.WriteLine("starting 3 with array: {0}", Length3.Length);

            foreach (var len in Length3)
                Calculate(len);

            Console.WriteLine("finished 3");
        }
        #endregion

        /// <summary>
        /// compares the two algorithms against each other over a rather large file (~8000 chars)
        /// </summary>
        private static void EvaluateSpeed()
        {
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
            src1 = File.ReadAllText(@"test\default_set\TPLV04-S01-02\main-01.c");
            src2 = File.ReadAllText(@"test\default_set\TPLV04-S01-02\main-02.c");
            var testRuns = 2;

            int len = 100;

            // JIT compile both algorithms before we start
            AbstractGSTAlgorithm<GSTToken<char>> initalgo = new HashingGSTAlgorithm<GSTToken<char>>(
                        GSTHelper.FromString(src1.Substring(0, 100)),
                        GSTHelper.FromString(src2.Substring(0, 100)));

            initalgo.RunToCompletion();

            initalgo = new GSTAlgorithm<GSTToken<char>>(GSTHelper.FromString(src1.Substring(0, 100)),
                        GSTHelper.FromString(src2.Substring(0, 100))); 
            initalgo.RunToCompletion();

            var liLen1 = new List<int>();
            var liLen2 = new List<int>();
            var liLen3 = new List<int>();
            int c = 0;

            while(len < src1.Length && len < src2.Length)
            {
                switch (c++ % 3)
                {
                    case 0:
                        liLen1.Add(len);
                        break;
                    case 1:
                        liLen2.Add(len);
                        break;
                    default:
                        liLen3.Add(len);
                        break;
                }

                len += 100;
            }

            Length1 = liLen1.ToArray();
            Length2 = liLen2.ToArray();
            Length3 = liLen3.ToArray();

            var t1 = new Thread(Start1);
            t1.Start();

            var t2 = new Thread(Start2);
            t2.Start();

            var t3 = new Thread(Start3);
            t3.Start();


            Console.WriteLine("Main thread joining");
            t1.Join();
            t2.Join();
            t3.Join();

            File.WriteAllLines(@"test\default_set\performance.txt", ResultList);

            Console.WriteLine("finished all runs");
            Console.ReadLine();
        }

        /// <summary>
        /// evaluates a 
        /// </summary>
        private static void EvaluateCompleteSetOfSources()
        {
            new CompleteComparisonReport("01_01").Run();
            Console.WriteLine("Finished complete set of sources");
            Console.ReadLine();
        }

        /// <summary>
        /// just the default comparison
        /// </summary>
        private static void EvaluateStandardSet()
        {
            try
            {
                var factory = new MutexTokenFactory();
                var watch = Stopwatch.StartNew();
                var evalModel = new EvaluationRunModel(TEST_SUITE_DIRECTORY);
                cLogger.DebugFormat("evaluation run finished in {0} ms", watch.ElapsedMilliseconds);
                new ListResultsExport().Run(evalModel);

                File.WriteAllLines(@"test\tokens\tok1.txt", factory.GetTokenWrapperListFromFile(@"test\tokens\main-01.c").GetDebugTokenStrings());
                File.WriteAllLines(@"test\tokens\tok2.txt", factory.GetTokenWrapperListFromFile(@"test\tokens\main-03.c").GetDebugTokenStrings());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //if ("y" == decision)
            {
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
            Console.ReadLine();
        }

        public static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));


            
            //EvaluateSpeed();
            
            //EvaluateCompleteSetOfSources()
            
            EvaluateStandardSet();
            
            /* */
        }
    }
}
