using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartGraphic
{
    public partial class AllLinesForm : Form
    {
        private readonly double P;
        private const int N = 15; // Количество кси
        private readonly int[] _quantityTries = new int[] { 100, 1_000, 10_000, 1_000_000, 10_000_000 };
        private readonly Color[] _colors = new Color[] { Color.FromArgb(0, 0, 95), Color.FromArgb(0, 0, 135), Color.FromArgb(0, 0, 175), Color.FromArgb(0, 0, 215), Color.FromArgb(0, 0, 255) };
        private Chart chart;
        private const string redColumnName = "TheoreticalProbability";
        public AllLinesForm(double p)
        {
            InitializeComponent();
            this.P = p;
            InitializeChart();
            PrintChart();
            AddTitlesToChart(chart);
        }
        private Dictionary<int, int> InitDictionary()
        {
            var frequency = new Dictionary<int, int>();
            for (int m = 1; m <= 50; m++)
            {
                frequency[m] = 0;
            }
            return frequency;
        }
        private List<double[]> SimulateGeometricDistribution(int iterations)
        {
            double q = 1 - P;
            var rand = new Random();
            var frequency = new Dictionary<int, int>();

            List<double[]> dataTable = new List<double[]>();

            // Инициализация словаря (от 1 до N)
            for (int m = 1; m <= N; m++)
            {
                frequency[m] = 0;
            }

            // Симуляция N экспериментов
            for (int j = 0; j < iterations; j++)
            {
                int m = 1;
                while (rand.NextDouble() > P) // Считаем шаги до успеха
                {
                    m++;
                    if (m > N) break; // Ограничение, чтобы не выйти за пределы
                }
                if (m <= N) frequency[m]++;
            }

            for (int m = 1; m <= N; m++)
            {
                double practicalProbability = (double)frequency[m] / iterations;
                dataTable.Add([m, practicalProbability]);
            }
            return dataTable;
        }
        private List<double[]> GetTheoreticalProbability()
        {

            double q = 1 - P;
            List<double[]> dataTable = new();
            for (int m = 1; m <= N; m++)
            {
                double theoreticalProbability = P * Math.Pow(q, m - 1);
                dataTable.Add([m, theoreticalProbability]);
            }
            return dataTable;
        }
        private void InitializeChart()
        {
            chart = new Chart();
            chart.Parent = this; // Добавляем в форму
            chart.Dock = DockStyle.Fill;
            chart.Height = ClientSize.Height;

            var chartArea = new ChartArea("MainArea");
            chart.ChartAreas.Add(chartArea);

            // Серия для практической вероятности
            for (int i = 0; i < _quantityTries.Length; i++)
            {
                var serea = new Series(_quantityTries[i].ToString())
                {
                    ChartType = SeriesChartType.Column, // Используем столбцы
                    Color = _colors[i] // Цвет для практической вероятности
                };
                chart.Series.Add(serea);
            }

            // Серия для теоретической вероятности
            var theoreticalSeries = new Series(redColumnName)
            {
                ChartType = SeriesChartType.Column, // Используем столбцы
                Color = Color.Red, // Цвет для теоретической вероятности
                LegendText = "Яркий синий (#0000FF)"
            };
            chart.Series.Add(theoreticalSeries);


            this.Controls.Add(chart);
        }
        private void AddTitlesToChart(Chart chart)
        {
            // Настройка подписей оси X через интервал в 10 единиц
            for (int i = 3, k = 0; i < 15; i += 2, k++)
            {
                CustomLabel label = new CustomLabel();
                label.FromPosition = i - 0.5; // Начало интервала подписи
                label.ToPosition = i + 0.5;   // Конец интервала подписи
                if (_quantityTries.Length <= k)
                {
                    label.Text = $"███\nВероятность";    // Текст подписи
                    label.ForeColor = Color.Red;
                }
                else
                {
                    label.Text = $"███\nПри\n{_quantityTries[k]}";    // Текст подписи
                    label.ForeColor = _colors[k];
                }
                chart.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Arial", 6); // Установите подходящий шрифт и размер
                chart.ChartAreas[0].AxisX.LabelStyle.Angle = 0; // Поворот на 0 градусов
                chart.ChartAreas[0].AxisX.CustomLabels.Add(label);
            }
        }
        private void PrintChart()
        {
            string[] seriesName = new string[_quantityTries.Length + 1];
            for(int i = 0; i < _quantityTries.Length; i++)
                seriesName[i] = _quantityTries[i].ToString();
            seriesName[seriesName.Length - 1] = redColumnName;

            //List<double[]> dataTable = new List<double[]>();
            foreach (var item in _quantityTries)
            {
                var dataTable = SimulateGeometricDistribution(item);
                BuildChartColumn(dataTable, item.ToString());
            }
            BuildChartColumn(GetTheoreticalProbability(), redColumnName);
            FinalizeChart(seriesName); // Завершение настройки
        }
        private void BuildChartColumn(List<double[]> dataTable, string seriesName)
        {
            foreach (var row in dataTable)
            {
                // Добавляем точки для графика
                chart.Series[seriesName].Points.AddXY(row[0], row[1]);
            }
        }

        private void FinalizeChart(string[] seriesName)
        {
            double max = 0;
            foreach (var name in seriesName)
            {
                double maxPractical = chart.Series[name].Points.Count > 0
                ? chart.Series[name].Points.Max(p => p.YValues[0])
                : double.MinValue;

                if(maxPractical > max)
                    max = maxPractical;
            }

            // Устанавливаем максимальное значение оси Y
            chart.ChartAreas[0].AxisY.Maximum = max;

            ConfigureChart();
        }
        private void ConfigureChart()
        {
            // Настройка масштабирования и прокрутки
            chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true; // Разрешает зум по Y
            chart.ChartAreas[0].AxisY.ScrollBar.Enabled = true;  // Добавляет скролл по Y
            chart.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
        }
    }
}
