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
            dataTable.Columns[2].HeaderText = "Практическая вероятность";
            dataTable.Columns[3].HeaderText = "Теоретическая вероятность";
            this.Controls.Add(dataTable);
        }
        private void InitializeChart()
        {
            chart = new Chart();
            chart.Parent = this; // Добавляем в форму
            chart.Dock = DockStyle.Bottom; // Прикрепляем к нижней части
            chart.Height = this.ClientSize.Height / 2; // 50% от высоты формы

            var chartArea = new ChartArea("MainArea");
            chart.ChartAreas.Add(chartArea);
            var series = new Series("Data")
            {
                ChartType = SeriesChartType.Line
            };
            chart.Series.Add(series);
            this.Controls.Add(chart);
        }
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
        private void FillTableBtn_Click(object sender, EventArgs e)
        {
            dataTable.Rows.Clear();
            SimulateGeometricDistribution();
            PrintChartBTN.Enabled = true;
        }
        private void SimulateGeometricDistribution()
        {
            double p = (double)parP.Value; // Вероятность успеха
            int iterations = (int)parM.Value; // Количество экспериментов
            double q = 1 - p;

            var rand = new Random();
            var frequency = new Dictionary<int, int>();

            // Инициализация словаря (от 1 до 100)
            for (int m = 1; m <= 100; m++)
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
                    if (m > 100) break; // Ограничение, чтобы не выйти за пределы
                }
                if (m <= 100) frequency[m]++;
            }

            for (int m = 1; m <= 100; m++)
            {
                double theoreticalProbability = p * Math.Pow(q, m - 1);
                double practicalProbability = (double)frequency[m] / iterations;
                dataTable.Rows.Add(m, frequency[m], practicalProbability, theoreticalProbability);
            }
        }
        private void PrintChart()
        {
            ChartIsAbsentLabel.Visible = false;
            var series = chart.Series["Data"];
            series.Points.Clear(); // Очистка старых данных
            series.ChartType = Enum.Parse<SeriesChartType>(chartTypeComboBox.Text);
            var axisX = ParseAxisComboBox(AxisComboBox.Text);
            foreach (DataGridViewRow row in dataTable.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[axisX].Value != null)
                {
                    if (double.TryParse(row.Cells[0].Value.ToString(), out double x) &&
                        double.TryParse(row.Cells[axisX].Value.ToString(), out double y))
                    {
                        series.Points.AddXY(x, y);
                    }
                }
            }
            chart.ChartAreas[0].AxisY.Maximum = series.Points.Max(p => p.YValues[0]);

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
        private void PrintChartBTN_Click(object sender, EventArgs e)
        {
            PrintChart();
            PrintChartBTN.Enabled = false;
        }

        private void RestBTN_Click(object sender, EventArgs e)
        {
            dataTable.Rows.Clear();
            parP.Value = 0.1M;
            parM.Value = 1_000_000;
            chart.Series["Data"].Points.Clear();
            PrintChartBTN.Enabled = false;
            chartTypeComboBox.SelectedIndex = 0;
            ChartIsAbsentLabel.Visible = true;
            chart.ChartAreas[0].AxisX.ScaleView.ZoomReset(); // Сбросить зум по X
            chart.ChartAreas[0].AxisY.ScaleView.ZoomReset(); // Сбросить зум по Y
        }


        private void chartTypeComboBox_TextChanged(object sender, EventArgs e)
        {
            var series = chart.Series["Data"];
            series.ChartType = Enum.Parse<SeriesChartType>(chartTypeComboBox.Text);
        }
        private void AxisComboBox_TextChanged(object sender, EventArgs e)
        {
            if(dataTable.Rows.Count > 2)
                PrintChart();
        }

        private void FillAndBuildBtn_Click(object sender, EventArgs e)
        {
            dataTable.Rows.Clear();
            SimulateGeometricDistribution();
            PrintChartBTN.Enabled = true;
            PrintChart();
            PrintChartBTN.Enabled = false;
        }

       
    }
}
