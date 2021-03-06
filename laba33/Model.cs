using OxyPlot;
using OxyPlot.Series;

namespace laba3
{
    class Model
    {
        /// <summary>
        /// Основная математичесская функция
        /// </summary>
        /// <returns>Результат расчетов</returns>
        private static double Function(double x, double a)
        {
            return (a * a * a) / (a * a + x * x);
        }

        /// <summary>
        /// Функция для получения точек графика в заданном диапазоне
        /// </summary>
        /// <param name="a"> Параметр A</param>
        /// <param name="x0"> Левая граница </param>
        /// <param name="x1"> Правая граница </param>
        /// <param name="step"> Шаг изменения X </param>
        /// <returns>Серия точек заданной функции</returns>
        public static FunctionSeries GetFunctionSeries(double a, double x0, double x1, double step)
        {
            FunctionSeries serie = new FunctionSeries();
            double x = x0;
            while (x < x1)
            {
                serie.Points.Add(new DataPoint(x, Function(x, a)));
                x += step;
            }
            serie.Points.Add(new DataPoint(x1, Function(x1, a)));
            return serie;
        }
    }
}
