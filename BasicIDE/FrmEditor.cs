using System;
using System.Windows.Forms;

namespace BasicIDE
{
    public partial class FrmEditor : Form
    {
        public event EventHandler CodeEdit = delegate { };

        private bool hasChange;
        private readonly string functionName;
        private readonly bool suppressEvents;

        public bool HasChange { get => hasChange; }
        public string[] Code { get => TbEditor.Lines; }

        public string FunctionName { get => functionName; }

        public FrmEditor(string Title, string[] Lines, bool Readonly)
        {
            InitializeComponent();
            suppressEvents = true;
            ApplyConfig();
            Program.ConfigUpdate += ApplyConfig;
            TbEditor.ReadOnly = Readonly;
            Text = $"Editor: {Title}";
            if (Lines != null)
            {
                TbEditor.Lines = Lines;
            }
            functionName = Title;
            hasChange = false;
            suppressEvents = false;
        }

        public void ApplyConfig()
        {
            TbEditor.Font = Program.Config.EditorFont.GetFont();
        }

        public bool Save()
        {
            if (hasChange)
            {
                if (((FrmMain)MdiParent).SaveFunction(FunctionName, TbEditor.Lines))
                {
                    hasChange = false;
                    return true;
                }
                return false;
            }
            return true;
        }

        public void SelectText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or empty.", nameof(text));
            }
            //Prefer case sensitive over insensitive
            var i = TbEditor.Text.IndexOf(text);
            if (i < 0)
            {
                i = TbEditor.Text.ToLower().IndexOf(text.ToLower());
            }
            if (i >= 0)
            {
                TbEditor.Select(i, text.Length);
            }
            TbEditor.Select();
            TbEditor.ScrollToCaret();
        }

        public TextBox GetBox()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(FrmEditor));
            }
            return TbEditor;
        }

        #region Events

        private void FrmEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hasChange)
            {
                var Btn = MessageBoxButtons.YesNoCancel;
                //Strip away the cancel button if cancelling likely causes data loss.
                switch (e.CloseReason)
                {
                    case CloseReason.TaskManagerClosing:
                    case CloseReason.WindowsShutDown:
                        Btn = MessageBoxButtons.YesNo;
                        break;
                }
                switch (MessageBox.Show(
                    "You have unsaved changes. Save them before closing?",
                    "Unsaved changes",
                    Btn,
                    MessageBoxIcon.Exclamation))
                {
                    case DialogResult.Yes:
                        e.Cancel = !Save();
                        break;
                    case DialogResult.No:
                        return;
                    default:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void TbEditor_TextChanged(object sender, EventArgs e)
        {
            if (!suppressEvents && !hasChange && !TbEditor.ReadOnly)
            {
                hasChange = true;
                CodeEdit(this, new System.EventArgs());
            }
        }

        private void FrmEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.ConfigUpdate -= ApplyConfig;
        }

        #endregion
    }
}
