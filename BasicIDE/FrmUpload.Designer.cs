
namespace BasicIDE
{
    partial class FrmUpload
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
            this.PbStatus = new System.Windows.Forms.ProgressBar();
            this.LblStatus = new System.Windows.Forms.Label();
            this.BtnClose = new System.Windows.Forms.Button();
            this.LblTransferInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PbStatus
            // 
            this.PbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PbStatus.Location = new System.Drawing.Point(12, 69);
            this.PbStatus.Name = "PbStatus";
            this.PbStatus.Size = new System.Drawing.Size(360, 23);
            this.PbStatus.TabIndex = 0;
            // 
            // LblStatus
            // 
            this.LblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblStatus.AutoEllipsis = true;
            this.LblStatus.Location = new System.Drawing.Point(12, 18);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(360, 37);
            this.LblStatus.TabIndex = 1;
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.Location = new System.Drawing.Point(297, 132);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 3;
            this.BtnClose.Text = "&Cancel";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // LblTransferInfo
            // 
            this.LblTransferInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblTransferInfo.AutoEllipsis = true;
            this.LblTransferInfo.Location = new System.Drawing.Point(12, 99);
            this.LblTransferInfo.Name = "LblTransferInfo";
            this.LblTransferInfo.Size = new System.Drawing.Size(360, 30);
            this.LblTransferInfo.TabIndex = 4;
            this.LblTransferInfo.Text = "INFO";
            // 
            // FrmUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 167);
            this.Controls.Add(this.LblTransferInfo);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.LblStatus);
            this.Controls.Add(this.PbStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUpload";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Project Upload";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar PbStatus;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Label LblTransferInfo;
    }
}