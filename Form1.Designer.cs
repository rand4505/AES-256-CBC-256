namespace AESEncryptAndDecrypt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.RTB = new System.Windows.Forms.RichTextBox();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.InputLabel = new System.Windows.Forms.Label();
            this.OutPutLabel = new System.Windows.Forms.Label();
            this.EncryptButton = new System.Windows.Forms.RadioButton();
            this.DecryptButton = new System.Windows.Forms.RadioButton();
            this.KeyArrayBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.VectorArrayBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.GoGoGadget = new System.Windows.Forms.Button();
            this.UpDateKeyAr = new System.Windows.Forms.Button();
            this.VectorUpdateBtn = new System.Windows.Forms.Button();
            this.randarrays = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FileMode_checkBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SeedBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.AlgoBox = new System.Windows.Forms.ComboBox();
            this.BCryptBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RTB
            // 
            this.RTB.BackColor = System.Drawing.SystemColors.WindowText;
            this.RTB.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RTB.ForeColor = System.Drawing.SystemColors.Menu;
            this.RTB.Location = new System.Drawing.Point(5, 181);
            this.RTB.Name = "RTB";
            this.RTB.ReadOnly = true;
            this.RTB.Size = new System.Drawing.Size(1010, 259);
            this.RTB.TabIndex = 4;
            this.RTB.Text = "";
            // 
            // InputBox
            // 
            this.InputBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InputBox.Location = new System.Drawing.Point(201, 23);
            this.InputBox.Multiline = true;
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(807, 69);
            this.InputBox.TabIndex = 3;
            // 
            // InputLabel
            // 
            this.InputLabel.AutoSize = true;
            this.InputLabel.Location = new System.Drawing.Point(198, 7);
            this.InputLabel.Name = "InputLabel";
            this.InputLabel.Size = new System.Drawing.Size(154, 13);
            this.InputLabel.TabIndex = 2;
            this.InputLabel.Text = "To Be Encrypted or Decrypted:";
            // 
            // OutPutLabel
            // 
            this.OutPutLabel.AutoSize = true;
            this.OutPutLabel.Location = new System.Drawing.Point(6, 79);
            this.OutPutLabel.Name = "OutPutLabel";
            this.OutPutLabel.Size = new System.Drawing.Size(40, 13);
            this.OutPutLabel.TabIndex = 3;
            this.OutPutLabel.Text = "Result:";
            // 
            // EncryptButton
            // 
            this.EncryptButton.AutoSize = true;
            this.EncryptButton.Location = new System.Drawing.Point(9, 35);
            this.EncryptButton.Name = "EncryptButton";
            this.EncryptButton.Size = new System.Drawing.Size(61, 17);
            this.EncryptButton.TabIndex = 1;
            this.EncryptButton.TabStop = true;
            this.EncryptButton.Text = "Encrypt";
            this.EncryptButton.UseVisualStyleBackColor = true;
            this.EncryptButton.CheckedChanged += new System.EventHandler(this.EncryptButton_CheckedChanged);
            // 
            // DecryptButton
            // 
            this.DecryptButton.AutoSize = true;
            this.DecryptButton.Location = new System.Drawing.Point(100, 35);
            this.DecryptButton.Name = "DecryptButton";
            this.DecryptButton.Size = new System.Drawing.Size(62, 17);
            this.DecryptButton.TabIndex = 2;
            this.DecryptButton.TabStop = true;
            this.DecryptButton.Text = "Decrypt";
            this.DecryptButton.UseVisualStyleBackColor = true;
            this.DecryptButton.CheckedChanged += new System.EventHandler(this.DecryptButton_CheckedChanged);
            // 
            // KeyArrayBox
            // 
            this.KeyArrayBox.Location = new System.Drawing.Point(90, 5);
            this.KeyArrayBox.Name = "KeyArrayBox";
            this.KeyArrayBox.Size = new System.Drawing.Size(838, 20);
            this.KeyArrayBox.TabIndex = 6;
            this.KeyArrayBox.TextChanged += new System.EventHandler(this.KeyArrayChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "KeyArrayVals";
            // 
            // VectorArrayBox
            // 
            this.VectorArrayBox.Location = new System.Drawing.Point(90, 34);
            this.VectorArrayBox.Name = "VectorArrayBox";
            this.VectorArrayBox.Size = new System.Drawing.Size(610, 20);
            this.VectorArrayBox.TabIndex = 7;
            this.VectorArrayBox.TextChanged += new System.EventHandler(this.VectorArrayChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "VectorArrayVals";
            // 
            // GoGoGadget
            // 
            this.GoGoGadget.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.GoGoGadget.Location = new System.Drawing.Point(69, 58);
            this.GoGoGadget.Name = "GoGoGadget";
            this.GoGoGadget.Size = new System.Drawing.Size(126, 31);
            this.GoGoGadget.TabIndex = 5;
            this.GoGoGadget.Text = "Execute!";
            this.GoGoGadget.UseVisualStyleBackColor = true;
            this.GoGoGadget.Click += new System.EventHandler(this.GoGoGadget_Click);
            // 
            // UpDateKeyAr
            // 
            this.UpDateKeyAr.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.UpDateKeyAr.Location = new System.Drawing.Point(931, 3);
            this.UpDateKeyAr.Name = "UpDateKeyAr";
            this.UpDateKeyAr.Size = new System.Drawing.Size(75, 23);
            this.UpDateKeyAr.TabIndex = 8;
            this.UpDateKeyAr.Text = "Update Key";
            this.UpDateKeyAr.UseVisualStyleBackColor = true;
            this.UpDateKeyAr.Click += new System.EventHandler(this.UpDateKeyAr_Click);
            // 
            // VectorUpdateBtn
            // 
            this.VectorUpdateBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.VectorUpdateBtn.Location = new System.Drawing.Point(706, 31);
            this.VectorUpdateBtn.Name = "VectorUpdateBtn";
            this.VectorUpdateBtn.Size = new System.Drawing.Size(100, 23);
            this.VectorUpdateBtn.TabIndex = 9;
            this.VectorUpdateBtn.Text = "Update Vector";
            this.VectorUpdateBtn.UseVisualStyleBackColor = true;
            this.VectorUpdateBtn.Click += new System.EventHandler(this.VectorUpdateBtn_Click);
            // 
            // randarrays
            // 
            this.randarrays.Location = new System.Drawing.Point(812, 31);
            this.randarrays.Name = "randarrays";
            this.randarrays.Size = new System.Drawing.Size(75, 23);
            this.randarrays.TabIndex = 10;
            this.randarrays.Text = "PRNG []\'s";
            this.randarrays.UseVisualStyleBackColor = true;
            this.randarrays.Click += new System.EventHandler(this.randarrays_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FileMode_checkBox);
            this.groupBox1.Controls.Add(this.GoGoGadget);
            this.groupBox1.Controls.Add(this.OutPutLabel);
            this.groupBox1.Controls.Add(this.DecryptButton);
            this.groupBox1.Controls.Add(this.EncryptButton);
            this.groupBox1.Controls.Add(this.InputBox);
            this.groupBox1.Controls.Add(this.InputLabel);
            this.groupBox1.Location = new System.Drawing.Point(5, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1010, 95);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Message Controls";
            // 
            // FileMode_checkBox
            // 
            this.FileMode_checkBox.AutoSize = true;
            this.FileMode_checkBox.Location = new System.Drawing.Point(9, 12);
            this.FileMode_checkBox.Name = "FileMode_checkBox";
            this.FileMode_checkBox.Size = new System.Drawing.Size(69, 17);
            this.FileMode_checkBox.TabIndex = 6;
            this.FileMode_checkBox.Text = "FileMode";
            this.FileMode_checkBox.UseVisualStyleBackColor = true;
            this.FileMode_checkBox.CheckedChanged += new System.EventHandler(this.FileMode_checkBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(893, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Randomize Array Values";
            // 
            // SeedBox
            // 
            this.SeedBox.Location = new System.Drawing.Point(90, 61);
            this.SeedBox.Name = "SeedBox";
            this.SeedBox.Size = new System.Drawing.Size(199, 20);
            this.SeedBox.TabIndex = 16;
            this.SeedBox.TextChanged += new System.EventHandler(this.SeedBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Integer Seed";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(322, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Encryption Algorithm";
            // 
            // AlgoBox
            // 
            this.AlgoBox.FormattingEnabled = true;
            this.AlgoBox.Items.AddRange(new object[] {
            "256-AES-256-CBC",
            "256-AES-128-CBC(Nist Rijndael)",
            "256-AES-128-CFB",
            "128-AES-128-CBC"});
            this.AlgoBox.Location = new System.Drawing.Point(432, 62);
            this.AlgoBox.Name = "AlgoBox";
            this.AlgoBox.Size = new System.Drawing.Size(195, 21);
            this.AlgoBox.TabIndex = 19;
            this.AlgoBox.SelectedIndexChanged += new System.EventHandler(this.AlgoBox_SelectedIndexChanged);
            // 
            // BCryptBox1
            // 
            this.BCryptBox1.AutoSize = true;
            this.BCryptBox1.Location = new System.Drawing.Point(646, 64);
            this.BCryptBox1.Name = "BCryptBox1";
            this.BCryptBox1.Size = new System.Drawing.Size(254, 17);
            this.BCryptBox1.TabIndex = 21;
            this.BCryptBox1.Text = "Use Bcrypt Hash instead of random key/vector?";
            this.BCryptBox1.UseVisualStyleBackColor = true;
            this.BCryptBox1.CheckedChanged += new System.EventHandler(this.BCryptBox1_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 442);
            this.Controls.Add(this.BCryptBox1);
            this.Controls.Add(this.AlgoBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SeedBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.randarrays);
            this.Controls.Add(this.VectorUpdateBtn);
            this.Controls.Add(this.UpDateKeyAr);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.VectorArrayBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.KeyArrayBox);
            this.Controls.Add(this.RTB);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = ".Net Crypto by John L. Grubbs";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.Label InputLabel;
        private System.Windows.Forms.Label OutPutLabel;
        private System.Windows.Forms.RadioButton EncryptButton;
        private System.Windows.Forms.RadioButton DecryptButton;
        private System.Windows.Forms.TextBox KeyArrayBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox VectorArrayBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button GoGoGadget;
        private System.Windows.Forms.Button UpDateKeyAr;
        private System.Windows.Forms.Button VectorUpdateBtn;
        private System.Windows.Forms.Button randarrays;
        public System.Windows.Forms.RichTextBox RTB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox FileMode_checkBox;
        private System.Windows.Forms.TextBox SeedBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox AlgoBox;
        private System.Windows.Forms.CheckBox BCryptBox1;
    }
}
