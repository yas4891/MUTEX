using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GSTEvaluation.model;
using SmartXLS;

namespace GSTEvaluation.export
{
    class ComparisonExcelExport : ExcelExport
    {
        public ComparisonExcelExport(string name) : base(name)
        {
        }

        protected override void WriteData(WorkBook wb, EvaluationRunModel model)
        {
            throw new NotImplementedException("not supported for this export, please use multi method");
        }

        protected override void WriteData(WorkBook wb, IList<ComparisonHistoryModel> data)
        {
            wb.insertSheets(1, data.Count);
            wb.setSheetName(0, "data");
            
            for (int i = 0; i < data.Count; i++)
            {
                wb.Sheet = DataSheetIndex;
                var compHistory = data[i];
                var columnOffset = i*3;

                if (!compHistory.Data.Any())
                {
                    Console.WriteLine("no data points for: {0}", compHistory.Name);
                    continue;
                }
                
                Console.WriteLine("Setting column headers: offset: {0}, name: {1}", columnOffset, compHistory.Name);
                
                WriteDataColumns(wb, compHistory, columnOffset);
                WriteChart(wb, compHistory, columnOffset, i + 1);
            }
            /* */
        }

        /// <summary>
        /// writes the chart
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="compHistory"></param>
        /// <param name="columnOffset"></param>
        private void WriteChart(WorkBook wb, ComparisonHistoryModel compHistory, int columnOffset, int sheetIndex)
        {
            Console.WriteLine("write chart sheetIndex: {0}", sheetIndex);

            // transform sheet into chart sheeet
            
            var chartShape = wb.addChartSheet(sheetIndex);

            wb.Sheet = sheetIndex;

            wb.PrintScaleFitToPage = true;
            wb.PrintLandscape = true;

            wb.setSheetName(sheetIndex, string.Format("Diagram_{0}", compHistory.Name));
            chartShape.ChartType = ChartShape.Scatter;

            chartShape.setAxisTitle(ChartShape.XAxis, 0, "Evaluation Run ID");
            chartShape.setAxisTitle(ChartShape.YAxis, 0, "Similarity [%]");

            ChartFormat tFormat = chartShape.PlotFormat;
            tFormat.setLineNone();

            chartShape.setSeriesName(0, compHistory.Name);
            var format = chartShape.getSeriesFormat(0);
            format.MarkerStyle = ChartFormat.MarkerCircle;
            chartShape.setSeriesFormat(0, format);

            string xFormula = string.Format("data!${0}${1}:${0}${2}", GetDataColumnIndex(columnOffset), 2, compHistory.Data.Count());
            
            chartShape.setSeriesXValueFormula(0, xFormula);
            string yFormula = string.Format("data!${0}${1}:${0}${2}", GetDataColumnIndex(columnOffset + 2),2, compHistory.Data.Count());
            Console.WriteLine("xFormula: {0}, yFormula: {1}", xFormula, yFormula);
            
            chartShape.setSeriesYValueFormula(0, yFormula);
        }

        /// <summary>
        /// write
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="compHistory"></param>
        /// <param name="columnOffset"></param>
        private void WriteDataColumns(WorkBook wb, ComparisonHistoryModel compHistory, int columnOffset)
        {
            var list = compHistory.Data.ToList();
            
            Console.WriteLine("Data column: {0}, {1}, count: {2}", compHistory.Name, columnOffset, list.Count);

            wb.setEntry(0, columnOffset, "EvalRun ID");
            wb.setEntry(0, columnOffset + 1, "Label");
            wb.setEntry(0, columnOffset + 2, string.Format("Result {0}", compHistory.Name));

            for(int i = 0; i < list.Count; i++)
            {
                var tuple = list[i];
                wb.setEntry(i + 1, columnOffset, tuple.EvaluationRunID.ToString(CultureInfo.InvariantCulture));
                wb.setEntry(i + 1, columnOffset + 1, tuple.EvaluationRunLabel);
                wb.setEntry(i + 1, columnOffset + 2, tuple.Result.ToString(CultureInfo.InvariantCulture)); 
            }
        }
    }
}
