namespace WinForms
{
    partial class WinForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.curvesComboBox = new System.Windows.Forms.ComboBox();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.currentScale = new System.Windows.Forms.Label();
            this.drawButton = new System.Windows.Forms.Button();
            this.scaleLabel = new System.Windows.Forms.Label();
            this.curveLabel = new System.Windows.Forms.Label();
            this.buttonPlus = new System.Windows.Forms.Button();
            this.buttonMinus = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(200, 40);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(695, 532);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // curvesComboBox
            // 
            this.curvesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.curvesComboBox.FormattingEnabled = true;
            this.curvesComboBox.Location = new System.Drawing.Point(12, 67);
            this.curvesComboBox.Name = "curvesComboBox";
            this.curvesComboBox.Size = new System.Drawing.Size(182, 28);
            this.curvesComboBox.TabIndex = 1;
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(-11, 578);
            this.trackBar.Maximum = 30;
            this.trackBar.Minimum = 1;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(999, 69);
            this.trackBar.TabIndex = 2;
            this.trackBar.Value = 10;
            this.trackBar.Scroll += new System.EventHandler(this.TrackBarValueChanged);
            // 
            // currentScale
            // 
            this.currentScale.BackColor = System.Drawing.Color.PaleGreen;
            this.currentScale.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.currentScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.currentScale.Location = new System.Drawing.Point(17, 163);
            this.currentScale.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.currentScale.Name = "currentScale";
            this.currentScale.Size = new System.Drawing.Size(90, 90);
            this.currentScale.TabIndex = 3;
            this.currentScale.Text = "1";
            this.currentScale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // drawButton
            // 
            this.drawButton.BackColor = System.Drawing.Color.Red;
            this.drawButton.Location = new System.Drawing.Point(17, 287);
            this.drawButton.Name = "drawButton";
            this.drawButton.Size = new System.Drawing.Size(90, 48);
            this.drawButton.TabIndex = 4;
            this.drawButton.Text = "Draw";
            this.drawButton.UseVisualStyleBackColor = false;
            this.drawButton.Click += new System.EventHandler(this.ButtonDrawClick);
            // 
            // scaleLabel
            // 
            this.scaleLabel.AutoSize = true;
            this.scaleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.scaleLabel.ForeColor = System.Drawing.Color.Black;
            this.scaleLabel.Location = new System.Drawing.Point(21, 113);
            this.scaleLabel.Name = "scaleLabel";
            this.scaleLabel.Size = new System.Drawing.Size(86, 29);
            this.scaleLabel.TabIndex = 5;
            this.scaleLabel.Text = "Scale:";
            // 
            // curveLabel
            // 
            this.curveLabel.AutoSize = true;
            this.curveLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.curveLabel.Location = new System.Drawing.Point(12, 23);
            this.curveLabel.Name = "curveLabel";
            this.curveLabel.Size = new System.Drawing.Size(101, 29);
            this.curveLabel.TabIndex = 6;
            this.curveLabel.Text = "Curves:";
            // 
            // buttonPlus
            // 
            this.buttonPlus.Location = new System.Drawing.Point(114, 163);
            this.buttonPlus.Name = "buttonPlus";
            this.buttonPlus.Size = new System.Drawing.Size(75, 39);
            this.buttonPlus.TabIndex = 7;
            this.buttonPlus.Text = "+0.1";
            this.buttonPlus.UseVisualStyleBackColor = true;
            this.buttonPlus.Click += new System.EventHandler(this.ChangeValueClick);
            // 
            // buttonMinus
            // 
            this.buttonMinus.Location = new System.Drawing.Point(114, 217);
            this.buttonMinus.Name = "buttonMinus";
            this.buttonMinus.Size = new System.Drawing.Size(75, 39);
            this.buttonMinus.TabIndex = 8;
            this.buttonMinus.Text = "-0.1";
            this.buttonMinus.UseVisualStyleBackColor = true;
            this.buttonMinus.Click += new System.EventHandler(this.ChangeValueClick);
            // 
            // WinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 644);
            this.Controls.Add(this.buttonMinus);
            this.Controls.Add(this.buttonPlus);
            this.Controls.Add(this.curveLabel);
            this.Controls.Add(this.scaleLabel);
            this.Controls.Add(this.drawButton);
            this.Controls.Add(this.currentScale);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.curvesComboBox);
            this.Controls.Add(this.pictureBox);
            this.MaximumSize = new System.Drawing.Size(1000, 700);
            this.MinimumSize = new System.Drawing.Size(1000, 700);
            this.Name = "WinForm";
            this.Text = "Curve";
            this.Load += new System.EventHandler(this.TrackBarValueChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ComboBox curvesComboBox;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label currentScale;
        private System.Windows.Forms.Button drawButton;
        private System.Windows.Forms.Label scaleLabel;
        private System.Windows.Forms.Label curveLabel;
        private System.Windows.Forms.Button buttonPlus;
        private System.Windows.Forms.Button buttonMinus;
    }
}

