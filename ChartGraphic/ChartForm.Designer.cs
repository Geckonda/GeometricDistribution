using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartGraphic
{
    partial class ChartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartForm));
            panel1 = new Panel();
            label4 = new Label();
            ChartComboBox = new ComboBox();
            FillAndBuildBtn = new Button();
            label3 = new Label();
            chartTypeComboBox = new ComboBox();
            RestBTN = new Button();
            label2 = new Label();
            parM = new NumericUpDown();
            label1 = new Label();
            parP = new NumericUpDown();
            ChartIsAbsentLabel = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)parM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)parP).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.GradientInactiveCaption;
            panel1.Controls.Add(label4);
            panel1.Controls.Add(ChartComboBox);
            panel1.Controls.Add(FillAndBuildBtn);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(chartTypeComboBox);
            panel1.Controls.Add(RestBTN);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(parM);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(parP);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(191, 316);
            panel1.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 179);
            label4.Name = "label4";
            label4.Size = new Size(63, 22);
            label4.TabIndex = 9;
            label4.Text = "Данные";
            // 
            // ChartComboBox
            // 
            ChartComboBox.FormattingEnabled = true;
            ChartComboBox.Items.AddRange(new object[] { "Обе вероятности", "По практ. вероятности", "По теорет. вероятности" });
            ChartComboBox.Location = new Point(3, 204);
            ChartComboBox.Name = "ChartComboBox";
            ChartComboBox.Size = new Size(182, 30);
            ChartComboBox.TabIndex = 3;
            ChartComboBox.Text = "Обе вероятности";
            ChartComboBox.TextChanged += ChartComboBox_TextChanged;
            // 
            // FillAndBuildBtn
            // 
            FillAndBuildBtn.Location = new Point(3, 240);
            FillAndBuildBtn.Name = "FillAndBuildBtn";
            FillAndBuildBtn.Size = new Size(182, 33);
            FillAndBuildBtn.TabIndex = 4;
            FillAndBuildBtn.Text = "Заполнить и построить";
            FillAndBuildBtn.UseVisualStyleBackColor = true;
            FillAndBuildBtn.Click += FillAndBuildBtn_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(3, 121);
            label3.Name = "label3";
            label3.Size = new Size(94, 22);
            label3.TabIndex = 7;
            label3.Text = "Тип графика";
            // 
            // chartTypeComboBox
            // 
            chartTypeComboBox.FormattingEnabled = true;
            chartTypeComboBox.Items.AddRange(new object[] { "Column", "Area", "Line", "Point", "Radar" });
            chartTypeComboBox.Location = new Point(3, 146);
            chartTypeComboBox.Name = "chartTypeComboBox";
            chartTypeComboBox.Size = new Size(182, 30);
            chartTypeComboBox.TabIndex = 2;
            chartTypeComboBox.Text = "Column";
            chartTypeComboBox.TextChanged += chartTypeComboBox_TextChanged;
            // 
            // RestBTN
            // 
            RestBTN.Location = new Point(73, 279);
            RestBTN.Name = "RestBTN";
            RestBTN.Size = new Size(112, 33);
            RestBTN.TabIndex = 5;
            RestBTN.Text = "Сброс";
            RestBTN.UseVisualStyleBackColor = true;
            RestBTN.Click += RestBTN_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 63);
            label2.Name = "label2";
            label2.Size = new Size(167, 22);
            label2.TabIndex = 3;
            label2.Text = "Количество испытаний";
            // 
            // parM
            // 
            parM.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            parM.Location = new Point(3, 88);
            parM.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            parM.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            parM.Name = "parM";
            parM.Size = new Size(167, 28);
            parM.TabIndex = 1;
            parM.Value = new decimal(new int[] { 1000000, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 4);
            label1.Name = "label1";
            label1.Size = new Size(90, 22);
            label1.TabIndex = 1;
            label1.Text = "Параметр p";
            // 
            // parP
            // 
            parP.DecimalPlaces = 2;
            parP.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            parP.Location = new Point(3, 29);
            parP.Maximum = new decimal(new int[] { 8, 0, 0, 65536 });
            parP.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            parP.Name = "parP";
            parP.Size = new Size(80, 28);
            parP.TabIndex = 0;
            parP.Value = new decimal(new int[] { 1, 0, 0, 65536 });
            // 
            // ChartIsAbsentLabel
            // 
            ChartIsAbsentLabel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            ChartIsAbsentLabel.AutoSize = true;
            ChartIsAbsentLabel.BackColor = Color.White;
            ChartIsAbsentLabel.Font = new Font("Arial Narrow", 36F, FontStyle.Regular, GraphicsUnit.Point, 204);
            ChartIsAbsentLabel.Location = new Point(300, 400);
            ChartIsAbsentLabel.Name = "ChartIsAbsentLabel";
            ChartIsAbsentLabel.Size = new Size(593, 83);
            ChartIsAbsentLabel.TabIndex = 2;
            ChartIsAbsentLabel.Text = "График не построен";
            // 
            // ChartForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(1178, 701);
            Controls.Add(ChartIsAbsentLabel);
            Controls.Add(panel1);
            Font = new Font("Arial Narrow", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2, 3, 2, 3);
            MaximizeBox = false;
            Name = "ChartForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Геометрическое распределение";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)parM).EndInit();
            ((System.ComponentModel.ISupportInitialize)parP).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private NumericUpDown parP;
        private Label label1;
        private Label label2;
        private NumericUpDown parM;
        private Button RestBTN;
        private Label label3;
        private ComboBox chartTypeComboBox;
        private Button FillAndBuildBtn;
        private Label ChartIsAbsentLabel;
        private Label label4;
        private ComboBox ChartComboBox;
    }
}