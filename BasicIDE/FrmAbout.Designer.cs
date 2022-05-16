
namespace BasicIDE
{
    public partial class FrmAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            this.TablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.Logo = new System.Windows.Forms.PictureBox();
            this.LblProductName = new System.Windows.Forms.Label();
            this.LblVersion = new System.Windows.Forms.Label();
            this.LblCopyright = new System.Windows.Forms.Label();
            this.LblCompanyName = new System.Windows.Forms.Label();
            this.TbDescription = new System.Windows.Forms.TextBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.TablePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // TablePanel
            // 
            this.TablePanel.ColumnCount = 2;
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.TablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67F));
            this.TablePanel.Controls.Add(this.Logo, 0, 0);
            this.TablePanel.Controls.Add(this.LblProductName, 1, 0);
            this.TablePanel.Controls.Add(this.LblVersion, 1, 1);
            this.TablePanel.Controls.Add(this.LblCopyright, 1, 2);
            this.TablePanel.Controls.Add(this.LblCompanyName, 1, 3);
            this.TablePanel.Controls.Add(this.TbDescription, 1, 4);
            this.TablePanel.Controls.Add(this.BtnOk, 1, 5);
            this.TablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TablePanel.Location = new System.Drawing.Point(9, 9);
            this.TablePanel.Name = "TablePanel";
            this.TablePanel.RowCount = 6;
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.TablePanel.Size = new System.Drawing.Size(417, 265);
            this.TablePanel.TabIndex = 0;
            // 
            // Logo
            // 
            this.Logo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Logo.Image = ((System.Drawing.Image)(resources.GetObject("Logo.Image")));
            this.Logo.Location = new System.Drawing.Point(3, 3);
            this.Logo.Name = "Logo";
            this.TablePanel.SetRowSpan(this.Logo, 6);
            this.Logo.Size = new System.Drawing.Size(131, 259);
            this.Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Logo.TabIndex = 12;
            this.Logo.TabStop = false;
            // 
            // LblProductName
            // 
            this.LblProductName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblProductName.Location = new System.Drawing.Point(143, 0);
            this.LblProductName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.LblProductName.MaximumSize = new System.Drawing.Size(0, 17);
            this.LblProductName.Name = "LblProductName";
            this.LblProductName.Size = new System.Drawing.Size(271, 17);
            this.LblProductName.TabIndex = 19;
            this.LblProductName.Text = "Product Name";
            this.LblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblVersion
            // 
            this.LblVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblVersion.Location = new System.Drawing.Point(143, 26);
            this.LblVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.LblVersion.MaximumSize = new System.Drawing.Size(0, 17);
            this.LblVersion.Name = "LblVersion";
            this.LblVersion.Size = new System.Drawing.Size(271, 17);
            this.LblVersion.TabIndex = 0;
            this.LblVersion.Text = "Version";
            this.LblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblCopyright
            // 
            this.LblCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblCopyright.Location = new System.Drawing.Point(143, 52);
            this.LblCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.LblCopyright.MaximumSize = new System.Drawing.Size(0, 17);
            this.LblCopyright.Name = "LblCopyright";
            this.LblCopyright.Size = new System.Drawing.Size(271, 17);
            this.LblCopyright.TabIndex = 21;
            this.LblCopyright.Text = "Copyright";
            this.LblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LblCompanyName
            // 
            this.LblCompanyName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LblCompanyName.Location = new System.Drawing.Point(143, 78);
            this.LblCompanyName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.LblCompanyName.MaximumSize = new System.Drawing.Size(0, 17);
            this.LblCompanyName.Name = "LblCompanyName";
            this.LblCompanyName.Size = new System.Drawing.Size(271, 17);
            this.LblCompanyName.TabIndex = 22;
            this.LblCompanyName.Text = "Company Name";
            this.LblCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TbDescription
            // 
            this.TbDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbDescription.Location = new System.Drawing.Point(143, 107);
            this.TbDescription.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.TbDescription.Multiline = true;
            this.TbDescription.Name = "TbDescription";
            this.TbDescription.ReadOnly = true;
            this.TbDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbDescription.Size = new System.Drawing.Size(271, 126);
            this.TbDescription.TabIndex = 23;
            this.TbDescription.TabStop = false;
            this.TbDescription.Text = "Description";
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnOk.Location = new System.Drawing.Point(339, 239);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 24;
            this.BtnOk.Text = "&OK";
            // 
            // FrmAbout
            // 
            this.AcceptButton = this.BtnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 283);
            this.Controls.Add(this.TablePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.TablePanel.ResumeLayout(false);
            this.TablePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TablePanel;
        private System.Windows.Forms.PictureBox Logo;
        private System.Windows.Forms.Label LblProductName;
        private System.Windows.Forms.Label LblVersion;
        private System.Windows.Forms.Label LblCopyright;
        private System.Windows.Forms.Label LblCompanyName;
        private System.Windows.Forms.TextBox TbDescription;
        private System.Windows.Forms.Button BtnOk;
    }
}
