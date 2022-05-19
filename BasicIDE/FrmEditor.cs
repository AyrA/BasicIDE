using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BasicIDE
{
    public partial class FrmEditor : Form
    {
        private struct Styles
        {
            public static readonly FastColoredTextBoxNS.TextStyle Comment = new FastColoredTextBoxNS.TextStyle(Brushes.Green, Brushes.Transparent, FontStyle.Regular);
            public static readonly FastColoredTextBoxNS.TextStyle Keyword = new FastColoredTextBoxNS.TextStyle(Brushes.Blue, Brushes.Transparent, FontStyle.Bold);
            public static readonly FastColoredTextBoxNS.TextStyle String = new FastColoredTextBoxNS.TextStyle(Brushes.Brown, Brushes.Transparent, FontStyle.Regular);
            public static readonly FastColoredTextBoxNS.TextStyle Variable = new FastColoredTextBoxNS.TextStyle(Brushes.Red, Brushes.Transparent, FontStyle.Regular);
            public static readonly FastColoredTextBoxNS.TextStyle Label = new FastColoredTextBoxNS.TextStyle(Brushes.Black, Brushes.Transparent, FontStyle.Bold);
            public static readonly FastColoredTextBoxNS.TextStyle LineNumber = new FastColoredTextBoxNS.TextStyle(Brushes.White, Brushes.Black, FontStyle.Bold);
        }

        private static readonly string KeywordRegex = string.Join("|", Basic.Compiler.Instructions.Select(Regex.Escape));

        public event EventHandler CodeEdit = delegate { };

        private bool hasChange;
        private string functionName;
        private readonly bool suppressEvents;

        public bool HasChange { get => hasChange; }
        public string[] Code { get => GetLines(); }

        public string FunctionName { get => functionName; }

        public FrmEditor(string Title, string[] Lines, bool Readonly)
        {
            InitializeComponent();
            suppressEvents = true;
            ApplyConfig();
            Program.ConfigUpdate += ApplyConfig;
            TbCode.ReadOnly = Readonly;
            Text = $"Editor: {Title}";
            if (Lines != null)
            {
                TbCode.Text = string.Join(Environment.NewLine, Lines);
            }
            functionName = Title;
            hasChange = false;
            suppressEvents = false;
        }

        public void SetFunctionName(string Name)
        {
            functionName = Name;
            Text = $"Editor: {Name}";
        }

        public void ApplyConfig()
        {
            TbCode.Font = Program.Config.EditorFont.GetFont();
        }

        public bool Save()
        {
            if (hasChange)
            {
                if (((FrmMain)MdiParent).SaveFunction(FunctionName, GetLines()))
                {
                    hasChange = false;
                    return true;
                }
                return false;
            }
            return true;
        }

        public void SelectLine(int LineNumber)
        {
            TbCode.SetSelectedLine(LineNumber);
        }

        public void SelectText(string Text)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }
            var Pos = TbCode.Text.ToLower().IndexOf(Text.ToLower());
            if (Pos >= 0)
            {
                TbCode.SelectionStart = Pos;
                TbCode.SelectionLength = Text.Length;
                BringToFront();
                TbCode.Select();
                TbCode.DoCaretVisible();
            }
        }

        private string[] GetLines()
        {
            var L = new List<string>(TbCode.Lines);
            return L.ToArray();
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

        private void FrmEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.ConfigUpdate -= ApplyConfig;
        }

        private void TbCode_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle();
            e.ChangedRange.SetStyle(Styles.String, "\"[^\"]+\"");
            e.ChangedRange.SetStyle(Styles.Comment, @"('|REM)[^:\r\n]*", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(Styles.Keyword, "(" + KeywordRegex + "|RETURN|ARG)", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(Styles.Variable, @"\w+[" + Regex.Escape(Basic.Compiler.Types) + "]");
            e.ChangedRange.SetStyle(Styles.Label, @"@\w+");
            e.ChangedRange.SetStyle(Styles.LineNumber, @"^\s*(?<range>\d+)", RegexOptions.Multiline);
            if (!suppressEvents && !hasChange && !TbCode.ReadOnly)
            {
                hasChange = true;
                CodeEdit(this, new EventArgs());
            }
        }

        #endregion
    }
}
