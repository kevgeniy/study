namespace textCut
{
    partial class Form1
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
            this.LengthT = new System.Windows.Forms.TextBox();
            this.LengthL = new System.Windows.Forms.Label();
            this.Open = new System.Windows.Forms.Button();
            this.SaveB = new System.Windows.Forms.Button();
            this.FormatB = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Eng = new System.Windows.Forms.RadioButton();
            this.Ru = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LengthT
            // 
            this.LengthT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LengthT.Location = new System.Drawing.Point(124, 15);
            this.LengthT.Name = "LengthT";
            this.LengthT.Size = new System.Drawing.Size(100, 20);
            this.LengthT.TabIndex = 1;
            this.LengthT.TextChanged += new System.EventHandler(this.Length_TextChanged);
            this.LengthT.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LengthT_KeyPress);
            // 
            // LengthL
            // 
            this.LengthL.AutoSize = true;
            this.LengthL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.LengthL.Location = new System.Drawing.Point(12, 12);
            this.LengthL.Name = "LengthL";
            this.LengthL.Size = new System.Drawing.Size(98, 20);
            this.LengthL.TabIndex = 2;
            this.LengthL.Text = "New Length:";
            // 
            // Open
            // 
            this.Open.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.Open.Location = new System.Drawing.Point(12, 72);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(80, 34);
            this.Open.TabIndex = 3;
            this.Open.Text = "Open File";
            this.Open.UseVisualStyleBackColor = true;
            this.Open.Click += new System.EventHandler(this.Open_Click);
            // 
            // SaveB
            // 
            this.SaveB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.SaveB.Location = new System.Drawing.Point(98, 72);
            this.SaveB.Name = "SaveB";
            this.SaveB.Size = new System.Drawing.Size(80, 34);
            this.SaveB.TabIndex = 4;
            this.SaveB.Text = "Save in";
            this.SaveB.UseVisualStyleBackColor = true;
            this.SaveB.Click += new System.EventHandler(this.Save_Click);
            // 
            // FormatB
            // 
            this.FormatB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.FormatB.Location = new System.Drawing.Point(184, 72);
            this.FormatB.Name = "FormatB";
            this.FormatB.Size = new System.Drawing.Size(80, 34);
            this.FormatB.TabIndex = 5;
            this.FormatB.Text = "Format";
            this.FormatB.UseVisualStyleBackColor = true;
            this.FormatB.Click += new System.EventHandler(this.FormatB_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.CausesValidation = false;
            this.groupBox1.Controls.Add(this.Ru);
            this.groupBox1.Controls.Add(this.Eng);
            this.groupBox1.Location = new System.Drawing.Point(27, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(218, 31);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // Eng
            // 
            this.Eng.AutoSize = true;
            this.Eng.Location = new System.Drawing.Point(6, 8);
            this.Eng.Name = "Eng";
            this.Eng.Size = new System.Drawing.Size(59, 17);
            this.Eng.TabIndex = 8;
            this.Eng.TabStop = true;
            this.Eng.Text = "English";
            this.Eng.UseVisualStyleBackColor = true;
            this.Eng.CheckedChanged += new System.EventHandler(this.Eng_CheckedChanged);
            // 
            // Ru
            // 
            this.Ru.AutoSize = true;
            this.Ru.Location = new System.Drawing.Point(145, 8);
            this.Ru.Name = "Ru";
            this.Ru.Size = new System.Drawing.Size(67, 17);
            this.Ru.TabIndex = 9;
            this.Ru.TabStop = true;
            this.Ru.Text = "Русский";
            this.Ru.UseVisualStyleBackColor = true;
            this.Ru.CheckedChanged += new System.EventHandler(this.Rus_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 112);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.FormatB);
            this.Controls.Add(this.SaveB);
            this.Controls.Add(this.Open);
            this.Controls.Add(this.LengthL);
            this.Controls.Add(this.LengthT);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Formatter";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LengthT;
        private System.Windows.Forms.Label LengthL;
        private System.Windows.Forms.Button Open;
        private System.Windows.Forms.Button SaveB;
        private System.Windows.Forms.Button FormatB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton Ru;
        private System.Windows.Forms.RadioButton Eng;
    }
}

