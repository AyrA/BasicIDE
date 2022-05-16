using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BasicIDE
{
    public partial class FrmMain : Form
    {
        private bool HasChange = false;
        private Project ProjectFile = null;
        private FrmEditor buildWindow;

        public FrmMain(string Filename)
        {
            InitializeComponent();
            DlgSave.Filter = DlgOpen.Filter;
            DlgSave.DefaultExt = DlgOpen.DefaultExt;
            try
            {
                if (string.IsNullOrWhiteSpace(Filename))
                {
                    ProjectFile = null;
                }
                else
                {
                    LoadProject(new Project(Filename));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Project load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            TsmiBuildType.SelectedIndex = 0;
        }

        public bool SaveAll()
        {
            if (ProjectFile == null)
            {
                NoProjectMessage();
                return false;
            }
            ProjectFile.Save();
            var ok = true;
            foreach (var Editor in MdiChildren.OfType<FrmEditor>())
            {
                ok &= Editor.Save();
            }
            if (ok)
            {
                SetChange(false);
            }
            PopulateTree(ProjectFile);
            return ok;
        }

        public bool SaveFunction(string Name, string[] Code)
        {
            if (ProjectFile == null)
            {
                NoProjectMessage();
                return false;
            }
            var Labels = Basic.Compiler.GetLabels(Code);
            var Functions = ProjectFile.GetFunctions().Select(m => m.FunctionName.ToUpper()).ToArray();
            var Existing = Labels.FirstOrDefault(m => Functions.Contains(m));
            if (Existing != null)
            {
                var frm = GetEditorWindow(Name);
                MessageBox.Show($"The label @{Existing} cannot be used because a function with that name already exists.\r\n" +
                    $"The file cannot be saved at this point",
                    "Cannot save file",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                if (frm != null)
                {
                    frm.BringToFront();
                    frm.SelectText($"\n@{Existing}");
                }
                return false;
            }

            ProjectFile.SaveFunction(Name, string.Join(Environment.NewLine, Code));
            PopulateTree(ProjectFile);
            return true;
        }

        private void PopulateTree(Project P)
        {
            TreeDocuments.Nodes.Clear();
            if (P != null)
            {
                var Root = new TreeNode(P.Title, P.GetFunctions().Select(m => new TreeNode(m.ToString())).ToArray());
                Root.Expand();
                TreeDocuments.Nodes.Add(Root);
            }
        }

        private void SetChange(bool HasChange)
        {
            this.HasChange = HasChange;
            SetTitle();
        }

        private void SetTitle()
        {
            var Name = ProjectFile == null ? "<Unnamed>" : ProjectFile.Title;
            var Change = HasChange ? "*" : "";
            Text = $"TRS-80 Basic IDE [{Name}]{Change}";
        }

        private FrmEditor ShowCode(string Title)
        {
            var Window = GetEditorWindow(Title);
            if (Window != null)
            {
                Window.BringToFront();
            }
            else
            {
                Window = ShowCode(Title, ProjectFile.GetFunctionCode(Title));
            }
            return Window;
        }

        private FrmEditor ShowCode(string Title, string[] Lines, bool Readonly = false)
        {
            var E = GetEditorWindow(Title);
            if (E != null)
            {
                E.BringToFront();
                return E;
            }
            E = new FrmEditor(Title, Lines, Readonly)
            {
                MdiParent = this
            };
            if (!Readonly)
            {
                E.CodeEdit += E_CodeEdit;
            }
            E.Show();
            return E;
        }

        private void SetWindowMenu()
        {
            var Items = TsmiWindow.DropDownItems
                .OfType<ToolStripItem>()
                .Where(m => m.Tag is Form)
                .ToArray();
            foreach (var Item in Items)
            {
                TsmiWindow.DropDownItems.Remove(Item);
            }
            foreach (var Window in MdiChildren)
            {
                var NewItem = TsmiWindow.DropDownItems.Add(Window.Text);
                NewItem.Tag = Window;
                NewItem.Click += TsmiDynWindow_Click;
            }
        }

        private Basic.CompilerResult Compile()
        {
            if (ProjectFile == null)
            {
                NoProjectMessage();
                return null;
            }
            PopulateTree(ProjectFile);
            var IsDebug = TsmiBuildType.SelectedItem.ToString() == "Debug";
            var C = new Basic.Compiler(IsDebug ? Basic.CompilerConfig.Debug : Basic.CompilerConfig.Release);
            return C.Compile(ProjectFile.GetAllCode(), ProjectFile.GetFunctions().ToArray());
        }

        private void NoProjectMessage()
        {
            MBox.W("You must open or create a project first", "No project");
        }

        private bool AddFunction()
        {
            if (ProjectFile == null)
            {
                NoProjectMessage();
                return false;
            }
            using (var Prompt = new FrmInput("Please enter the function name", "New file", Expression: @"^\w+$"))
            {
                Prompt.InvalidFormatMessage = "Function name must be alphanumeric";
                if (Prompt.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var Labels = Basic.Compiler.GetLabels(ProjectFile.GetAllCode());
                        if (Labels.Contains(Prompt.Response.ToUpper()))
                        {
                            throw new Exception("Cannot name a function after an existing label.");
                        }
                        ProjectFile.AddFunction(Prompt.Response);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            ex.Message,
                            "Failed to add function",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        return false;
                    }
                    PopulateTree(ProjectFile);
                    ShowCode(Prompt.Response, ProjectFile.GetFunctionCode(Prompt.Response));
                    return true;
                }
            }
            return false;
        }

        private bool RenameProject()
        {
            if (ProjectFile == null)
            {
                NoProjectMessage();
                return false;
            }
            using (var frmQuery = new FrmInput("Enter new project name", "Rename project"))
            {
                if (frmQuery.ShowDialog() == DialogResult.OK)
                {
                    ProjectFile.Title = frmQuery.Response;
                    ProjectFile.Save();
                    PopulateTree(ProjectFile);
                    return true;
                }
            }
            return false;
        }

        private bool RenameFunction(string FunctionName)
        {
            if (ProjectFile == null)
            {
                NoProjectMessage();
                return false;
            }
            if (HasChange)
            {
                if (MessageBox.Show(
                    "You have unsaved changes. You must save all changes before a rename operation.\r\n" +
                    "Save now?",
                    "Unsaved changes",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    return false;
                }
                SaveAll();
                //Abort if there are still changes (save failed)
                if (HasChange)
                {
                    return false;
                }
            }
            using (var Prompt = new FrmInput("Enter the new function name", "Rename function", FunctionName, false, @"^\w+$"))
            {
                if (Prompt.ShowDialog() == DialogResult.OK)
                {
                    var NewName = Prompt.Response;
                    if (ProjectFile.HasFunction(NewName))
                    {
                        MessageBox.Show("A function with that name already exists", "Function exists", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                    try
                    {
                        if (FunctionName == ProjectFile.MainFunction)
                        {
                            ProjectFile.SetMainFunction(NewName);
                            SaveAll();
                        }
                        else
                        {
                            ProjectFile.RenameFunction(FunctionName, NewName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Rename failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    var Editor = GetEditorWindow(FunctionName);
                    if (Editor != null)
                    {
                        Editor.Text = NewName;
                    }
                    PopulateTree(ProjectFile);
                    return true;
                }
            }
            return false;
        }

        private FrmEditor GetEditorWindow(string FunctionName)
        {
            return MdiChildren.OfType<FrmEditor>().FirstOrDefault(m => m.FunctionName == FunctionName);
        }

        private void LoadProject(Project P)
        {
            if (ProjectFile != null)
            {
                throw new InvalidOperationException("A project is already loaded");
            }
            ProjectFile = P;
            if (!ProjectFile.HasFunction(ProjectFile.MainFunction))
            {
                ProjectFile.SaveFunction(ProjectFile.MainFunction, "PRINT \"Hello, World!\"");
            }
            SetChange(false);
            PopulateTree(ProjectFile);
            ShowCode(ProjectFile.MainFunction).WindowState = FormWindowState.Maximized;
        }

        #region Events

        private void E_CodeEdit(object sender, EventArgs e)
        {
            SetChange(true);
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //NOOP
        }

        private void TsmiNew_Click(object sender, EventArgs e)
        {
            var Backup = DlgSave.Title;
            DlgSave.Title = "Create a new project";
            if (DlgSave.ShowDialog() == DialogResult.OK)
            {
                DlgSave.Title = Backup;
                var P = new Project();
                try
                {
                    P.SaveAs(DlgSave.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to create a new project.\r\n{ex.Message}",
                        "New project",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                //Use current application instance if no project loaded yet
                if (ProjectFile == null)
                {
                    ProjectFile = P;
                    SetChange(false);
                    PopulateTree(ProjectFile);
                }
                else
                {
                    System.Diagnostics.Process.Start(Application.ExecutablePath, DlgSave.FileName);
                }
            }
            DlgSave.Title = Backup;
        }

        private void TsmiAddFile_Click(object sender, EventArgs e)
        {
            AddFunction();
        }

        private void TsmiOpen_Click(object sender, EventArgs e)
        {
            if (DlgOpen.ShowDialog() == DialogResult.OK)
            {
                Project P;

                try
                {
                    P = new Project(DlgOpen.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to load project.\r\n{ex.Message}",
                        "Load project",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                //Use current application instance if no project loaded yet
                if (ProjectFile == null)
                {
                    LoadProject(P);
                }
                else
                {
                    System.Diagnostics.Process.Start(Application.ExecutablePath, DlgSave.FileName);
                }
            }
        }

        private void TreeDocuments_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            var N = TreeDocuments.SelectedNode;
            if (N == null)
            {
                return;
            }
            if (N == TreeDocuments.Nodes[0])
            {
                RenameProject();
            }
            else
            {
                //Get function name without arguments
                var FuncName = N.Text.Substring(0, N.Text.IndexOf('('));
                ShowCode(FuncName, ProjectFile.GetFunctionCode(FuncName));
            }
        }

        private void TsmiBuild_Click(object sender, EventArgs e)
        {
            if (ProjectFile == null)
            {
                NoProjectMessage();
                return;
            }
            if (HasChange)
            {
                if (MessageBox.Show(
                    "You have unsaved changes that would not be included if you build now. " +
                    "Save all changes first?",
                    "Unsaved changes",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (!SaveAll())
                    {
                        return;
                    }
                }
            }
            var Source = ProjectFile.GetAllCode();
            var Result = Compile();

            LvErrors.SuspendLayout();
            LvErrors.Items.Clear();
            foreach (var E in Result.Errors.Concat(Result.Warnings).Concat(Result.Infos))
            {
                var Item = LvErrors.Items.Add(E.ErrorType.ToString());
                if (E.ErrorType != Basic.SyntaxErrorType.Info)
                {
                    Item.BackColor = E.ErrorType == Basic.SyntaxErrorType.Warning ? Color.FromArgb(0xFF, 0xFF, 0xDD) : Color.FromArgb(0xFF, 0xDD, 0xDD);
                }
                Item.SubItems.Add(string.IsNullOrEmpty(E.FunctionName) ? "?" : E.FunctionName);
                Item.SubItems.Add(Source[E.LineIndex]);
                Item.SubItems.Add(E.Message);
            }
            LvErrors.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            LvErrors.Visible = LvErrors.Items.Count > 0;
            LvErrors.ResumeLayout();

            if (Result.Errors.Length > 0)
            {
                return;
            }

            if (buildWindow != null && !buildWindow.IsDisposed)
            {
                buildWindow.Close();
                buildWindow.Dispose();
            }
            buildWindow = ShowCode("Compilation result", Result.GetLines(), true);
        }

        private void TsmiSave_Click(object sender, EventArgs e)
        {
            if (ProjectFile == null)
            {
                NoProjectMessage();
                return;
            }
            SaveAll();
        }

        private void TsmiExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TsmiSaveAs_Click(object sender, EventArgs e)
        {
            if (ProjectFile == null)
            {
                NoProjectMessage();
                return;
            }

            if (DlgSave.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ProjectFile.SaveAs(DlgSave.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to create a new project.\r\n{ex.Message}",
                        "New project",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                SaveAll();
            }
        }

        private void MenuMain_MenuActivate(object sender, EventArgs e)
        {
            var Ports = System.IO.Ports.SerialPort.GetPortNames().OrderBy(m => m);
            TsmiUpload.DropDownItems.Clear();
            TsmiBackup.DropDownItems.Clear();
            TsmiTerminal.DropDownItems.Clear();
            foreach (var SP in Ports)
            {
                var UploadItem = TsmiUpload.DropDownItems.Add(SP);
                UploadItem.Click += TsmiDynUpload_Click;

                var BackupItem = TsmiBackup.DropDownItems.Add(SP);
                BackupItem.Click += TsmiDynBackup_Click;

                var TerminalItem = TsmiTerminal.DropDownItems.Add(SP);
                TerminalItem.Click += TsmiDynTerminal_Click;
            }
            SetWindowMenu();
        }

        private void TsmiClearMessages_Click(object sender, EventArgs e)
        {
            LvErrors.Items.Clear();
            LvErrors.Visible = false;
        }

        private void TsmiDynUpload_Click(object sender, EventArgs e)
        {
            if (ProjectFile == null)
            {
                NoProjectMessage();
                return;
            }
            var Item = (ToolStripItem)sender;
            var PortName = Item.Text;
            var Result = Compile();
            if (Result.Errors.Length > 0)
            {
                MessageBox.Show(
                    "Cannot send file because it contains errors. Please build and try again.",
                    "Compilation failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            //Upload form
            using (var Uploader = new FrmUpload(PortName, Result.GetLines()))
            {
                Uploader.ShowDialog();
            }
        }

        private void TsmiDynBackup_Click(object sender, EventArgs e)
        {
            var Item = (ToolStripItem)sender;
            var PortName = Item.Text;
            var Frm = MdiChildren.OfType<FrmTransfer>().FirstOrDefault(m => m.PortName == PortName);
            if (Frm == null)
            {
                Frm = new FrmTransfer(PortName)
                {
                    MdiParent = this
                };
                Frm.Show();
            }
            else
            {
                Frm.BringToFront();
            }
        }

        private void TsmiDynTerminal_Click(object sender, EventArgs e)
        {
            var Item = (ToolStripItem)sender;
            var PortName = Item.Text;
            var Frm = MdiChildren.OfType<FrmTerminal>().FirstOrDefault(m => m.PortName == PortName);
            if (Frm == null)
            {
                Frm = new FrmTerminal(PortName)
                {
                    MdiParent = this
                };
                Frm.Show();
            }
            else
            {
                Frm.BringToFront();
            }
        }

        private void TsmiCustomize_Click(object sender, EventArgs e)
        {
            DlgFont.Font = Program.Config.EditorFont.GetFont();
            if (DlgFont.ShowDialog() == DialogResult.OK)
            {
                Program.Config.EditorFont = new FontInfo(DlgFont.Font);
                Program.SaveSettings();
            }
        }

        private void TsmiOptions_Click(object sender, EventArgs e)
        {
            var F = MdiChildren.OfType<FrmOptions>().FirstOrDefault();
            if (F == null)
            {
                F = new FrmOptions()
                {
                    MdiParent = this
                };
                F.Show();
            }
            F.BringToFront();
        }

        private void CmsAddFunction_Click(object sender, EventArgs e)
        {
            AddFunction();
        }

        private void CmsRenameFunction_Click(object sender, EventArgs e)
        {
            var N = TreeDocuments.SelectedNode;
            if (N == null || N.Parent == null)
            {
                RenameProject();
            }
            else
            {
                var FuncName = N.Text.Substring(0, N.Text.IndexOf('('));
                RenameFunction(FuncName);
            }
        }

        private void CmsDeleteFunction_Click(object sender, EventArgs e)
        {
            if (ProjectFile == null)
            {
                NoProjectMessage();
                return;
            }
            var N = TreeDocuments.SelectedNode;
            if (N == null || N.Parent == null)
            {
                return;
            }
            try
            {
                ProjectFile.DeleteFunction(N.Text);
            }
            catch (Exception ex)
            {
                MBox.E(ex.Message, "Cannot delete function");
            }
            PopulateTree(ProjectFile);
        }

        private void TreeDocuments_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeDocuments.SelectedNode = e.Node;
        }

        private void LvErrors_DoubleClick(object sender, EventArgs e)
        {
            if (LvErrors.SelectedItems.Count > 0)
            {
                var Item = LvErrors.SelectedItems[0];
                var FunctionName = Item.SubItems[1].Text;
                var Line = Item.SubItems[2].Text.TrimEnd();
                var CodeWindow = ShowCode(FunctionName);
                if (CodeWindow != null)
                {
                    CodeWindow.BringToFront();
                    CodeWindow.SelectText(Line);
                }
            }
        }

        private void TsmiTileHorizontal_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void TsmiTileVertical_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TsmiMaximize_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
            if (ActiveMdiChild != null)
            {
                ActiveMdiChild.WindowState = FormWindowState.Maximized;
            }
        }

        private void TsmiCascade_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TsmiDynWindow_Click(object sender, EventArgs e)
        {
            var Item = (ToolStripMenuItem)sender;
            if (Item.Tag is Form Window)
            {
                Window.BringToFront();
                Window.Focus();
            }
        }

        private void TsmiAbout_Click(object sender, EventArgs e)
        {
            using (var Dlg = new FrmAbout())
            {
                Dlg.ShowDialog();
            }
        }

        private void TsmiProjectWebsite_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/AyrA/BasicIDE").Dispose();
        }

#pragma warning disable IDE0019 //Pattern matching

        private void TsmiCut_Click(object sender, EventArgs e)
        {
            var Current = ActiveMdiChild?.ActiveControl as TextBox;
            if (Current == null)
            {
                return;
            }
            if (!Current.ReadOnly)
            {
                Current.Cut();
            }
        }

        private void TsmiCopy_Click(object sender, EventArgs e)
        {
            var Current = ActiveMdiChild?.ActiveControl as TextBox;
            if (Current == null)
            {
                return;
            }
            Current.Copy();
        }

        private void TsmiPaste_Click(object sender, EventArgs e)
        {
            var Current = ActiveMdiChild?.ActiveControl as TextBox;
            if (Current == null)
            {
                return;
            }
            if (!Current.ReadOnly)
            {
                Current.Paste();
            }
        }

        private void TsmiSelectAll_Click(object sender, EventArgs e)
        {
            var Current = ActiveMdiChild?.ActiveControl as TextBox;
            if (Current == null)
            {
                return;
            }
            Current.SelectAll();
        }

        private void TsmiUndo_Click(object sender, EventArgs e)
        {
            var Current = ActiveMdiChild?.ActiveControl as TextBox;
            if (Current == null)
            {
                return;
            }
            if (Current.CanUndo)
            {
                Current.Undo();
            }
        }

        private void TsmiShowHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://help.ayra.ch/basic-ide");
        }

        private void TsmiShowBasicReference_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://help.ayra.ch/trs80-reference");
        }

#pragma warning restore IDE0019

        #endregion
    }
}
