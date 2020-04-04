namespace SampleViewer
{
    partial class Main
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.Map1Btn = new System.Windows.Forms.Button();
            this.Map2Btn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.queryBox = new System.Windows.Forms.RichTextBox();
            this.resultBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Map1Btn
            // 
            this.Map1Btn.Location = new System.Drawing.Point(13, 13);
            this.Map1Btn.Name = "Map1Btn";
            this.Map1Btn.Size = new System.Drawing.Size(108, 31);
            this.Map1Btn.TabIndex = 0;
            this.Map1Btn.Text = "Mapeamento 1";
            this.Map1Btn.UseVisualStyleBackColor = true;
            this.Map1Btn.Click += new System.EventHandler(this.Map1Btn_Click);
            // 
            // Map2Btn
            // 
            this.Map2Btn.Location = new System.Drawing.Point(127, 13);
            this.Map2Btn.Name = "Map2Btn";
            this.Map2Btn.Size = new System.Drawing.Size(108, 31);
            this.Map2Btn.TabIndex = 1;
            this.Map2Btn.Text = "Mapeamento 2";
            this.Map2Btn.UseVisualStyleBackColor = true;
            this.Map2Btn.Click += new System.EventHandler(this.Map2Btn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.queryBox);
            this.groupBox1.Location = new System.Drawing.Point(13, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(414, 632);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Código consulta";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.resultBox);
            this.groupBox2.Location = new System.Drawing.Point(433, 50);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(464, 632);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Resultado";
            // 
            // queryBox
            // 
            this.queryBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queryBox.Location = new System.Drawing.Point(3, 16);
            this.queryBox.Name = "queryBox";
            this.queryBox.Size = new System.Drawing.Size(408, 613);
            this.queryBox.TabIndex = 0;
            this.queryBox.Text = "";
            // 
            // resultBox
            // 
            this.resultBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultBox.Location = new System.Drawing.Point(3, 16);
            this.resultBox.Name = "resultBox";
            this.resultBox.Size = new System.Drawing.Size(458, 613);
            this.resultBox.TabIndex = 0;
            this.resultBox.Text = "";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 694);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Map2Btn);
            this.Controls.Add(this.Map1Btn);
            this.Name = "Main";
            this.Text = "QueryGenerator Sample Viewer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Map1Btn;
        private System.Windows.Forms.Button Map2Btn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox queryBox;
        private System.Windows.Forms.RichTextBox resultBox;
    }
}

