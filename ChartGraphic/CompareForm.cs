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
            //InitTable();
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
        private Dictionary<int, int> InitDictionary()
        {
            var frequency = new Dictionary<int, int>();
            for (int m = 1; m <= 50; m++)
            {
                frequency[m] = 0;
            }
            return frequency;
        }
        private List<double[]> SimulateGeometricDistribution(Dictionary<int, int> frequency, int beginIndex, int iterations)
        {
            double q = 1 - P;
            int n = beginIndex + 9;

            var rand = new Random();
            List<double[]> dataTable = new List<double[]>();

           

            // Симуляция N экспериментов
            for (int j = 0; j < iterations; j++)
            {
                int m = beginIndex;
                while (rand.NextDouble() > P) // Считаем шаги до успеха
                {
                    m++;
                    if (m > n) break; // Ограничение, чтобы не выйти за пределы
                }
                if (m <= n) frequency[m]++;
            }

            for (int m = beginIndex; m <= n; m++)
            {
                double theoreticalProbability = P * Math.Pow(q, m  - beginIndex);
                double practicalProbability = (double)frequency[m] / iterations;

                var data = new double[] { m, frequency[m], practicalProbability, theoreticalProbability };
                dataTable.Add(data);
            }
            return dataTable;
        }

        private void BuildPracticalChart(Chart chart, List<List<double[]>> dataList)
        {
            foreach (var dataTable in dataList)
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
        }

        private void BuildTheoreticalChart(Chart chart, List<List<double[]>> dataList)
        {
            foreach (var dataTable in dataList)
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
        private void PrintChart(Chart chart, List<List<double[]>> dataList)
        {
            BuildTheoreticalChart(chart, dataList); // Построение теоретической вероятности
            BuildPracticalChart(chart, dataList); // Построение практической вероятности
            FinalizeChart(chart); // Завершение настройки
        }
        private Chart InitializeChart()
        {
            var chart = new Chart();
            chart.Parent = this;
            chart.Dock = DockStyle.Fill;
            chart.Height = ClientSize.Height;
            chart.Width = 100;

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
            var chart = InitializeChart();
            this.Controls.Add(chart);
            List<List<double[]>> dataList = new();
            int beginIndex = 1;
            var dict = InitDictionary();
            foreach (var item in _quantityTries)
            {
                var dataTable = SimulateGeometricDistribution(dict, beginIndex, item);
                dataList.Add(dataTable);
                //chart.Titles.Add($"При {item}");
                //flowLayoutPanel.Controls.Add(chart);
                beginIndex += 10;
            }
            PrintChart(chart, dataList);
            ChangeStripLineColour(chart);
            AddTitlesToChart(chart);
        }
        private void ChangeStripLineColour(Chart chart)
        {
            // Настройка StripLines для фона
            for (int i = 0; i <= 50; i += 10)
            {
                StripLine stripLine = new StripLine();
                stripLine.IntervalOffset = i+0.5; // Начало интервала
                stripLine.StripWidth = 10.5; // Ширина интервала
                stripLine.BackColor = (i / 10) % 2 == 0 ? Color.LightGray : Color.White; // Чередование цветов
                chart.ChartAreas[0].AxisX.StripLines.Add(stripLine);
            }
        }
        private void AddTitlesToChart(Chart chart)
        {
            // Настройка подписей оси X через интервал в 10 единиц
            for (int i = 5, k = 0; i < 55; i += 10, k++)
            {
                CustomLabel label = new CustomLabel();
                label.FromPosition = i - 0.5; // Начало интервала подписи
                label.ToPosition = i + 0.5;   // Конец интервала подписи
                label.Text = $"При {_quantityTries[k]}";    // Текст подписи
                chart.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Поворот на 45 градусов
                chart.ChartAreas[0].AxisX.CustomLabels.Add(label);
            }
        }
    }
}
