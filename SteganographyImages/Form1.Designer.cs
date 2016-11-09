namespace SteganographyImages {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.picResult = new System.Windows.Forms.PictureBox();
            this.picSorce = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInfo = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnResetData = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.lblMaxData = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnInsertData = new System.Windows.Forms.Button();
            this.btnShowData = new System.Windows.Forms.Button();
            this.btnRandData = new System.Windows.Forms.Button();
            this.lblSize = new System.Windows.Forms.Label();
            this.btnOptimize = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSorce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.picResult);
            this.panel1.Controls.Add(this.picSorce);
            this.panel1.Location = new System.Drawing.Point(12, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(900, 317);
            this.panel1.TabIndex = 0;
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // picResult
            // 
            this.picResult.Dock = System.Windows.Forms.DockStyle.Left;
            this.picResult.Location = new System.Drawing.Point(450, 0);
            this.picResult.Margin = new System.Windows.Forms.Padding(10);
            this.picResult.Name = "picResult";
            this.picResult.Size = new System.Drawing.Size(450, 317);
            this.picResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picResult.TabIndex = 1;
            this.picResult.TabStop = false;
            // 
            // picSorce
            // 
            this.picSorce.Dock = System.Windows.Forms.DockStyle.Left;
            this.picSorce.Location = new System.Drawing.Point(0, 0);
            this.picSorce.Margin = new System.Windows.Forms.Padding(10);
            this.picSorce.Name = "picSorce";
            this.picSorce.Size = new System.Drawing.Size(450, 317);
            this.picSorce.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSorce.TabIndex = 0;
            this.picSorce.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Image";
            // 
            // txtInfo
            // 
            this.txtInfo.Location = new System.Drawing.Point(56, 11);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.ReadOnly = true;
            this.txtInfo.Size = new System.Drawing.Size(414, 20);
            this.txtInfo.TabIndex = 2;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(476, 11);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 20);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnResetData
            // 
            this.btnResetData.Enabled = false;
            this.btnResetData.Location = new System.Drawing.Point(557, 11);
            this.btnResetData.Name = "btnResetData";
            this.btnResetData.Size = new System.Drawing.Size(75, 20);
            this.btnResetData.TabIndex = 4;
            this.btnResetData.Text = "Reset data";
            this.btnResetData.UseVisualStyleBackColor = true;
            this.btnResetData.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(874, 15);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(38, 13);
            this.lblProgress.TabIndex = 5;
            this.lblProgress.Text = "Ready";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Bits";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(56, 37);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(45, 20);
            this.numericUpDown1.TabIndex = 7;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // lblMaxData
            // 
            this.lblMaxData.AutoSize = true;
            this.lblMaxData.Location = new System.Drawing.Point(107, 40);
            this.lblMaxData.Name = "lblMaxData";
            this.lblMaxData.Size = new System.Drawing.Size(23, 13);
            this.lblMaxData.TabIndex = 8;
            this.lblMaxData.Text = "0 B";
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(719, 37);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 20);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save image";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnInsertData
            // 
            this.btnInsertData.Enabled = false;
            this.btnInsertData.Location = new System.Drawing.Point(638, 37);
            this.btnInsertData.Name = "btnInsertData";
            this.btnInsertData.Size = new System.Drawing.Size(75, 20);
            this.btnInsertData.TabIndex = 9;
            this.btnInsertData.Text = "Insert data";
            this.btnInsertData.UseVisualStyleBackColor = true;
            this.btnInsertData.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // btnShowData
            // 
            this.btnShowData.Enabled = false;
            this.btnShowData.Location = new System.Drawing.Point(476, 37);
            this.btnShowData.Name = "btnShowData";
            this.btnShowData.Size = new System.Drawing.Size(75, 20);
            this.btnShowData.TabIndex = 10;
            this.btnShowData.Text = "Show data";
            this.btnShowData.UseVisualStyleBackColor = true;
            this.btnShowData.Click += new System.EventHandler(this.btnShowData_Click);
            // 
            // btnRandData
            // 
            this.btnRandData.Enabled = false;
            this.btnRandData.Location = new System.Drawing.Point(638, 11);
            this.btnRandData.Name = "btnRandData";
            this.btnRandData.Size = new System.Drawing.Size(75, 20);
            this.btnRandData.TabIndex = 4;
            this.btnRandData.Text = "Rand data";
            this.btnRandData.UseVisualStyleBackColor = true;
            this.btnRandData.Click += new System.EventHandler(this.btnRandData_Click);
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(800, 40);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(23, 13);
            this.lblSize.TabIndex = 11;
            this.lblSize.Text = "0 B";
            // 
            // btnOptimize
            // 
            this.btnOptimize.Enabled = false;
            this.btnOptimize.Location = new System.Drawing.Point(557, 37);
            this.btnOptimize.Name = "btnOptimize";
            this.btnOptimize.Size = new System.Drawing.Size(75, 20);
            this.btnOptimize.TabIndex = 9;
            this.btnOptimize.Text = "Optimize";
            this.btnOptimize.UseVisualStyleBackColor = true;
            this.btnOptimize.Click += new System.EventHandler(this.btnOptimize_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 392);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.btnShowData);
            this.Controls.Add(this.btnOptimize);
            this.Controls.Add(this.btnInsertData);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblMaxData);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnRandData);
            this.Controls.Add(this.btnResetData);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.txtInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSorce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picResult;
        private System.Windows.Forms.PictureBox picSorce;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInfo;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnResetData;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label lblMaxData;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnInsertData;
        private System.Windows.Forms.Button btnShowData;
        private System.Windows.Forms.Button btnRandData;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Button btnOptimize;
    }
}

