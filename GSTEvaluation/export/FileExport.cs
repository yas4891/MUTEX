using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GSTEvaluation.model;

namespace GSTEvaluation.export
{
    /// <summary>
    /// base class for file-based exports. 
    /// defines the default behaviour for those types of exports
    /// </summary>
    abstract class FileExport : IExport
    {
        public static readonly string RESULT_DIRECTORY = @"D:\test\MUTEX\Results";

        private string filePath;

        public string Name { get; protected set; }
        
        /// <summary>
        /// returns the file name extension (including the dot) for the export file
        /// </summary>
        protected abstract string FileExtension { get; }

        /// <summary>
        /// returns the path to the file which is used by this export to export the data.
        /// The path is determined by the RESULT_DIRECTORY, the Name property, an incremental file number and the 
        /// FileExtension property
        /// Format: [RESULT_DIRECTORY]\[Export-Name]\[FileNumber][FileExtension]
        /// e.g.: D:\test\Results\Export01\0001.xlsx
        /// </summary>
        protected string FilePath
        {
            get
            {
                filePath = filePath ?? (filePath = GetFileNumber());

                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                return filePath;
            }
        }

        /// <summary>
        /// tries every possible combination from 0001-9999
        /// </summary>
        /// <returns></returns>
        private string GetFileNumber()
        {
            var baseName = Path.Combine(RESULT_DIRECTORY, Name);

            for(int i = 0; i < 10000; i++)
            {
                var name = string.Format("{0}\\{1:0000}{2}", baseName, i, FileExtension);
                
                //Console.WriteLine(name);

                if (!File.Exists(name))
                    return name;
            }

            var backup_name = Path.Combine(RESULT_DIRECTORY, Path.ChangeExtension(Path.GetRandomFileName(), FileExtension));
            
            Console.WriteLine("could not find a valid filename, using backup instead: {0}", backup_name);
            return backup_name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Run(EvaluationRunModel model)
        {
            WriteData(CreateContent(model));
        }

        public void Run(IList<ComparisonHistoryModel> data)
        {
            WriteData(CreateContent(data));
        }


        private void WriteData(MemoryStream instream)
        {
            string filePath = FilePath;
            Console.WriteLine("FilePath:{0}", filePath);

            using(instream)
            {
                instream.Seek(0, SeekOrigin.Begin);
                try
                {
                    using (var outstream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        instream.CopyTo(outstream);
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine("could not create file at {0}", filePath);
                    Console.WriteLine(ex.ToString());
                }
                catch (IOException ex)
                {
                    Console.WriteLine("could not write to file at {0}", filePath);
                    Console.WriteLine(ex.ToString());
                }
            } // using instream
        }

        /// <summary>
        /// creates the content and returns it in a memory stream
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected abstract MemoryStream CreateContent(EvaluationRunModel model);

        /// <summary>
        /// creates the content and returns it in a memory stream
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected abstract MemoryStream CreateContent(IList<ComparisonHistoryModel> data);
    }
}
