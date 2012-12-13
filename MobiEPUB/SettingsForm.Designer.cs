namespace MobiEPUB
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textDefaultDir = new System.Windows.Forms.TextBox();
            this.comboDefaultLang = new System.Windows.Forms.ComboBox();
            this.comboDefaultEnc = new System.Windows.Forms.ComboBox();
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Default Directory";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Default Language";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Default Encoding";
            // 
            // textDefaultDir
            // 
            this.textDefaultDir.Location = new System.Drawing.Point(115, 38);
            this.textDefaultDir.Name = "textDefaultDir";
            this.textDefaultDir.Size = new System.Drawing.Size(293, 20);
            this.textDefaultDir.TabIndex = 1;
            this.textDefaultDir.TextChanged += new System.EventHandler(this.Text_Changed);
            // 
            // comboDefaultLang
            // 
            this.comboDefaultLang.FormattingEnabled = true;
            this.comboDefaultLang.Location = new System.Drawing.Point(115, 76);
            this.comboDefaultLang.Name = "comboDefaultLang";
            this.comboDefaultLang.Size = new System.Drawing.Size(293, 21);
            this.comboDefaultLang.TabIndex = 2;
            this.comboDefaultLang.TextChanged += new System.EventHandler(this.Text_Changed);
            // 
            // comboDefaultEnc
            // 
            this.comboDefaultEnc.FormattingEnabled = true;
            this.comboDefaultEnc.Location = new System.Drawing.Point(115, 124);
            this.comboDefaultEnc.Name = "comboDefaultEnc";
            this.comboDefaultEnc.Size = new System.Drawing.Size(293, 21);
            this.comboDefaultEnc.TabIndex = 3;
            this.comboDefaultEnc.TextChanged += new System.EventHandler(this.Text_Changed);
            // 
            // okBtn
            // 
            this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okBtn.Location = new System.Drawing.Point(333, 227);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 23);
            this.okBtn.TabIndex = 5;
            this.okBtn.Text = "&Ok";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(236, 227);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 4;
            this.cancelBtn.Text = "&Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(420, 262);
            this.ControlBox = false;
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.comboDefaultEnc);
            this.Controls.Add(this.comboDefaultLang);
            this.Controls.Add(this.textDefaultDir);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Settings";
            this.Text = "MobiEPUB Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textDefaultDir;
        private System.Windows.Forms.ComboBox comboDefaultLang;
        private System.Windows.Forms.ComboBox comboDefaultEnc;
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
    }
}