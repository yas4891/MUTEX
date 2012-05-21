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

namespace GSTConsole
{
    static class Program
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof (Program).Name);

        static void Main(string[] args)
        {
            /*
            var lexer = LexerHelper.CreateLexer(@"X:\Katharina und Christoph\Christoph\Studium\Bachelorarbeit\Quelltexte\nach Versuch\1\Reihe\01\main.c");
            Console.WriteLine(lexer.GetJoinedTokenString());
            Console.WriteLine(lexer.GetTokens().Count());
            Environment.Exit(1);
            /* */
            doEvaluate();
            /* */
            string student = null;
            string assignment = null;
            string path = null;
            
            if(!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("QUERY_STRING")))
            {
                var queryString = Environment.GetEnvironmentVariable("QUERY_STRING");

                assignment = GeneralHelper.GetAssignmentIdentifier(queryString);
                student = GeneralHelper.GetStudentIdentifier(queryString);
                path = GeneralHelper.GetPath(queryString);
            }
            else if(3 == args.Length)
            {
                student = args[0];
                assignment = args[1];
                path = args[2];
            }
            else
            {
                Console.WriteLine("Usage: mutex.exe [student_identifier] [assignment_identifier] [path_to_sourcefile]");
                Environment.Exit(-1);
            }

            XmlConfigurator.Configure(new FileInfo("log4net.xml"));
            Stopwatch watch = Stopwatch.StartNew();

            string source;
            using (var reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                source = reader.ReadToEnd();
            }

            var appLogic = AppLogic.GetAppLogic();
            appLogic.Start(student, assignment, source);
            
            cLogger.DebugFormat("total runtime: {0} ms", watch.ElapsedMilliseconds);
            Console.WriteLine("Similarity:{0}", appLogic.MaximumSimilarity);

#if DEBUG
            if(Environment.UserInteractive)
                Console.ReadLine();
#endif
        }

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

        private static GSTTokenList<GSTToken<TokenWrapper>> GetTokens(FileInfo file)
        {
            string source;
            using (var reader = new StreamReader(new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                source = reader.ReadToEnd();
            }
            var tokens = LexerHelper.CreateLexerFromSource(source).GetTokenWrappers().ToList();

            return tokens.ToGSTTokenList();
        }
    }
}
