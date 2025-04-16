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
            this.tbxAnalysis = new System.Windows.Forms.TextBox();
            this.tabErrors = new System.Windows.Forms.TabPage();
            this.tbxErrors = new System.Windows.Forms.TextBox();
            this.tabWarnings = new System.Windows.Forms.TabPage();
            this.tbxWarnings = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabExs.SuspendLayout();
            this.tabEx1.SuspendLayout();
            this.tabEx2.SuspendLayout();
            this.tabConsol.SuspendLayout();
            this.tabAnalysis.SuspendLayout();
            this.tabErrors.SuspendLayout();
            this.tabWarnings.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // scintilla1
            // 
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.Location = new System.Drawing.Point(3, 3);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.ScrollWidth = 5001;
            this.scintilla1.Size = new System.Drawing.Size(843, 247);
            this.scintilla1.TabIndex = 0;
            this.scintilla1.Text = resources.GetString("scintilla1.Text");
            this.scintilla1.WrapMode = ScintillaNET.WrapMode.Word;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(-1, 23);
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
            this.splitContainer1.Size = new System.Drawing.Size(859, 528);
            this.splitContainer1.SplitterDistance = 281;
            this.splitContainer1.TabIndex = 4;
            // 
            // tabExs
            // 
            this.tabExs.Controls.Add(this.tabEx1);
            this.tabExs.Controls.Add(this.tabEx2);
            this.tabExs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabExs.Location = new System.Drawing.Point(0, 0);
            this.tabExs.Name = "tabExs";
            this.tabExs.SelectedIndex = 0;
            this.tabExs.Size = new System.Drawing.Size(857, 279);
            this.tabExs.TabIndex = 5;
            // 
            // tabEx1
            // 
            this.tabEx1.Controls.Add(this.scintilla1);
            this.tabEx1.Location = new System.Drawing.Point(4, 22);
            this.tabEx1.Name = "tabEx1";
            this.tabEx1.Padding = new System.Windows.Forms.Padding(3);
            this.tabEx1.Size = new System.Drawing.Size(849, 253);
            this.tabEx1.TabIndex = 0;
            this.tabEx1.Text = "Пример №1";
            this.tabEx1.UseVisualStyleBackColor = true;
            // 
            // tabEx2
            // 
            this.tabEx2.Controls.Add(this.scintilla2);
            this.tabEx2.Location = new System.Drawing.Point(4, 22);
            this.tabEx2.Name = "tabEx2";
            this.tabEx2.Padding = new System.Windows.Forms.Padding(3);
            this.tabEx2.Size = new System.Drawing.Size(849, 253);
            this.tabEx2.TabIndex = 1;
            this.tabEx2.Text = "Пример №2";
            this.tabEx2.UseVisualStyleBackColor = true;
            // 
            // scintilla2
            // 
            this.scintilla2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla2.Location = new System.Drawing.Point(3, 3);
            this.scintilla2.Name = "scintilla2";
            this.scintilla2.ScrollWidth = 5001;
            this.scintilla2.Size = new System.Drawing.Size(843, 247);
            this.scintilla2.TabIndex = 1;
            this.scintilla2.Text = "print(\"Hello, world!\");";
            this.scintilla2.WrapMode = ScintillaNET.WrapMode.Word;
            // 
            // tabConsol
            // 
            this.tabConsol.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabConsol.Controls.Add(this.tabAnalysis);
            this.tabConsol.Controls.Add(this.tabErrors);
            this.tabConsol.Controls.Add(this.tabWarnings);
            this.tabConsol.Location = new System.Drawing.Point(0, 3);
            this.tabConsol.Name = "tabConsol";
            this.tabConsol.SelectedIndex = 0;
            this.tabConsol.Size = new System.Drawing.Size(850, 235);
            this.tabConsol.TabIndex = 0;
            // 
            // tabAnalysis
            // 
            this.tabAnalysis.Controls.Add(this.tbxAnalysis);
            this.tabAnalysis.Location = new System.Drawing.Point(4, 22);
            this.tabAnalysis.Name = "tabAnalysis";
            this.tabAnalysis.Padding = new System.Windows.Forms.Padding(3);
            this.tabAnalysis.Size = new System.Drawing.Size(842, 209);
            this.tabAnalysis.TabIndex = 0;
            this.tabAnalysis.Text = "Анализ";
            this.tabAnalysis.UseVisualStyleBackColor = true;
            // 
            // tbxAnalysis
            // 
            this.tbxAnalysis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxAnalysis.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxAnalysis.Location = new System.Drawing.Point(3, 6);
            this.tbxAnalysis.Multiline = true;
            this.tbxAnalysis.Name = "tbxAnalysis";
            this.tbxAnalysis.ReadOnly = true;
            this.tbxAnalysis.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxAnalysis.Size = new System.Drawing.Size(833, 197);
            this.tbxAnalysis.TabIndex = 0;
            this.tbxAnalysis.Text = "Здесь будет массив лексем";
            // 
            // tabErrors
            // 
            this.tabErrors.Controls.Add(this.tbxErrors);
            this.tabErrors.Location = new System.Drawing.Point(4, 22);
            this.tabErrors.Name = "tabErrors";
            this.tabErrors.Padding = new System.Windows.Forms.Padding(3);
            this.tabErrors.Size = new System.Drawing.Size(842, 209);
            this.tabErrors.TabIndex = 1;
            this.tabErrors.Text = "Ошибки";
            this.tabErrors.UseVisualStyleBackColor = true;
            // 
            // tbxErrors
            // 
            this.tbxErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxErrors.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.tbxErrors.Location = new System.Drawing.Point(4, 6);
            this.tbxErrors.Multiline = true;
            this.tbxErrors.Name = "tbxErrors";
            this.tbxErrors.ReadOnly = true;
            this.tbxErrors.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxErrors.Size = new System.Drawing.Size(832, 197);
            this.tbxErrors.TabIndex = 1;
            this.tbxErrors.Text = "Здесь будут ошибки";
            // 
            // tabWarnings
            // 
            this.tabWarnings.Controls.Add(this.tbxWarnings);
            this.tabWarnings.Location = new System.Drawing.Point(4, 22);
            this.tabWarnings.Name = "tabWarnings";
            this.tabWarnings.Padding = new System.Windows.Forms.Padding(3);
            this.tabWarnings.Size = new System.Drawing.Size(842, 209);
            this.tabWarnings.TabIndex = 2;
            this.tabWarnings.Text = "Предупреждения";
            this.tabWarnings.UseVisualStyleBackColor = true;
            // 
            // tbxWarnings
            // 
            this.tbxWarnings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxWarnings.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.tbxWarnings.Location = new System.Drawing.Point(4, 6);
            this.tbxWarnings.Multiline = true;
            this.tbxWarnings.Name = "tbxWarnings";
            this.tbxWarnings.ReadOnly = true;
            this.tbxWarnings.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxWarnings.Size = new System.Drawing.Size(833, 197);
            this.tbxWarnings.TabIndex = 2;
            this.tbxWarnings.Text = "Здесь будут предупреждения";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(858, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(858, 550);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestForm";
            this.Text = "SiMPLE";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabExs.ResumeLayout(false);
            this.tabEx1.ResumeLayout(false);
            this.tabEx2.ResumeLayout(false);
            this.tabConsol.ResumeLayout(false);
            this.tabAnalysis.ResumeLayout(false);
            this.tabAnalysis.PerformLayout();
            this.tabErrors.ResumeLayout(false);
            this.tabErrors.PerformLayout();
            this.tabWarnings.ResumeLayout(false);
            this.tabWarnings.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private ScintillaNET.Scintilla scintilla1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabExs;
        private System.Windows.Forms.TabPage tabEx1;
        private System.Windows.Forms.TabPage tabEx2;
        private ScintillaNET.Scintilla scintilla2;
        private System.Windows.Forms.TabControl tabConsol;
        private System.Windows.Forms.TabPage tabAnalysis;
        private System.Windows.Forms.TabPage tabErrors;
        private System.Windows.Forms.TextBox tbxAnalysis;
        private System.Windows.Forms.TextBox tbxErrors;
        private System.Windows.Forms.TabPage tabWarnings;
        private System.Windows.Forms.TextBox tbxWarnings;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
    }
}

