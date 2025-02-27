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
    public partial class ChartForm : Form
    {
        private DataGridView dataTable;
        private Chart chart;
        public ChartForm()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi; // Включаем масштабирование по DPI
            InitializeDataGridView();
            InitializeChart();
        }
        private void InitializeDataGridView()
        {
            dataTable = new DataGridView();
            dataTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //dataTable.Location = new Point(406, 12);
            dataTable.Dock = DockStyle.Right;
            dataTable.Name = "dataTable";
            dataTable.Size = new Size(650, 225);
            dataTable.TabIndex = 0;
            dataTable.ColumnCount = 4;
            dataTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataTable.Columns[0].Width = 50;
            dataTable.Columns[1].Width = 100;
            dataTable.Columns[0].HeaderText = "ξ";
            dataTable.Columns[1].HeaderText = "Частота";
            dataTable.Columns[2].HeaderText = "Относительная частота";
            dataTable.Columns[3].HeaderText = "Вероятность";
            this.Controls.Add(dataTable);
        }
        //private void InitializeChart()
        //{
        //    chart = new Chart();
        //    chart.Parent = this; // Добавляем в форму
        //    chart.Dock = DockStyle.Bottom; // Прикрепляем к нижней части
        //    chart.Height = this.ClientSize.Height / 2; // 50% от высоты формы

        //    var chartArea = new ChartArea("MainArea");
        //    chart.ChartAreas.Add(chartArea);
        //    var series = new Series("Data")
        //    {
        //        ChartType = SeriesChartType.Line
        //    };
        //    chart.Series.Add(series);
        //    this.Controls.Add(chart);
        //}
        private int ParseAxisComboBox(string text)
        {
            switch (text)
            {
                case ("По частоте"):
                    return 1;
                case ("По практ. вероятности"):
                    return 2;
                case ("По теорет. вероятности"):
                    return 3;
                default
                    :
                    return 0;
            }
        }
        private double GeometricDistribution(double p, double q, int m)
            => p * Math.Pow(q, m);

        private Dictionary<int, double> Adict;
        private List<double> _mainData = new();
        private List<double> GetRundomNumbers()
        {
            List<double> rundomeNumbers = new();
            var rand = new Random();
            int m = (int)parM.Value;
            for (int i = 0; i < m; i++)
            {
                rundomeNumbers.Add(rand.NextDouble());
            }
            return rundomeNumbers;
        }
        private void FillTable()
        {
            int m = (int)parM.Value;
            double p = (double)parP.Value;
            double q = 1 - p;
            int n = _mainData.Count;
            for (int i = 0; i < n; i++)
            {
                dataTable.Rows.Add(p, q, i + 1, _mainData[i]);
            }
        }
        private void SimulateGeometricDistribution()
        {
            double p = (double)parP.Value; // Вероятность успеха
            int iterations = (int)parM.Value; // Количество экспериментов
            double q = 1 - p;

            var rand = new Random();
            var frequency = new Dictionary<int, int>();

            // Инициализация словаря (от 1 до 50)
            for (int m = 1; m <= 50; m++)
            {
                frequency[m] = 0;
            }

            // Симуляция N экспериментов
            for (int j = 0; j < iterations; j++)
            {
                int m = 1;
                while (rand.NextDouble() > p) // Считаем шаги до успеха
                {
                    m++;
                    if (m > 50) break; // Ограничение, чтобы не выйти за пределы
                }
                if (m <= 50) frequency[m]++;
            }

            for (int m = 1; m <= 50; m++)
            {
                double theoreticalProbability = p * Math.Pow(q, m - 1);
                double practicalProbability = (double)frequency[m] / iterations;
                dataTable.Rows.Add(m, frequency[m], practicalProbability, theoreticalProbability);
            }
        }
        private void InitializeChart()
        {
            chart = new Chart();
            chart.Parent = this; // Добавляем в форму
            chart.Dock = DockStyle.Bottom; // Прикрепляем к нижней части
            chart.Height = this.ClientSize.Height / 2; // 50% от высоты формы

            var chartArea = new ChartArea("MainArea");
            chart.ChartAreas.Add(chartArea);

            // Серия для практической вероятности
            var practicalSeries = new Series("PracticalProbability")
            {
                ChartType = SeriesChartType.Column, // Используем столбцы
                Color = Color.Blue // Цвет для практической вероятности
            };
            chart.Series.Add(practicalSeries);

            // Серия для теоретической вероятности
            var theoreticalSeries = new Series("TheoreticalProbability")
            {
                ChartType = SeriesChartType.Column, // Используем столбцы
                Color = Color.Red // Цвет для теоретической вероятности
            };
            chart.Series.Add(theoreticalSeries);

            this.Controls.Add(chart);
        }

        private void PrepareChart()
        {
            ChartIsAbsentLabel.Visible = false;

            // Очистка старых данных
            chart.Series["PracticalProbability"].Points.Clear();
            chart.Series["TheoreticalProbability"].Points.Clear();

            // Установка типа графика
            chart.Series["PracticalProbability"].ChartType = Enum.Parse<SeriesChartType>(chartTypeComboBox.Text);
            chart.Series["TheoreticalProbability"].ChartType = Enum.Parse<SeriesChartType>(chartTypeComboBox.Text);
        }

        private void BuildPracticalChart()
        {
            foreach (DataGridViewRow row in dataTable.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[2].Value != null)
                {
                    if (double.TryParse(row.Cells[0].Value.ToString(), out double x) &&
                        double.TryParse(row.Cells[2].Value.ToString(), out double practicalY))
                    {
                        // Добавляем точки для практической вероятности
                        chart.Series["PracticalProbability"].Points.AddXY(x, practicalY);
                    }
                }
            }
        }

        private void BuildTheoreticalChart()
        {
            foreach (DataGridViewRow row in dataTable.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[3].Value != null)
                {
                    if (double.TryParse(row.Cells[0].Value.ToString(), out double x) &&
                        double.TryParse(row.Cells[3].Value.ToString(), out double theoreticalY))
                    {
                        // Добавляем точки для теоретической вероятности
                        chart.Series["TheoreticalProbability"].Points.AddXY(x, theoreticalY);
                    }
                }
            }
        }

        private void FinalizeChart()
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

            ConfigureChart();
        }

        private void PrintChart()
        {
            PrepareChart(); // Подготовка графика
            switch (ChartComboBox.Text)
            {
                case ("По практ. вероятности"):
                    BuildPracticalChart(); // Построение практической вероятности
                    break;
                case ("По теорет. вероятности"):
                    BuildTheoreticalChart(); // Построение теоретической вероятности
                    break;
                default:
                    BuildTheoreticalChart(); // Построение теоретической вероятности
                    BuildPracticalChart(); // Построение практической вероятности
                    break;
            }
            FinalizeChart(); // Завершение настройки
        }

        private void PrintPracticalChart()
        {
            PrepareChart(); // Подготовка графика
            BuildPracticalChart(); // Построение практической вероятности
            FinalizeChart(); // Завершение настройки
        }

        private void PrintTheoreticalChart()
        {
            PrepareChart(); // Подготовка графика
            BuildTheoreticalChart(); // Построение теоретической вероятности
            FinalizeChart(); // Завершение настройки
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

        private void RestBTN_Click(object sender, EventArgs e)
        {
            dataTable.Rows.Clear();
            parP.Value = 0.1M;
            parM.Value = 1_000_000;
            chart.Series["PracticalProbability"].Points.Clear();
            chart.Series["TheoreticalProbability"].Points.Clear();
            chartTypeComboBox.SelectedIndex = 0;
            ChartComboBox.SelectedIndex = 0;
            ChartIsAbsentLabel.Visible = true;
            chart.ChartAreas[0].AxisX.ScaleView.ZoomReset(); // Сбросить зум по X
            chart.ChartAreas[0].AxisY.ScaleView.ZoomReset(); // Сбросить зум по Y
        }


        private void chartTypeComboBox_TextChanged(object sender, EventArgs e)
        {
            var series = chart.Series["PracticalProbability"];
            series.ChartType = Enum.Parse<SeriesChartType>(chartTypeComboBox.Text);
            series = chart.Series["TheoreticalProbability"];
            series.ChartType = Enum.Parse<SeriesChartType>(chartTypeComboBox.Text);
        }
        private void ChartComboBox_TextChanged(object sender, EventArgs e)
        {
            if (dataTable.Rows.Count > 2)
                PrintChart();
        }

        private void FillAndBuildBtn_Click(object sender, EventArgs e)
        {
            dataTable.Rows.Clear();
            SimulateGeometricDistribution();
            PrintChart();
        }

        private void CompareBtn_Click(object sender, EventArgs e)
        {
            var compareForm = new CompareForm((double)parP.Value);
            compareForm.Show();
        }

        private void AllLinesBtn_Click(object sender, EventArgs e)
        {

            var allLinesForm = new AllLinesForm((double)parP.Value);
            allLinesForm.Show();
        }
    }
}
