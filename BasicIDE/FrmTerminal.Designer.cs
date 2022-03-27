
namespace BasicIDE
{
    partial class FrmTerminal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTerminal));
            this.TbTerminal = new System.Windows.Forms.TextBox();
            this.TbSend = new System.Windows.Forms.TextBox();
            this.SerialInfoStrip = new System.Windows.Forms.StatusStrip();
            this.TsLblBrk = new System.Windows.Forms.ToolStripStatusLabel();
            this.TsLblCd = new System.Windows.Forms.ToolStripStatusLabel();
            this.TsLblCts = new System.Windows.Forms.ToolStripStatusLabel();
            this.TsLblDsr = new System.Windows.Forms.ToolStripStatusLabel();
            this.TsLblDtr = new System.Windows.Forms.ToolStripStatusLabel();
            this.TsLblRts = new System.Windows.Forms.ToolStripStatusLabel();
            this.TsLblMode = new System.Windows.Forms.ToolStripStatusLabel();
            this.TsDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.TsCrLfItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TsLfItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TsCrItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SerialInfoStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // TbTerminal
            // 
            this.TbTerminal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbTerminal.Location = new System.Drawing.Point(12, 12);
            this.TbTerminal.Multiline = true;
            this.TbTerminal.Name = "TbTerminal";
            this.TbTerminal.ReadOnly = true;
            this.TbTerminal.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbTerminal.Size = new System.Drawing.Size(560, 272);
            this.TbTerminal.TabIndex = 0;
            // 
            // TbSend
            // 
            this.TbSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbSend.Location = new System.Drawing.Point(12, 300);
            this.TbSend.Name = "TbSend";
            this.TbSend.Size = new System.Drawing.Size(560, 20);
            this.TbSend.TabIndex = 1;
            this.TbSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbSend_KeyDown);
            // 
            // SerialInfoStrip
            // 
            this.SerialInfoStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsLblBrk,
            this.TsLblCd,
            this.TsLblCts,
            this.TsLblDsr,
            this.TsLblDtr,
            this.TsLblRts,
            this.TsLblMode,
            this.TsDropDown});
            this.SerialInfoStrip.Location = new System.Drawing.Point(0, 339);
            this.SerialInfoStrip.Name = "SerialInfoStrip";
            this.SerialInfoStrip.Size = new System.Drawing.Size(584, 22);
            this.SerialInfoStrip.TabIndex = 2;
            this.SerialInfoStrip.Text = "statusStrip1";
            // 
            // TsLblBrk
            // 
            this.TsLblBrk.IsLink = true;
            this.TsLblBrk.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.TsLblBrk.Name = "TsLblBrk";
            this.TsLblBrk.Size = new System.Drawing.Size(29, 17);
            this.TsLblBrk.Text = "BRK";
            this.TsLblBrk.ToolTipText = "Break state";
            this.TsLblBrk.Click += new System.EventHandler(this.TsLblBrk_Click);
            // 
            // TsLblCd
            // 
            this.TsLblCd.Name = "TsLblCd";
            this.TsLblCd.Size = new System.Drawing.Size(22, 17);
            this.TsLblCd.Text = "CD";
            this.TsLblCd.ToolTipText = "Carrier detect";
            this.TsLblCd.Click += new System.EventHandler(this.UnusablePin_Click);
            // 
            // TsLblCts
            // 
            this.TsLblCts.Name = "TsLblCts";
            this.TsLblCts.Size = new System.Drawing.Size(28, 17);
            this.TsLblCts.Text = "CTS";
            this.TsLblCts.ToolTipText = "Clear to send";
            this.TsLblCts.Click += new System.EventHandler(this.UnusablePin_Click);
            // 
            // TsLblDsr
            // 
            this.TsLblDsr.Name = "TsLblDsr";
            this.TsLblDsr.Size = new System.Drawing.Size(30, 17);
            this.TsLblDsr.Text = "DSR";
            this.TsLblDsr.ToolTipText = "Data set ready";
            this.TsLblDsr.Click += new System.EventHandler(this.UnusablePin_Click);
            // 
            // TsLblDtr
            // 
            this.TsLblDtr.IsLink = true;
            this.TsLblDtr.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.TsLblDtr.Name = "TsLblDtr";
            this.TsLblDtr.Size = new System.Drawing.Size(30, 17);
            this.TsLblDtr.Text = "DTR";
            this.TsLblDtr.ToolTipText = "Data terminal ready";
            this.TsLblDtr.Click += new System.EventHandler(this.TsLblDtr_Click);
            // 
            // TsLblRts
            // 
            this.TsLblRts.IsLink = true;
            this.TsLblRts.Name = "TsLblRts";
            this.TsLblRts.Size = new System.Drawing.Size(29, 17);
            this.TsLblRts.Text = "RTS";
            this.TsLblRts.ToolTipText = "Request to send";
            this.TsLblRts.Click += new System.EventHandler(this.TsLblRts_Click);
            // 
            // TsLblMode
            // 
            this.TsLblMode.Name = "TsLblMode";
            this.TsLblMode.Size = new System.Drawing.Size(34, 17);
            this.TsLblMode.Text = "Mode";
            this.TsLblMode.ToolTipText = "TRS-80 port mode";
            this.TsLblMode.Click += new System.EventHandler(this.TsLblMode_Click);
            // 
            // TsDropDown
            // 
            this.TsDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TsDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsCrItem,
            this.TsLfItem,
            this.TsCrLfItem});
            this.TsDropDown.Image = ((System.Drawing.Image)(resources.GetObject("TsDropDown.Image")));
            this.TsDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TsDropDown.Name = "TsDropDown";
            this.TsDropDown.Size = new System.Drawing.Size(137, 20);
            this.TsDropDown.Text = "Send Line Ending: CRLF";
            // 
            // TsCrLfItem
            // 
            this.TsCrLfItem.Name = "TsCrLfItem";
            this.TsCrLfItem.Size = new System.Drawing.Size(180, 22);
            this.TsCrLfItem.Tag = "CRLF";
            this.TsCrLfItem.Text = "CRLF";
            this.TsCrLfItem.Click += new System.EventHandler(this.ChangeLineEnding_Click);
            // 
            // TsLfItem
            // 
            this.TsLfItem.Name = "TsLfItem";
            this.TsLfItem.Size = new System.Drawing.Size(180, 22);
            this.TsLfItem.Tag = "LF";
            this.TsLfItem.Text = "LF";
            this.TsLfItem.Click += new System.EventHandler(this.ChangeLineEnding_Click);
            // 
            // TsCrItem
            // 
            this.TsCrItem.Name = "TsCrItem";
            this.TsCrItem.Size = new System.Drawing.Size(180, 22);
            this.TsCrItem.Tag = "CR";
            this.TsCrItem.Text = "CR";
            this.TsCrItem.Click += new System.EventHandler(this.ChangeLineEnding_Click);
            // 
            // FrmTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.SerialInfoStrip);
            this.Controls.Add(this.TbSend);
            this.Controls.Add(this.TbTerminal);
            this.Name = "FrmTerminal";
            this.Text = "Serial Terminal";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmTerminal_FormClosed);
            this.Load += new System.EventHandler(this.FrmTerminal_Load);
            this.SerialInfoStrip.ResumeLayout(false);
            this.SerialInfoStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TbTerminal;
        private System.Windows.Forms.TextBox TbSend;
        private System.Windows.Forms.StatusStrip SerialInfoStrip;
        private System.Windows.Forms.ToolStripStatusLabel TsLblBrk;
        private System.Windows.Forms.ToolStripStatusLabel TsLblCd;
        private System.Windows.Forms.ToolStripStatusLabel TsLblCts;
        private System.Windows.Forms.ToolStripStatusLabel TsLblDsr;
        private System.Windows.Forms.ToolStripStatusLabel TsLblDtr;
        private System.Windows.Forms.ToolStripStatusLabel TsLblRts;
        private System.Windows.Forms.ToolStripStatusLabel TsLblMode;
        private System.Windows.Forms.ToolStripDropDownButton TsDropDown;
        private System.Windows.Forms.ToolStripMenuItem TsCrItem;
        private System.Windows.Forms.ToolStripMenuItem TsLfItem;
        private System.Windows.Forms.ToolStripMenuItem TsCrLfItem;
    }
}