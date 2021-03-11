using System;
using Microsoft.Office.Interop.Excel;
using OxyPlot.Series;

namespace laba3
{
    /// <summary>
    /// Класс с данными для графика
    /// </summary>
    internal class LiveData
    {
        public readonly double A;
        public readonly FunctionSeries Series;
        public readonly double Step;
        public readonly double X0;
        public readonly double X1;

        public LiveData(double a, double x0, double x1, double step)
        {
            if (x0 >= x1) throw new Exception();
            A = a;
            X0 = x0;
            X1 = x1;
            Step = step;
            Series = Model.GetFunctionSeries(a, x0, x1, step);
        }

        /// <summary>
        ///     Экспортирование данных в таблицу Excel
        /// </summary>
        public void ExportToExcel()
        {
            var excelApp = new Application();
            excelApp.Visible = false;
            excelApp.Workbooks.Add();
            _Worksheet workSheet = (Worksheet) excelApp.ActiveSheet;

            workSheet.Cells[1, "A"] = "Параметр а";
            workSheet.Cells[2, "A"] = A;
            workSheet.Cells[1, "B"] = "Левая граница";
            workSheet.Cells[2, "B"] = X0;
            workSheet.Cells[1, "C"] = "Правая граница";
            workSheet.Cells[2, "C"] = X1;
            workSheet.Cells[1, "D"] = "Шаг";
            workSheet.Cells[2, "D"] = Step;

            workSheet.Cells[1, "F"] = "Функция: a³/a²+x²";

            workSheet.Cells[4, "A"] = "X";
            workSheet.Cells[4, "B"] = "Y";


            var row = 4;
            foreach (var point in Series.Points)
            {
                row++;
                workSheet.Cells[row, "A"] = point.X;
                workSheet.Cells[row, "B"] = point.Y;
            }

            workSheet.Columns[1].AutoFit();
            workSheet.Columns[2].AutoFit();
            workSheet.Columns[3].AutoFit();
            workSheet.Columns[4].AutoFit();
            workSheet.Columns[5].AutoFit();
            workSheet.Columns[6].AutoFit();
            excelApp.Visible = true;
        }
    }
}