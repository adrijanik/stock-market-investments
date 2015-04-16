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
            this.login_txt = new System.Windows.Forms.TextBox();
            this.hasło_txt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.login_label = new System.Windows.Forms.Label();
            this.hasło_label = new System.Windows.Forms.Label();
            this.nickname_label = new System.Windows.Forms.Label();
            this.nickname_txt = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // login_txt
            // 
            this.login_txt.Location = new System.Drawing.Point(85, 92);
            this.login_txt.Name = "login_txt";
            this.login_txt.Size = new System.Drawing.Size(100, 20);
            this.login_txt.TabIndex = 0;
            this.login_txt.TextChanged += new System.EventHandler(this.login_txt_TextChanged);
            // 
            // hasło_txt
            // 
            this.hasło_txt.Location = new System.Drawing.Point(85, 130);
            this.hasło_txt.Name = "hasło_txt";
            this.hasło_txt.Size = new System.Drawing.Size(100, 20);
            this.hasło_txt.TabIndex = 1;
            this.hasło_txt.TextChanged += new System.EventHandler(this.hasło_txt_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(163, 175);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(87, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "utwórz konto";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // login_label
            // 
            this.login_label.AutoSize = true;
            this.login_label.Location = new System.Drawing.Point(23, 92);
            this.login_label.Name = "login_label";
            this.login_label.Size = new System.Drawing.Size(29, 13);
            this.login_label.TabIndex = 3;
            this.login_label.Text = "login";
            // 
            // hasło_label
            // 
            this.hasło_label.AutoSize = true;
            this.hasło_label.Location = new System.Drawing.Point(23, 137);
            this.hasło_label.Name = "hasło_label";
            this.hasło_label.Size = new System.Drawing.Size(34, 13);
            this.hasło_label.TabIndex = 4;
            this.hasło_label.Text = "hasło";
            // 
            // nickname_label
            // 
            this.nickname_label.AutoSize = true;
            this.nickname_label.Location = new System.Drawing.Point(23, 48);
            this.nickname_label.Name = "nickname_label";
            this.nickname_label.Size = new System.Drawing.Size(53, 13);
            this.nickname_label.TabIndex = 6;
            this.nickname_label.Text = "nickname";
            // 
            // nickname_txt
            // 
            this.nickname_txt.Location = new System.Drawing.Point(85, 48);
            this.nickname_txt.Name = "nickname_txt";
            this.nickname_txt.Size = new System.Drawing.Size(100, 20);
            this.nickname_txt.TabIndex = 5;
            this.nickname_txt.TextChanged += new System.EventHandler(this.nickname_txt_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(41, 175);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "zaloguj";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.zaloguj_Click);
            // 
            // GraInwestycyjna
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.nickname_label);
            this.Controls.Add(this.nickname_txt);
            this.Controls.Add(this.hasło_label);
            this.Controls.Add(this.login_label);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.hasło_txt);
            this.Controls.Add(this.login_txt);
            this.Name = "GraInwestycyjna";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox login_txt;
        private System.Windows.Forms.TextBox hasło_txt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label login_label;
        private System.Windows.Forms.Label hasło_label;
        private System.Windows.Forms.Label nickname_label;
        private System.Windows.Forms.TextBox nickname_txt;
        private System.Windows.Forms.Button button2;
    }
}

