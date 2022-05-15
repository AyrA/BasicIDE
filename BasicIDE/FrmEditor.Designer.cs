
namespace BasicIDE
{
    partial class FrmEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEditor));
            this.TbCode = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.TbCode)).BeginInit();
            this.SuspendLayout();
            // 
            // TbCode
            // 
            this.TbCode.AllowMacroRecording = false;
            this.TbCode.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.TbCode.AutoIndent = false;
            this.TbCode.AutoIndentChars = false;
            this.TbCode.AutoIndentExistingLines = false;
            this.TbCode.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.TbCode.BackBrush = null;
            this.TbCode.CharHeight = 14;
            this.TbCode.CharWidth = 8;
            this.TbCode.CommentPrefix = "\'";
            this.TbCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TbCode.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.TbCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbCode.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.TbCode.IsReplaceMode = false;
            this.TbCode.Location = new System.Drawing.Point(0, 0);
            this.TbCode.Name = "TbCode";
            this.TbCode.Paddings = new System.Windows.Forms.Padding(0);
            this.TbCode.PreferredLineWidth = 254;
            this.TbCode.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.TbCode.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("TbCode.ServiceColors")));
            this.TbCode.Size = new System.Drawing.Size(584, 361);
            this.TbCode.TabIndex = 1;
            this.TbCode.Zoom = 100;
            this.TbCode.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.TbCode_TextChanged);
            // 
            // FrmEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.TbCode);
            this.Name = "FrmEditor";
            this.Text = "Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmEditor_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmEditor_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.TbCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private FastColoredTextBoxNS.FastColoredTextBox TbCode;
    }
}