using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CTokenizer;
using GSTAppLogic.app;
using GSTAppLogic.ext;
using GSTLibrary.tile;
using GSTLibrary.token;
using Tokenizer;
using log4net;
using System.Linq;
using log4net.Config;
using System.Web;

namespace GSTConsole
{
    static class Program
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof (Program).Name);

        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
            cLogger.DebugFormat("starting MUTEX");
            cLogger.DebugFormat("64-bit process: {0}", Environment.Is64BitProcess);
            Console.WriteLine();

            string student;
            string assignment;
            string path;
            int threshold;

            if(!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("QUERY_STRING")))
            {
                var queryString = Environment.GetEnvironmentVariable("QUERY_STRING");
                cLogger.DebugFormat("reading from query string: {0}", queryString);

                
                var normalizedQS = queryString.Replace("+", " ");


                assignment = GeneralHelper.GetAssignmentIdentifier(normalizedQS);
                student = GeneralHelper.GetStudentIdentifier(normalizedQS);
                path = GeneralHelper.GetPath(normalizedQS);
                threshold = GeneralHelper.GetThreshold(normalizedQS);
            }
            else
            {
                student = GeneralHelper.GetStudentIdentifierFromArgs(args);
                assignment = GeneralHelper.GetAssignmentIdentifierFromArgs(args);
                path = GeneralHelper.GetPathFromArgs(args);
                threshold = GeneralHelper.GetThresholdFromArgs(args);
            }

            HandleInputErrors(assignment, student, path);

            cLogger.DebugFormat("assignment: {0}, student: {1}, path: {2}", assignment, student, path);
            Stopwatch watch = Stopwatch.StartNew();

            string source = File.ReadAllText(path);

            var appLogic = AppLogic.GetAppLogic();
            appLogic.Threshold = threshold;
            appLogic.Start(student, assignment, source);
            
            cLogger.DebugFormat("total runtime: {0} ms", watch.ElapsedMilliseconds);
            Console.WriteLine("{0}", appLogic.MaximumSimilarity);

#if DEBUG
            if(Environment.UserInteractive)
                Console.ReadLine();
#endif
        }

        private static void HandleInputErrors(string assignment, string student, string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("could not find file at {0}. aborting", path);
                cLogger.DebugFormat("could not find file at {0}. aborting", path);
                cLogger.DebugFormat("current working directory: {0}", new DirectoryInfo(".").FullName);
                PrintUsageInformation();
                Environment.Exit(1);
            }
            else if (string.IsNullOrWhiteSpace(student))
            {
                Console.WriteLine("no student identifier. ");
                cLogger.DebugFormat("no student identifier. ");
                PrintUsageInformation();
                Environment.Exit(2);
            }
            else if (string.IsNullOrWhiteSpace(assignment))
            {
                Console.WriteLine("no assignment identifier"); 
                cLogger.DebugFormat("no assignment identifier");
                PrintUsageInformation();
                Environment.Exit(3);
            }
        }

        private static void PrintUsageInformation()
        {
            Console.WriteLine("Usage: {OPTIONS} --student [STUDENT_ID] --assignment [ASSIGNMENT_ID] [PATH_TO_FILE]");
        }

        private static GSTTokenList<GSTToken<TokenWrapper>> GetTokens(FileInfo file)
        {
            string source = File.ReadAllText(file.FullName);
            var tokens = LexerHelper.CreateLexerFromSource(source).GetTokenWrappers().ToList();

            return tokens.ToGSTTokenList();
        }

        private static GSTTokenList<GSTToken<TokenWrapper>> GetTokens(string file)
        {
            return GetTokens(new FileInfo(file));
        }

        /// <summary>
        /// evaluation of overall speed only.
        /// 
        /// DO NOT TRY THIS AT HOME!
        /// </summary>
        private static void EvaluateOverallSpeed()
        {
            var fileContents = Directory.GetFiles(@"C:\Quelltexte", @"main.c", SearchOption.AllDirectories).Select(File.ReadAllText).ToList();

            Console.WriteLine("number of files: {0}", fileContents.Count());
            var appLogic = AppLogic.GetAppLogic();

            var watch = Stopwatch.StartNew();
            foreach(var content in fileContents)
            {
                appLogic.Start("speedTest", "speedTest", content);
            }

            Console.WriteLine("runtime: {0} seconds", watch.Elapsed.TotalSeconds);
            Console.ReadLine();
            Environment.Exit(0);
        }

        /// <summary>
        /// evaluation of GST speed only. 
        /// DO NOT TRY THIS AT HOME!
        /// </summary>
        private static void EvaluateGSTSpeed()
        {
            var files = Directory.GetFiles(@"C:\Quelltexte", @"main.c", SearchOption.AllDirectories).Select(GetTokens).ToList();

            
            

            //Console.WriteLine(string.Format("total product count: {0}", cartesianProduct.Count()));

            long cRuntime = 0;
            int similarity = 0;
            IEnumerable<GSTTokenList<GSTToken<TokenWrapper>>[]> product = null;

            for (int i = 100; i >= 0; i--)
            {
                var cartesianProduct = from first in files
                                       from second in files
                                       select new[] { first, second };

                GSTAlgorithm<GSTToken<TokenWrapper>> algorithm = null;
                foreach (var set in cartesianProduct)
                {
                    var alg = new GSTAlgorithm<GSTToken<TokenWrapper>>(set[0], set[1]) {MinimumMatchLength = 5};
                    algorithm = alg;
                    var watch = Stopwatch.StartNew();
                    alg.RunToCompletion();

                    cRuntime += watch.ElapsedTicks;
                    watch.Stop();
                }
                similarity += algorithm.Similarity;
                product = cartesianProduct;
            }

            Console.WriteLine("Number: {0}, Similarity: {1}", product.Count(), similarity);



            Console.WriteLine("finished in {0} seconds", TimeSpan.FromTicks(cRuntime).TotalSeconds / 100);
            Console.ReadLine();
            Environment.Exit(0);
        }

        /// <summary>
        /// evaluation of accuracy only. 
        /// DO NOT TRY THIS AT HOME!
        /// </summary>
        private static void doEvaluate()
        {
            var doneList = new List<string>();

            var args = Environment.GetCommandLineArgs();
            
            var dir = new DirectoryInfo(@"myDir");

            if(args.Length > 1)
                dir = new DirectoryInfo(args[1]);

            Console.WriteLine("directory: {0}", dir.FullName);
            foreach (var subdir in dir.GetDirectories().OrderBy(sd => sd.Name))
            {
                DirectoryInfo subdir1 = subdir;
                var otherFiles = dir.GetDirectories().Where(od => subdir1.Name != od.Name).OrderBy(od => od.Name).Select(od => new FileInfo(Path.Combine(od.FullName, "main.c")));

                
                var fileA = new FileInfo(Path.Combine(subdir.FullName, "main.c"));

                var tokensA = GetTokens(fileA);


                foreach(var fileB in otherFiles)
                {
                    var format1 = string.Format("{0}-{1}", fileA.Directory.Name, fileB.Directory.Name);
                    var format2 = string.Format("{1}-{0}", fileA.Directory.Name, fileB.Directory.Name);

                    if (doneList.Any(str => str.Equals(format1) || str.Equals(format2)))
                        continue;

                    doneList.Add(format1);
                    var tokensB = GetTokens(fileB);

                    var algo = new GSTAlgorithm<GSTToken<TokenWrapper>>(tokensA, tokensB) {MinimumMatchLength = 5};
                    algo.RunToCompletion();
                    Console.WriteLine("{0}-{1}:{2}", fileA.Directory.Name, fileB.Directory.Name, algo.Similarity);
                }
            }
            

            Console.WriteLine("FINISHED EVAL");
            Environment.Exit(0);
        }

    }
}
