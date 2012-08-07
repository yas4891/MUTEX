using System;
using System.Collections.Generic;
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
            /*
            if(1 < data.Count)
                wb.insertSheets(0, data.Count - 1);
            /* */

            wb.setSheetName(0, "data");
            
            for (int i = 0; i < data.Count; i++)
            {
                //Console.WriteLine("COUNT:{0},{1}", list.Count, i);
                var compHistory = data[i];
                var columnOffset = i*2;
                
                
                wb.setEntry(0, columnOffset, "EvalRun ID");
                wb.setEntry(0, columnOffset + 1, string.Format("Result {0}", compHistory.Name));
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

            //insert sheet and transform into chart sheeet
            wb.insertSheets(sheetIndex, 1);
            ChartShape tChart = wb.addChartSheet(sheetIndex);

            wb.Sheet = sheetIndex;

            wb.PrintScaleFitToPage = true;
            wb.PrintLandscape = true;

            wb.setSheetName(sheetIndex, string.Format("Diagramm_{0}", compHistory.Name));
            tChart.ChartType = ChartShape.Scatter;

            tChart.setAxisTitle(ChartShape.XAxis, 0, "Evaluation Run ID");
            tChart.setAxisTitle(ChartShape.YAxis, 0, "Similarity [%]");

            ChartFormat tFormat = tChart.PlotFormat;
            tFormat.setLineNone();

            tChart.setSeriesName(0, compHistory.Name);
            var format = tChart.getSeriesFormat(0);
            format.MarkerStyle = ChartFormat.MarkerCircle;
            tChart.setSeriesFormat(0, format);

            string xFormula = string.Format("data!${0}${1}:${0}${2}", getDataColumnIndex(columnOffset), 2, compHistory.Data.Count());
            Console.WriteLine("Hello: {0}", xFormula);
            tChart.setSeriesXValueFormula(0, xFormula);
            string yFormula = string.Format("data!${0}${1}:${0}${2}", getDataColumnIndex(columnOffset + 1),2, compHistory.Data.Count());
            tChart.setSeriesYValueFormula(0, yFormula);

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
            for(int i = 0; i < list.Count; i++)
            {
                var tuple = list[i];
                wb.setEntry(i + 1, columnOffset, tuple.Item1.ToString()); // eval Run ID
                wb.setEntry(i + 1, columnOffset + 1, tuple.Item2.ToString()); // result
            }
        }


        private static char getDataColumnIndex(int aMeasurementIndex)
        {
            return (char)('A' + (char)aMeasurementIndex);
        }
    }
}
