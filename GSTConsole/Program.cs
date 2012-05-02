using System;
using System.IO;
using GSTAppLogic.app;

namespace GSTConsole
{
    static class Program
    {
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
#if DEBUG
                Console.ReadLine();
#endif
                Environment.Exit(-1);
            }
            
            string source;
            using (var reader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                source = reader.ReadToEnd();
            }

            var appLogic = AppLogic.GetAppLogic();
            appLogic.Start(student, assignment, source);
            
            Console.WriteLine("Similarity:{0}", appLogic.MaximumSimilarity);
#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}
