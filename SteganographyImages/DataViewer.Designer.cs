namespace SteganographyImages {
    partial class DataViewer {
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
            this.tabViewer = new System.Windows.Forms.TabControl();
            this.tpgText = new System.Windows.Forms.TabPage();
            this.lblFlags = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtText = new System.Windows.Forms.TextBox();
            this.txtEncoding = new System.Windows.Forms.TextBox();
            this.tpgImage = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnSaveFile = new System.Windows.Forms.Button();
            this.lblMD5 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtKeys = new System.Windows.Forms.TextBox();
            this.chbCompression = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chbEncryption = new System.Windows.Forms.CheckBox();
            this.chbNoNulls = new System.Windows.Forms.CheckBox();
            this.tabViewer.SuspendLayout();
            this.tpgText.SuspendLayout();
            this.tpgImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabViewer
            // 
            this.tabViewer.Controls.Add(this.tpgText);
            this.tabViewer.Controls.Add(this.tpgImage);
            this.tabViewer.Controls.Add(this.tabPage1);
            this.tabViewer.Controls.Add(this.tabPage2);
            this.tabViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabViewer.Location = new System.Drawing.Point(0, 0);
            this.tabViewer.Name = "tabViewer";
            this.tabViewer.SelectedIndex = 0;
            this.tabViewer.Size = new System.Drawing.Size(875, 431);
            this.tabViewer.TabIndex = 0;
            this.tabViewer.SelectedIndexChanged += new System.EventHandler(this.tabViewer_SelectedIndexChanged);
            // 
            // tpgText
            // 
            this.tpgText.Controls.Add(this.chbNoNulls);
            this.tpgText.Controls.Add(this.lblFlags);
            this.tpgText.Controls.Add(this.label1);
            this.tpgText.Controls.Add(this.txtText);
            this.tpgText.Controls.Add(this.txtEncoding);
            this.tpgText.Location = new System.Drawing.Point(4, 22);
            this.tpgText.Name = "tpgText";
            this.tpgText.Padding = new System.Windows.Forms.Padding(3);
            this.tpgText.Size = new System.Drawing.Size(867, 405);
            this.tpgText.TabIndex = 0;
            this.tpgText.Text = "Text";
            this.tpgText.UseVisualStyleBackColor = true;
            // 
            // lblFlags
            // 
            this.lblFlags.AutoSize = true;
            this.lblFlags.Location = new System.Drawing.Point(213, 9);
            this.lblFlags.Name = "lblFlags";
            this.lblFlags.Size = new System.Drawing.Size(42, 13);
            this.lblFlags.TabIndex = 2;
            this.lblFlags.Text = "0x0000";
            this.lblFlags.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Encoding";
            // 
            // txtText
            // 
            this.txtText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtText.Location = new System.Drawing.Point(11, 32);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.ReadOnly = true;
            this.txtText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtText.Size = new System.Drawing.Size(848, 365);
            this.txtText.TabIndex = 0;
            this.txtText.TextChanged += new System.EventHandler(this.txtText_TextChanged);
            // 
            // txtEncoding
            // 
            this.txtEncoding.Location = new System.Drawing.Point(82, 6);
            this.txtEncoding.Name = "txtEncoding";
            this.txtEncoding.Size = new System.Drawing.Size(125, 20);
            this.txtEncoding.TabIndex = 0;
            this.txtEncoding.Text = "UTF-8";
            this.txtEncoding.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // tpgImage
            // 
            this.tpgImage.Controls.Add(this.pictureBox1);
            this.tpgImage.Location = new System.Drawing.Point(4, 22);
            this.tpgImage.Name = "tpgImage";
            this.tpgImage.Padding = new System.Windows.Forms.Padding(3);
            this.tpgImage.Size = new System.Drawing.Size(867, 405);
            this.tpgImage.TabIndex = 1;
            this.tpgImage.Text = "Image";
            this.tpgImage.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(8, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(851, 391);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnSaveFile);
            this.tabPage1.Controls.Add(this.lblMD5);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(867, 405);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Binary";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnSaveFile
            // 
            this.btnSaveFile.Location = new System.Drawing.Point(9, 42);
            this.btnSaveFile.Name = "btnSaveFile";
            this.btnSaveFile.Size = new System.Drawing.Size(75, 23);
            this.btnSaveFile.TabIndex = 1;
            this.btnSaveFile.Text = "Save as";
            this.btnSaveFile.UseVisualStyleBackColor = true;
            this.btnSaveFile.Click += new System.EventHandler(this.btnSaveFile_Click);
            // 
            // lblMD5
            // 
            this.lblMD5.AutoSize = true;
            this.lblMD5.Location = new System.Drawing.Point(6, 13);
            this.lblMD5.Name = "lblMD5";
            this.lblMD5.Size = new System.Drawing.Size(56, 13);
            this.lblMD5.TabIndex = 0;
            this.lblMD5.Text = "MD5: N/A";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.chbEncryption);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(867, 405);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "Decryption";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.txtKeys);
            this.panel1.Controls.Add(this.chbCompression);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Enabled = false;
            this.panel1.Location = new System.Drawing.Point(8, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(856, 373);
            this.panel1.TabIndex = 1;
            // 
            // txtKeys
            // 
            this.txtKeys.Location = new System.Drawing.Point(3, 39);
            this.txtKeys.Multiline = true;
            this.txtKeys.Name = "txtKeys";
            this.txtKeys.Size = new System.Drawing.Size(848, 329);
            this.txtKeys.TabIndex = 1;
            this.txtKeys.Text = "Swordfish\r\nDragula\r\nJacknife";
            // 
            // chbCompression
            // 
            this.chbCompression.AutoSize = true;
            this.chbCompression.Checked = true;
            this.chbCompression.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbCompression.Location = new System.Drawing.Point(6, 3);
            this.chbCompression.Name = "chbCompression";
            this.chbCompression.Size = new System.Drawing.Size(135, 17);
            this.chbCompression.TabIndex = 0;
            this.chbCompression.Text = "Use compression (gzip)";
            this.chbCompression.UseVisualStyleBackColor = true;
            this.chbCompression.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Keys (one per line): ";
            // 
            // chbEncryption
            // 
            this.chbEncryption.AutoSize = true;
            this.chbEncryption.Location = new System.Drawing.Point(8, 6);
            this.chbEncryption.Name = "chbEncryption";
            this.chbEncryption.Size = new System.Drawing.Size(63, 17);
            this.chbEncryption.TabIndex = 0;
            this.chbEncryption.Text = "Decrypt";
            this.chbEncryption.UseVisualStyleBackColor = true;
            this.chbEncryption.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // chbNoNulls
            // 
            this.chbNoNulls.AutoSize = true;
            this.chbNoNulls.Location = new System.Drawing.Point(261, 8);
            this.chbNoNulls.Name = "chbNoNulls";
            this.chbNoNulls.Size = new System.Drawing.Size(90, 17);
            this.chbNoNulls.TabIndex = 3;
            this.chbNoNulls.Text = "Replace nulls";
            this.chbNoNulls.UseVisualStyleBackColor = true;
            this.chbNoNulls.CheckedChanged += new System.EventHandler(this.chbNoNulls_CheckedChanged);
            // 
            // DataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 431);
            this.Controls.Add(this.tabViewer);
            this.Name = "DataViewer";
            this.Text = "DataViewer";
            this.Load += new System.EventHandler(this.DataViewer_Load);
            this.tabViewer.ResumeLayout(false);
            this.tpgText.ResumeLayout(false);
            this.tpgText.PerformLayout();
            this.tpgImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabViewer;
        private System.Windows.Forms.TabPage tpgText;
        private System.Windows.Forms.TabPage tpgImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEncoding;
        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.Label lblFlags;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblMD5;
        private System.Windows.Forms.Button btnSaveFile;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chbEncryption;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtKeys;
        private System.Windows.Forms.CheckBox chbCompression;
        private System.Windows.Forms.CheckBox chbNoNulls;
    }
}