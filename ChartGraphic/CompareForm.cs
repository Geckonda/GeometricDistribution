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
    public partial class CompareForm : Form
    {
        private readonly double P;
        private readonly int[] _quantityTries = new int[] { 100, 1_000, 10_000, 1_000_000, 10_000_000 };
        FlowLayoutPanel flowLayoutPanel;
        //private List<double[]> dataTable = new List<double[]>();
        //private Chart chart;
        public CompareForm(double p)
        {
            InitializeComponent();
            InitTable();
            this.P = p;
            InitAllCharts();
        }
        private void InitTable()
        {
            flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight; // Направление размещения

            this.Controls.Add(flowLayoutPanel);
        }
        private List<double[]> SimulateGeometricDistribution(int iterations)
        {
            double q = 1 - P;
            const int n = 10;

            var rand = new Random();
            var frequency = new Dictionary<int, int>();
            List<double[]> dataTable = new List<double[]>();

            // Инициализация словаря (от 1 до 50)
            for (int m = 1; m <= n; m++)
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
                    if (m > n) break; // Ограничение, чтобы не выйти за пределы
                }
                if (m <= n) frequency[m]++;
            }

            for (int m = 1; m <= n; m++)
            {
                double theoreticalProbability = P * Math.Pow(q, m - 1);
                double practicalProbability = (double)frequency[m] / iterations;

                var data = new double[] { m, frequency[m], practicalProbability, theoreticalProbability };
                dataTable.Add(data);
            }
            return dataTable;
        }

        private void BuildPracticalChart(Chart chart,List<double[]> dataTable)
        {
            foreach (var row in dataTable)
            {
                if (double.TryParse(row[0].ToString(), out double x) &&
                    double.TryParse(row[2].ToString(), out double practicalY))
                {
                    // Добавляем точки для практической вероятности
                    chart.Series["PracticalProbability"].Points.AddXY(x, practicalY);
                }
            }
        }

        private void BuildTheoreticalChart(Chart chart, List<double[]> dataTable)
        {
            foreach (var row in dataTable)
            {
                if (double.TryParse(row[0].ToString(), out double x) &&
                    double.TryParse(row[3].ToString(), out double theoreticalY))
                {
                    // Добавляем точки для теоретической вероятности
                    chart.Series["TheoreticalProbability"].Points.AddXY(x, theoreticalY);
                }
            }
        }

        private void FinalizeChart(Chart chart)
        {
            // Находим максимальное значение для практической вероятности (если есть точки)
            double maxPractical = chart.Series["PracticalProbability"].Points.Count > 0
                ? chart.Series["PracticalProbability"].Points.Max(p => p.YValues[0])
                : double.MinValue;

            // Находим максимальное значение для теоретической вероятности (если есть точки)
            double maxTheoretical = chart.Series["TheoreticalProbability"].Points.Count > 0
                ? chart.Series["TheoreticalProbability"].Points.Max(p => p.YValues[0])
                : double.MinValue;

            // Устанавливаем максимальное значение оси Y
            chart.ChartAreas[0].AxisY.Maximum = Math.Max(maxPractical, maxTheoretical);

            ConfigureChart(chart);
        }
        private void ConfigureChart(Chart chart)
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
        private void PrintChart(Chart chart, List<double[]> dataTable)
        {
            BuildTheoreticalChart(chart, dataTable); // Построение теоретической вероятности
            BuildPracticalChart(chart, dataTable); // Построение практической вероятности
            FinalizeChart(chart); // Завершение настройки
        }
        private Chart InitializeChart()
        {
            var chart = new Chart();
            chart.Parent = flowLayoutPanel; // Добавляем в форму
            //chart.Dock = DockStyle.Bottom; // Прикрепляем к нижней части
            chart.Height = ClientSize.Height; // 50% от высоты формы
            chart.Width = 287; // 50% от высоты формы

            var chartArea = new ChartArea("MainArea");
            chart.ChartAreas.Add(chartArea);

            // Серия для практической вероятности
            var practicalSeries = new Series("PracticalProbability")
            {
                ChartType = SeriesChartType.Column, // Используем столбцы
                Color = Color.Blue // Цвет для практической вероятности
            };
            chart.Series.Add(practicalSeries);
            //chart.Series[0].IsValueShownAsLabel = true; // Включить отображение значений

            // Серия для теоретической вероятности
            var theoreticalSeries = new Series("TheoreticalProbability")
            {
                ChartType = SeriesChartType.Column, // Используем столбцы
                Color = Color.Red // Цвет для теоретической вероятности
            };
            chart.Series.Add(theoreticalSeries);
            return chart;
        }

        private void InitAllCharts()
        {
            foreach (var item in _quantityTries)
            {
                var dataTable = SimulateGeometricDistribution(item);
                var chart = InitializeChart();
                chart.Titles.Add($"При {item}");
                flowLayoutPanel.Controls.Add(chart);
                PrintChart(chart, dataTable);
            }
        }

    }
}
