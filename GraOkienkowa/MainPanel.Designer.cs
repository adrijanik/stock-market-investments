namespace GraInwestycyjna
{
    partial class MainPanel
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
            this.historia = new System.Windows.Forms.Button();
            this.Rynek = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.portfel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // historia
            // 
            this.historia.Location = new System.Drawing.Point(56, 48);
            this.historia.Name = "historia";
            this.historia.Size = new System.Drawing.Size(88, 33);
            this.historia.TabIndex = 0;
            this.historia.Text = "Historia";
            this.historia.UseVisualStyleBackColor = true;
            // 
            // Rynek
            // 
            this.Rynek.Location = new System.Drawing.Point(56, 97);
            this.Rynek.Name = "Rynek";
            this.Rynek.Size = new System.Drawing.Size(88, 33);
            this.Rynek.TabIndex = 1;
            this.Rynek.Text = "Rynek";
            this.Rynek.UseVisualStyleBackColor = true;
            this.Rynek.Click += new System.EventHandler(this.Rynek_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(166, 16);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(376, 238);
            this.dataGridView1.TabIndex = 2;
            // 
            // portfel
            // 
            this.portfel.Location = new System.Drawing.Point(56, 145);
            this.portfel.Name = "portfel";
            this.portfel.Size = new System.Drawing.Size(88, 34);
            this.portfel.TabIndex = 3;
            this.portfel.Text = "Portfel";
            this.portfel.UseVisualStyleBackColor = true;
            this.portfel.Click += new System.EventHandler(this.portfel_Click);
            // 
            // MainPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 292);
            this.Controls.Add(this.portfel);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.Rynek);
            this.Controls.Add(this.historia);
            this.Name = "MainPanel";
            this.Text = "MainPanel";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button historia;
        private System.Windows.Forms.Button Rynek;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button portfel;
    }
}