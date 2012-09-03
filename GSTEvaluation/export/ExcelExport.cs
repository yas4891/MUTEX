using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GSTEvaluation.model;
using SmartXLS;

namespace GSTEvaluation.export
{
    /// <summary>
    /// base class for excel exports
    /// </summary>
    abstract class ExcelExport : FileExport
    {
        /// <summary>
        /// this is the index of the sheet which contains all of the data
        /// </summary>
        protected const int DataSheetIndex = 0;

        /// <summary>
        /// the file extension is .xlsx
        /// </summary>
        protected override string FileExtension
        {
            get { return ".xlsx"; }
        }

        protected ExcelExport(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override MemoryStream CreateContent(EvaluationRunModel model)
        {
            var wb = new WorkBook();
            WriteData(wb, model);
            var stream = new MemoryStream();
            
            wb.writeXLSX(stream);
            
            return stream;
        }

        protected override MemoryStream CreateContent(IList<ComparisonHistoryModel> data)
        {
            var wb = new WorkBook();
            WriteData(wb, data);
            var stream = new MemoryStream();

            wb.writeXLSX(stream);

            return stream;
        }
        /// <summary>
        /// writes the excel data into the workbook
        /// </summary>
        /// <param name="wb"></param>
        protected abstract void WriteData(WorkBook wb, EvaluationRunModel model);

        protected abstract void WriteData(WorkBook wb, IList<ComparisonHistoryModel> data);


        protected static char GetDataColumnIndex(int aDataColumn)
        {
            return (char)('A' + (char)aDataColumn);
        }
    }
}
