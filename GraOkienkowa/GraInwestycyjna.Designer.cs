namespace GraInwestycyjna
{
    partial class GraInwestycyjna
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraInwestycyjna));
            this.login_txt = new System.Windows.Forms.TextBox();
            this.hasło_txt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.nickname_txt = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // login_txt
            // 
            this.login_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.login_txt.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.login_txt.Location = new System.Drawing.Point(95, 102);
            this.login_txt.Name = "login_txt";
            this.login_txt.Size = new System.Drawing.Size(100, 20);
            this.login_txt.TabIndex = 0;
            this.login_txt.Text = "login";
            this.login_txt.TextChanged += new System.EventHandler(this.login_txt_TextChanged);
            // 
            // hasło_txt
            // 
            this.hasło_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.hasło_txt.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.hasło_txt.Location = new System.Drawing.Point(95, 128);
            this.hasło_txt.Name = "hasło_txt";
            this.hasło_txt.Size = new System.Drawing.Size(100, 20);
            this.hasło_txt.TabIndex = 1;
            this.hasło_txt.Text = "hasło";
            this.hasło_txt.TextChanged += new System.EventHandler(this.hasło_txt_TextChanged);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button1.Location = new System.Drawing.Point(226, 219);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(55, 31);
            this.button1.TabIndex = 2;
            this.button1.Text = "register";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // nickname_txt
            // 
            this.nickname_txt.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.nickname_txt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.nickname_txt.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.nickname_txt.Location = new System.Drawing.Point(95, 76);
            this.nickname_txt.Name = "nickname_txt";
            this.nickname_txt.Size = new System.Drawing.Size(100, 20);
            this.nickname_txt.TabIndex = 5;
            this.nickname_txt.Text = "nickname";
            this.nickname_txt.TextChanged += new System.EventHandler(this.nickname_txt_TextChanged);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.button2.Location = new System.Drawing.Point(12, 219);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(57, 31);
            this.button2.TabIndex = 7;
            this.button2.Text = "login";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.zaloguj_Click);
            // 
            // GraInwestycyjna
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.nickname_txt);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.hasło_txt);
            this.Controls.Add(this.login_txt);
            this.Name = "GraInwestycyjna";
            this.Text = "Stock Market";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox login_txt;
        private System.Windows.Forms.TextBox hasło_txt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox nickname_txt;
        private System.Windows.Forms.Button button2;
    }
}

