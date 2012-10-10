using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GSTAppLogic.ext;
using GSTEvaluation.model;
using GSTLibrary.tile;
using GSTLibrary.token;
using SmartXLS;
using System.IO;
using Tokenizer;
using CTokenizer;
using System.Globalization;

namespace GSTEvaluation.export
{
    /// <summary>
    /// runs MUTEX for a subset of all available 
    /// </summary>
    class CompleteComparisonReport : FileExport
    {
        private readonly string testName;
        public CompleteComparisonReport(string name)
        {
            testName = name;
            Name = "CompleteComparison\\" + name;
        }

        protected override string FileExtension
        {
            get { return ".txt"; }
        }

        protected override MemoryStream CreateContent(EvaluationRunModel model)
        {
            throw new NotImplementedException();
        }

        protected override MemoryStream CreateContent(IList<ComparisonHistoryModel> data)
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            var cartesianProduct = GetUniqueCartesianProduct(testName);
            var results =
                cartesianProduct.Select(elem => Calculate(testName, elem)).Select(
                    el => string.Format("{0}: {1}", el[0], el[1]));

            File.WriteAllLines(FilePath, results);
            Console.WriteLine("Finished complete comparison report");
        }

        /// <summary>
        /// evaluation of GST speed only. 
        /// DO NOT TRY THIS AT HOME!
        /// </summary>
        public static void PerformTest(string testName)
        {
            //File.Copy(@"D:\test\MUTEX\CompleteComparison\template-complete.xlsx", @"D:\test\MUTEX\CompleteComparison\template-complete2.xlsx", true);

            var cartesianProduct = GetUniqueCartesianProduct(testName);
            var results = cartesianProduct.Select(elem => Calculate(testName, elem));



            var wb = new WorkBook();
            wb.readXLSX(@"D:\test\MUTEX\CompleteComparison\template-complete2.xlsx");

            SetActiveSheet(testName, wb);

            int i = 0;
            try
            {
                foreach (var elem in results)
                {
                    wb.setEntry(i + 1, 1, string.Format("{0}.0", elem[1]));

                    i++;
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                Environment.Exit(0);
            }
            

            wb.writeXLSX(@"D:\test\MUTEX\CompleteComparison\heyja.xlsx");
        }

        private static List<int[]> GetUniqueCartesianProduct(string testName)
        {
            var files = Directory.GetFiles(string.Format(@"D:\test\MUTEX\CompleteComparison\sources\{0}", testName),
                                           @"main.c", SearchOption.AllDirectories).Select(p =>
                                                                                              {
                                                                                                  var dn =
                                                                                                      new FileInfo(p).
                                                                                                          DirectoryName;
                                                                                                  return
                                                                                                      Int32.Parse(
                                                                                                          dn.Substring(
                                                                                                              dn.LastIndexOf(
                                                                                                                  '\\') + 1));
                                                                                              }).ToList();

            var cp = from first in files
                     from second in files
                     orderby first , second
                     select new[] {first, second};

            var cartesianProduct = cp.Where(elem => elem[0] < elem[1]).ToList();
            return cartesianProduct;
        }

        private static void SetActiveSheet(string testName, WorkBook wb)
        {
            for (int i = 0; i < wb.NumSheets; i++)
            {
                if (testName == wb.getSheetName(i))
                {
                    Console.WriteLine("Setting active sheet {0}", i);
                    wb.Sheet = i;
                    break;
                }
            }
        }

        private static string[] Calculate(string testName, int[] elem)
        {
            var path1 = string.Format(
                @"D:\test\MUTEX\CompleteComparison\sources\{0}\{1:00}\main.c",
                testName, elem[0]);
            var path2 = string.Format(
                @"D:\test\MUTEX\CompleteComparison\sources\{0}\{1:00}\main.c",
                testName, elem[1]);
            return new[]
                       {
                           string.Format("{0}-{1}", elem[0], elem[1]),
                           AppHelper.CompareFiles(path1, path2).ToString(CultureInfo.InvariantCulture)
                       };
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
    }
}
