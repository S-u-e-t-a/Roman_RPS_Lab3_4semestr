using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using System.Configuration;
namespace laba3
{
    public partial class MainWindow
    {
        private LiveData _currentData;

        /// <summary>
        ///     Инициализация формы
        /// </summary>
        public MainWindow()
        {

            InitializeComponent();
            var myModel = new PlotModel();
            Plot.Model = myModel;
            bool ShowHelpOnStart = bool.Parse(ConfigurationManager.AppSettings.Get("ShowHelpOnStart"));
            if (ShowHelpOnStart)
            {
                var help = new HelpForm();
                help.Show();
            }
        }

        /// <summary>
        ///     Функция отрисовки графика
        /// </summary>
        private void DrawGraph(object sender, RoutedEventArgs e)
        {
            try
            {
                var a = double.Parse(inputA.Text);
                var x0 = double.Parse(inputX0.Text);
                var x1 = double.Parse(inputX1.Text);
                var step = double.Parse(inputStep.Text);
                if (step <= 0)
                {
                    MessageBox.Show("Невозможно построить с таким шагом, убедитесь что шаг больше 0", "Ошибка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (x0 >= x1)
                {
                    MessageBox.Show("Невозможно построить график в таком диапазоне значений", "Ошибка!",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _currentData = new LiveData(a, x0, x1, step);
                    UpdateGraph(_currentData);
                }
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "Вы ввели некорректные данные данные, убедитесь что поля ввода не пустые и не содержат только цифры.",
                    "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        ///     Функция для эскпорта в Excel
        /// </summary>
        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            try
            {
                Cursor = Cursors.Wait;
                _currentData.ExportToExcel();
                Cursor = Cursors.Arrow;
            }
            catch (NullReferenceException)
            {
                Cursor = Cursors.Arrow;
                MessageBox.Show("Данные не проинициализированы", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        ///     Функция для получения начальных данных из файла
        /// </summary>
        private void ImportFromFile(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            var result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
                try
                {
                    var initialData = FileSystem.ReadFromFile(dlg.FileName);
                    if (initialData.Count > 4)
                    {
                        MessageBox.Show("В файле найдены ненужные данные", "Ошибка!", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    else if (initialData.Count < 4)
                    {
                        MessageBox.Show("В файле недостаточно данных", "Ошибка!", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    else if (initialData[1] >= initialData[2])
                    {
                        MessageBox.Show("Левая граница должна быть меньше правой!", "Ошибка!", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                    else
                    {
                        _currentData = new LiveData(initialData[0], initialData[1], initialData[2], initialData[3]);
                        inputA.Text = _currentData.A.ToString();
                        inputX0.Text = _currentData.X0.ToString();
                        inputX1.Text = _currentData.X1.ToString();
                        inputStep.Text = _currentData.Step.ToString();
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("В файле найдены некорректные данные", "Ошибка!", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
        }

        /// <summary>
        /// Сохранение начальных данных
        /// </summary>
        private void SaveInitialData(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            var result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                try
                {
                    FileSystem.SaveToFile(dlg.FileName, _currentData.GetInitialData());
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Данные не проинициализированы", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
               
            }

        }
        
        /// <summary>
        ///     Выход из программы
        /// </summary>
        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Вызов справки 
        /// </summary>
        private void ShowHelp(object sender, RoutedEventArgs e)
        {
            var help = new HelpForm();
            help.Owner = this;
            help.ShowDialog();
        }

        /// <summary>
        ///     Обновление графика в соответсвии с текущими данными
        /// </summary>
        /// <param name="liveData">Текущие данные</param>
        private void UpdateGraph(LiveData liveData)
        {
            var model = new PlotModel {Title = "График функции a³/a²+x²"};
            model.LegendPosition = LegendPosition.RightBottom;
            model.LegendPlacement = LegendPlacement.Outside;
            model.LegendOrientation = LegendOrientation.Horizontal;
            model.Series.Add(liveData.Series);

            var yAxis = new LinearAxis();
            var xAxis = new LinearAxis {Position = AxisPosition.Bottom};
            xAxis.Title = "X";
            yAxis.Title = "a³/a²+x²";
            model.Axes.Add(yAxis);
            model.Axes.Add(xAxis);
            model.PlotType = PlotType.Cartesian;
            Plot.Model = model;
        }
    }
}