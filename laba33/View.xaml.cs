using System;
using System.Windows;
using System.Windows.Input;
using OxyPlot;

namespace laba3
{
    public partial class MainWindow
    {
        private LiveData _currentData;

        /// <summary>
        /// Инициализация формы
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            var myModel = new PlotModel();
            this.Plot.Model = myModel;
        }

        /// <summary>
        /// Функция отрисовки графика
        /// </summary>
        private void DrawGraph(object sender, RoutedEventArgs e)
        {
            double a = Double.Parse(inputA.Text);
            double x0 = Double.Parse(inputX0.Text);
            double x1 = Double.Parse(inputX1.Text);
            double step = Double.Parse(inputStep.Text);
            if (x0 >= x1)
            {
                MessageBox.Show("Невозможно построить график в таком диапазоне значений", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _currentData = new LiveData(a,x0,x1,step);
                UpdateGraph(_currentData);
            }
        }

        /// <summary>
        /// Функция для отрисовки графика
        /// </summary>
        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                _currentData.ExportToExcel();
                this.Cursor = Cursors.Arrow;
            }
            catch (NullReferenceException)
            {
                this.Cursor = Cursors.Arrow;
                MessageBox.Show("Данные не проинициализированы", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        /// <summary>
        /// Функция для получения начальных данных из файла
        /// </summary>
        private void ImportFromFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                try
                {
                    var initialData = FileSystem.ReadFromFile(dlg.FileName);
                    if (initialData.Count > 4)
                    {
                        MessageBox.Show("В файле найдены ненужные данные", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (initialData.Count < 4)
                    {
                        MessageBox.Show("В файле недостаточно данных", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (initialData[1]>=initialData[2])
                    {
                        MessageBox.Show("Левая граница должна быть меньше правой!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        _currentData= new LiveData(initialData[0], initialData[1], initialData[2], initialData[3]);
                        inputA.Text = _currentData.A.ToString();
                        inputX0.Text = _currentData.X0.ToString();
                        inputX1.Text = _currentData.X1.ToString();
                        inputStep.Text = _currentData.Step.ToString();

                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("В файле найдены некорректные данные", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        /// <summary>
        /// Выход из программы
        /// </summary>
        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ShowHelp(object sender, RoutedEventArgs e)
        {
            HelpForm help = new HelpForm();
            help.Owner = this;
            help.ShowDialog();
        }

        /// <summary>
        /// Обновление графика в соответсвии с текущими данными
        /// </summary>
        /// <param name="liveData">Текущие данные</param>
        private void UpdateGraph(LiveData liveData)
        {
            var model = new PlotModel { Title = "График функции a³/a²+x²" };
            model.LegendPosition = LegendPosition.RightBottom;
            model.LegendPlacement = LegendPlacement.Outside;
            model.LegendOrientation = LegendOrientation.Horizontal;
            model.Series.Add(liveData.Series);

            var yAxis = new OxyPlot.Axes.LinearAxis();
            var xAxis = new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom };
            xAxis.Title = "X";
            yAxis.Title = "a³/a²+x²";
            model.Axes.Add(yAxis);
            model.Axes.Add(xAxis);
            model.PlotType = PlotType.Cartesian;
            this.Plot.Model = model;
        }
    }
}
