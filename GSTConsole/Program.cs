using System;
using System.Diagnostics;
using System.IO;
using GSTAppLogic.app;
using log4net;
using log4net.Config;

namespace GSTConsole
{
    static class Program
    {
        private static readonly ILog cLogger = LogManager.GetLogger(typeof (Program).Name);

        static void Main(string[] args)
        {
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
    }
}
