
namespace BasicIDE
{
    partial class FrmTransfer
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
            this.OFD = new System.Windows.Forms.OpenFileDialog();
            this.BtnUploadDoc = new System.Windows.Forms.Button();
            this.BtnUploadBasic = new System.Windows.Forms.Button();
            this.BtnDownloadDoc = new System.Windows.Forms.Button();
            this.BtnDownloadBasic = new System.Windows.Forms.Button();
            this.BtnCancelClose = new System.Windows.Forms.Button();
            this.LblStatus = new System.Windows.Forms.Label();
            this.SFD = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // BtnUploadDoc
            // 
            this.BtnUploadDoc.Location = new System.Drawing.Point(12, 12);
            this.BtnUploadDoc.Name = "BtnUploadDoc";
            this.BtnUploadDoc.Size = new System.Drawing.Size(150, 23);
            this.BtnUploadDoc.TabIndex = 0;
            this.BtnUploadDoc.Text = "Send a &Document";
            this.BtnUploadDoc.UseVisualStyleBackColor = true;
            this.BtnUploadDoc.Click += new System.EventHandler(this.BtnUploadDoc_Click);
            // 
            // BtnUploadBasic
            // 
            this.BtnUploadBasic.Location = new System.Drawing.Point(12, 44);
            this.BtnUploadBasic.Name = "BtnUploadBasic";
            this.BtnUploadBasic.Size = new System.Drawing.Size(150, 23);
            this.BtnUploadBasic.TabIndex = 2;
            this.BtnUploadBasic.Text = "Send &BASIC code";
            this.BtnUploadBasic.UseVisualStyleBackColor = true;
            this.BtnUploadBasic.Click += new System.EventHandler(this.BtnUploadBasic_Click);
            // 
            // BtnDownloadDoc
            // 
            this.BtnDownloadDoc.Location = new System.Drawing.Point(170, 12);
            this.BtnDownloadDoc.Name = "BtnDownloadDoc";
            this.BtnDownloadDoc.Size = new System.Drawing.Size(150, 23);
            this.BtnDownloadDoc.TabIndex = 1;
            this.BtnDownloadDoc.Text = "Receive a D&ocument";
            this.BtnDownloadDoc.UseVisualStyleBackColor = true;
            this.BtnDownloadDoc.Click += new System.EventHandler(this.BtnDownloadDoc_Click);
            // 
            // BtnDownloadBasic
            // 
            this.BtnDownloadBasic.Location = new System.Drawing.Point(170, 44);
            this.BtnDownloadBasic.Name = "BtnDownloadBasic";
            this.BtnDownloadBasic.Size = new System.Drawing.Size(150, 23);
            this.BtnDownloadBasic.TabIndex = 3;
            this.BtnDownloadBasic.Text = "Receive B&ASIC code";
            this.BtnDownloadBasic.UseVisualStyleBackColor = true;
            this.BtnDownloadBasic.Click += new System.EventHandler(this.BtnDownloadBasic_Click);
            // 
            // BtnCancelClose
            // 
            this.BtnCancelClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancelClose.Location = new System.Drawing.Point(497, 326);
            this.BtnCancelClose.Name = "BtnCancelClose";
            this.BtnCancelClose.Size = new System.Drawing.Size(75, 23);
            this.BtnCancelClose.TabIndex = 5;
            this.BtnCancelClose.Text = "&Close";
            this.BtnCancelClose.UseVisualStyleBackColor = true;
            this.BtnCancelClose.Click += new System.EventHandler(this.BtnCancelClose_Click);
            // 
            // LblStatus
            // 
            this.LblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblStatus.AutoEllipsis = true;
            this.LblStatus.Location = new System.Drawing.Point(12, 85);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(560, 224);
            this.LblStatus.TabIndex = 4;
            this.LblStatus.Text = "Status: Idle";
            // 
            // FrmTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.LblStatus);
            this.Controls.Add(this.BtnCancelClose);
            this.Controls.Add(this.BtnDownloadBasic);
            this.Controls.Add(this.BtnDownloadDoc);
            this.Controls.Add(this.BtnUploadBasic);
            this.Controls.Add(this.BtnUploadDoc);
            this.Name = "FrmTransfer";
            this.Text = "File transfer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog OFD;
        private System.Windows.Forms.Button BtnUploadDoc;
        private System.Windows.Forms.Button BtnUploadBasic;
        private System.Windows.Forms.Button BtnDownloadDoc;
        private System.Windows.Forms.Button BtnDownloadBasic;
        private System.Windows.Forms.Button BtnCancelClose;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.SaveFileDialog SFD;
    }
}