
namespace BasicIDE
{
    partial class FrmOptions
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
            this.GbSerial = new System.Windows.Forms.GroupBox();
            this.CbPrimitiveMode = new System.Windows.Forms.CheckBox();
            this.BtnSerialDefaults = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.CbXON = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DdStopBits = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DdParity = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DdBaudrate = new System.Windows.Forms.ComboBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.GbSerial.SuspendLayout();
            this.SuspendLayout();
            // 
            // GbSerial
            // 
            this.GbSerial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GbSerial.Controls.Add(this.CbPrimitiveMode);
            this.GbSerial.Controls.Add(this.BtnSerialDefaults);
            this.GbSerial.Controls.Add(this.label4);
            this.GbSerial.Controls.Add(this.CbXON);
            this.GbSerial.Controls.Add(this.label3);
            this.GbSerial.Controls.Add(this.DdStopBits);
            this.GbSerial.Controls.Add(this.label2);
            this.GbSerial.Controls.Add(this.DdParity);
            this.GbSerial.Controls.Add(this.label1);
            this.GbSerial.Controls.Add(this.DdBaudrate);
            this.GbSerial.Location = new System.Drawing.Point(12, 12);
            this.GbSerial.Name = "GbSerial";
            this.GbSerial.Size = new System.Drawing.Size(448, 179);
            this.GbSerial.TabIndex = 0;
            this.GbSerial.TabStop = false;
            this.GbSerial.Text = "Serial Communication";
            // 
            // CbPrimitiveMode
            // 
            this.CbPrimitiveMode.AutoSize = true;
            this.CbPrimitiveMode.Location = new System.Drawing.Point(81, 143);
            this.CbPrimitiveMode.Name = "CbPrimitiveMode";
            this.CbPrimitiveMode.Size = new System.Drawing.Size(95, 17);
            this.CbPrimitiveMode.TabIndex = 8;
            this.CbPrimitiveMode.Text = "Primitive Mode";
            this.CbPrimitiveMode.UseVisualStyleBackColor = true;
            // 
            // BtnSerialDefaults
            // 
            this.BtnSerialDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSerialDefaults.Location = new System.Drawing.Point(363, 139);
            this.BtnSerialDefaults.Name = "BtnSerialDefaults";
            this.BtnSerialDefaults.Size = new System.Drawing.Size(75, 23);
            this.BtnSerialDefaults.TabIndex = 9;
            this.BtnSerialDefaults.Text = "&Defaults";
            this.BtnSerialDefaults.UseVisualStyleBackColor = true;
            this.BtnSerialDefaults.Click += new System.EventHandler(this.BtnSerialDefaults_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoEllipsis = true;
            this.label4.Location = new System.Drawing.Point(208, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(230, 33);
            this.label4.TabIndex = 2;
            this.label4.Text = "Setting rate above 300 may cause data loss.";
            // 
            // CbXON
            // 
            this.CbXON.AutoSize = true;
            this.CbXON.Location = new System.Drawing.Point(81, 120);
            this.CbXON.Name = "CbXON";
            this.CbXON.Size = new System.Drawing.Size(81, 17);
            this.CbXON.TabIndex = 7;
            this.CbXON.Text = "&XON/XOFF";
            this.CbXON.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Stop Bits";
            // 
            // DdStopBits
            // 
            this.DdStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DdStopBits.FormattingEnabled = true;
            this.DdStopBits.Location = new System.Drawing.Point(81, 88);
            this.DdStopBits.Name = "DdStopBits";
            this.DdStopBits.Size = new System.Drawing.Size(121, 21);
            this.DdStopBits.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Parity";
            // 
            // DdParity
            // 
            this.DdParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DdParity.FormattingEnabled = true;
            this.DdParity.Location = new System.Drawing.Point(81, 56);
            this.DdParity.Name = "DdParity";
            this.DdParity.Size = new System.Drawing.Size(121, 21);
            this.DdParity.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Baud Rate";
            // 
            // DdBaudrate
            // 
            this.DdBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DdBaudrate.FormattingEnabled = true;
            this.DdBaudrate.Location = new System.Drawing.Point(81, 24);
            this.DdBaudrate.Name = "DdBaudrate";
            this.DdBaudrate.Size = new System.Drawing.Size(121, 21);
            this.DdBaudrate.TabIndex = 1;
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOK.Location = new System.Drawing.Point(304, 226);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 1;
            this.BtnOK.Text = "&OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(385, 226);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "&Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // FrmOptions
            // 
            this.AcceptButton = this.BtnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(472, 261);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.GbSerial);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "FrmOptions";
            this.Text = "Options";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmOptions_FormClosed);
            this.GbSerial.ResumeLayout(false);
            this.GbSerial.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GbSerial;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox DdStopBits;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox DdParity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox DdBaudrate;
        private System.Windows.Forms.CheckBox CbXON;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnSerialDefaults;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.CheckBox CbPrimitiveMode;
    }
}