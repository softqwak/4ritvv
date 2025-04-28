namespace Demo
{
    partial class TestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabExs = new System.Windows.Forms.TabControl();
            this.tabEx1 = new System.Windows.Forms.TabPage();
            this.tabEx2 = new System.Windows.Forms.TabPage();
            this.scintilla2 = new ScintillaNET.Scintilla();
            this.tabConsol = new System.Windows.Forms.TabControl();
            this.tabAnalysis = new System.Windows.Forms.TabPage();
            this.rtbxAnalysis = new System.Windows.Forms.RichTextBox();
            this.tabOutput = new System.Windows.Forms.TabPage();
            this.rtbxOutput = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabExs.SuspendLayout();
            this.tabEx1.SuspendLayout();
            this.tabEx2.SuspendLayout();
            this.tabConsol.SuspendLayout();
            this.tabAnalysis.SuspendLayout();
            this.tabOutput.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // scintilla1
            // 
            this.scintilla1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Location = new System.Drawing.Point(3, 3);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.ScrollWidth = 5001;
            this.scintilla1.Size = new System.Drawing.Size(935, 208);
            this.scintilla1.TabIndex = 0;
            this.scintilla1.Text = resources.GetString("scintilla1.Text");
            this.scintilla1.WrapMode = ScintillaNET.WrapMode.Word;
            this.scintilla1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.scintilla1_KeyDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 28);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabExs);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabConsol);
            this.splitContainer1.Size = new System.Drawing.Size(949, 622);
            this.splitContainer1.SplitterDistance = 355;
            this.splitContainer1.TabIndex = 4;
            // 
            // tabExs
            // 
            this.tabExs.Controls.Add(this.tabEx1);
            this.tabExs.Controls.Add(this.tabEx2);
            this.tabExs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabExs.HotTrack = true;
            this.tabExs.Location = new System.Drawing.Point(0, 0);
            this.tabExs.Name = "tabExs";
            this.tabExs.SelectedIndex = 0;
            this.tabExs.Size = new System.Drawing.Size(949, 355);
            this.tabExs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabExs.TabIndex = 5;
            // 
            // tabEx1
            // 
            this.tabEx1.BackColor = System.Drawing.SystemColors.Window;
            this.tabEx1.Controls.Add(this.scintilla1);
            this.tabEx1.Location = new System.Drawing.Point(4, 22);
            this.tabEx1.Name = "tabEx1";
            this.tabEx1.Padding = new System.Windows.Forms.Padding(3);
            this.tabEx1.Size = new System.Drawing.Size(941, 214);
            this.tabEx1.TabIndex = 0;
            this.tabEx1.Text = "Пример №1";
            // 
            // tabEx2
            // 
            this.tabEx2.BackColor = System.Drawing.SystemColors.Window;
            this.tabEx2.Controls.Add(this.scintilla2);
            this.tabEx2.Location = new System.Drawing.Point(4, 22);
            this.tabEx2.Name = "tabEx2";
            this.tabEx2.Padding = new System.Windows.Forms.Padding(3);
            this.tabEx2.Size = new System.Drawing.Size(941, 329);
            this.tabEx2.TabIndex = 1;
            this.tabEx2.Text = "Пример №2";
            // 
            // scintilla2
            // 
            this.scintilla2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.scintilla2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla2.Location = new System.Drawing.Point(3, 3);
            this.scintilla2.Name = "scintilla2";
            this.scintilla2.ScrollWidth = 5001;
            this.scintilla2.Size = new System.Drawing.Size(935, 323);
            this.scintilla2.TabIndex = 1;
            this.scintilla2.Text = resources.GetString("scintilla2.Text");
            this.scintilla2.WrapMode = ScintillaNET.WrapMode.Word;
            // 
            // tabConsol
            // 
            this.tabConsol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabConsol.Controls.Add(this.tabAnalysis);
            this.tabConsol.Controls.Add(this.tabOutput);
            this.tabConsol.Location = new System.Drawing.Point(0, 3);
            this.tabConsol.Name = "tabConsol";
            this.tabConsol.SelectedIndex = 0;
            this.tabConsol.Size = new System.Drawing.Size(949, 257);
            this.tabConsol.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabConsol.TabIndex = 0;
            // 
            // tabAnalysis
            // 
            this.tabAnalysis.BackColor = System.Drawing.SystemColors.Window;
            this.tabAnalysis.Controls.Add(this.rtbxAnalysis);
            this.tabAnalysis.Location = new System.Drawing.Point(4, 22);
            this.tabAnalysis.Name = "tabAnalysis";
            this.tabAnalysis.Padding = new System.Windows.Forms.Padding(3);
            this.tabAnalysis.Size = new System.Drawing.Size(941, 231);
            this.tabAnalysis.TabIndex = 0;
            this.tabAnalysis.Text = "Анализ";
            // 
            // rtbxAnalysis
            // 
            this.rtbxAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbxAnalysis.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbxAnalysis.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbxAnalysis.Location = new System.Drawing.Point(6, 4);
            this.rtbxAnalysis.Name = "rtbxAnalysis";
            this.rtbxAnalysis.Size = new System.Drawing.Size(932, 224);
            this.rtbxAnalysis.TabIndex = 0;
            this.rtbxAnalysis.Text = "";
            // 
            // tabOutput
            // 
            this.tabOutput.BackColor = System.Drawing.SystemColors.Window;
            this.tabOutput.Controls.Add(this.rtbxOutput);
            this.tabOutput.Location = new System.Drawing.Point(4, 22);
            this.tabOutput.Name = "tabOutput";
            this.tabOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tabOutput.Size = new System.Drawing.Size(941, 346);
            this.tabOutput.TabIndex = 3;
            this.tabOutput.Text = "Вывод";
            // 
            // rtbxOutput
            // 
            this.rtbxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbxOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbxOutput.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbxOutput.Location = new System.Drawing.Point(6, 4);
            this.rtbxOutput.Name = "rtbxOutput";
            this.rtbxOutput.Size = new System.Drawing.Size(929, 336);
            this.rtbxOutput.TabIndex = 0;
            this.rtbxOutput.Text = "";
            this.rtbxOutput.TextChanged += new System.EventHandler(this.rtbxOutput_TextChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(10, 1);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(976, 24);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 21);
            this.toolStripButton1.Text = "Выполнить";
            this.toolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(62, 21);
            this.toolStripLabel1.Text = "Запустить";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(947, 650);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer1);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestForm";
            this.Text = "SiMPLE";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TestForm_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabExs.ResumeLayout(false);
            this.tabEx1.ResumeLayout(false);
            this.tabEx2.ResumeLayout(false);
            this.tabConsol.ResumeLayout(false);
            this.tabAnalysis.ResumeLayout(false);
            this.tabOutput.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private ScintillaNET.Scintilla scintilla1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabExs;
        private System.Windows.Forms.TabPage tabEx1;
        private System.Windows.Forms.TabPage tabEx2;
        private ScintillaNET.Scintilla scintilla2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.TabControl tabConsol;
        private System.Windows.Forms.TabPage tabAnalysis;
        private System.Windows.Forms.TabPage tabOutput;
        private System.Windows.Forms.RichTextBox rtbxOutput;
        private System.Windows.Forms.RichTextBox rtbxAnalysis;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    }
}

