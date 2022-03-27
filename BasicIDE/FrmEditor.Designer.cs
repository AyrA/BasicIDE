
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
            this.TbEditor = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TbEditor
            // 
            this.TbEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbEditor.Location = new System.Drawing.Point(0, 0);
            this.TbEditor.Multiline = true;
            this.TbEditor.Name = "TbEditor";
            this.TbEditor.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbEditor.Size = new System.Drawing.Size(584, 361);
            this.TbEditor.TabIndex = 0;
            this.TbEditor.TextChanged += new System.EventHandler(this.TbEditor_TextChanged);
            // 
            // FrmEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.TbEditor);
            this.Name = "FrmEditor";
            this.Text = "Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmEditor_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmEditor_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TbEditor;
    }
}