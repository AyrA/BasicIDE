
namespace BasicIDE
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.DlgFont = new System.Windows.Forms.FontDialog();
            this.DlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.DlgSave = new System.Windows.Forms.SaveFileDialog();
            this.TreeDocuments = new System.Windows.Forms.TreeView();
            this.CMS = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CmsAddFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.CmsRenameFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.CmsDeleteFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.ListSplit = new System.Windows.Forms.Splitter();
            this.MenuMain = new System.Windows.Forms.MenuStrip();
            this.TsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiNew = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.TsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.TsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiSep3 = new System.Windows.Forms.ToolStripSeparator();
            this.TsmiCut = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiSep4 = new System.Windows.Forms.ToolStripSeparator();
            this.TsmiSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiProject = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiAddFile = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiBuildType = new System.Windows.Forms.ToolStripComboBox();
            this.TsmiBuild = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiClearMessages = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiTools = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiCustomize = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiTrs80 = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiBackup = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiTerminal = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiTileHorizontal = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiTileVertical = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiMaximize = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiCascade = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiSep5 = new System.Windows.Forms.ToolStripSeparator();
            this.TsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiShowHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.TsmiProjectWebsite = new System.Windows.Forms.ToolStripMenuItem();
            this.LvErrors = new System.Windows.Forms.ListView();
            this.ChType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChFunction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChLineIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SplitHandler = new System.Windows.Forms.Splitter();
            this.TsmiShowBasicReference = new System.Windows.Forms.ToolStripMenuItem();
            this.CMS.SuspendLayout();
            this.MenuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // DlgFont
            // 
            this.DlgFont.AllowScriptChange = false;
            this.DlgFont.FixedPitchOnly = true;
            this.DlgFont.FontMustExist = true;
            this.DlgFont.MaxSize = 72;
            this.DlgFont.MinSize = 1;
            this.DlgFont.ShowEffects = false;
            // 
            // DlgOpen
            // 
            this.DlgOpen.DefaultExt = "t80";
            this.DlgOpen.Filter = "TRS-80 Project|*.t80";
            this.DlgOpen.Title = "Load TRS-80 BASIC project";
            // 
            // DlgSave
            // 
            this.DlgSave.Title = "Save TRS-80 BASIC project";
            // 
            // TreeDocuments
            // 
            this.TreeDocuments.ContextMenuStrip = this.CMS;
            this.TreeDocuments.Dock = System.Windows.Forms.DockStyle.Right;
            this.TreeDocuments.Location = new System.Drawing.Point(641, 24);
            this.TreeDocuments.Name = "TreeDocuments";
            this.TreeDocuments.Size = new System.Drawing.Size(143, 537);
            this.TreeDocuments.TabIndex = 1;
            this.TreeDocuments.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeDocuments_NodeMouseClick);
            this.TreeDocuments.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TreeDocuments_MouseDoubleClick);
            // 
            // CMS
            // 
            this.CMS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CmsAddFunction,
            this.CmsRenameFunction,
            this.CmsDeleteFunction});
            this.CMS.Name = "CMS";
            this.CMS.Size = new System.Drawing.Size(150, 70);
            // 
            // CmsAddFunction
            // 
            this.CmsAddFunction.Name = "CmsAddFunction";
            this.CmsAddFunction.Size = new System.Drawing.Size(149, 22);
            this.CmsAddFunction.Text = "&Add Function";
            this.CmsAddFunction.Click += new System.EventHandler(this.CmsAddFunction_Click);
            // 
            // CmsRenameFunction
            // 
            this.CmsRenameFunction.Name = "CmsRenameFunction";
            this.CmsRenameFunction.Size = new System.Drawing.Size(149, 22);
            this.CmsRenameFunction.Text = "&Rename";
            this.CmsRenameFunction.Click += new System.EventHandler(this.CmsRenameFunction_Click);
            // 
            // CmsDeleteFunction
            // 
            this.CmsDeleteFunction.Name = "CmsDeleteFunction";
            this.CmsDeleteFunction.Size = new System.Drawing.Size(149, 22);
            this.CmsDeleteFunction.Text = "&Delete Function";
            this.CmsDeleteFunction.Click += new System.EventHandler(this.CmsDeleteFunction_Click);
            // 
            // ListSplit
            // 
            this.ListSplit.Dock = System.Windows.Forms.DockStyle.Right;
            this.ListSplit.Location = new System.Drawing.Point(629, 24);
            this.ListSplit.Name = "ListSplit";
            this.ListSplit.Size = new System.Drawing.Size(12, 537);
            this.ListSplit.TabIndex = 2;
            this.ListSplit.TabStop = false;
            // 
            // MenuMain
            // 
            this.MenuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmiFile,
            this.TsmiEdit,
            this.TsmiProject,
            this.TsmiTools,
            this.TsmiTrs80,
            this.TsmiWindow,
            this.TsmiHelp});
            this.MenuMain.Location = new System.Drawing.Point(0, 0);
            this.MenuMain.Name = "MenuMain";
            this.MenuMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MenuMain.Size = new System.Drawing.Size(784, 24);
            this.MenuMain.TabIndex = 4;
            this.MenuMain.Text = "menuStrip1";
            this.MenuMain.MenuActivate += new System.EventHandler(this.MenuMain_MenuActivate);
            // 
            // TsmiFile
            // 
            this.TsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmiNew,
            this.TsmiOpen,
            this.TsmiSep1,
            this.TsmiSave,
            this.TsmiSaveAs,
            this.TsmiSep2,
            this.TsmiExit});
            this.TsmiFile.Name = "TsmiFile";
            this.TsmiFile.Size = new System.Drawing.Size(35, 20);
            this.TsmiFile.Text = "&File";
            // 
            // TsmiNew
            // 
            this.TsmiNew.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.TsmiNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TsmiNew.Name = "TsmiNew";
            this.TsmiNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.TsmiNew.Size = new System.Drawing.Size(136, 22);
            this.TsmiNew.Text = "&New";
            this.TsmiNew.Click += new System.EventHandler(this.TsmiNew_Click);
            // 
            // TsmiOpen
            // 
            this.TsmiOpen.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.TsmiOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TsmiOpen.Name = "TsmiOpen";
            this.TsmiOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.TsmiOpen.Size = new System.Drawing.Size(136, 22);
            this.TsmiOpen.Text = "&Open";
            this.TsmiOpen.Click += new System.EventHandler(this.TsmiOpen_Click);
            // 
            // TsmiSep1
            // 
            this.TsmiSep1.Name = "TsmiSep1";
            this.TsmiSep1.Size = new System.Drawing.Size(133, 6);
            // 
            // TsmiSave
            // 
            this.TsmiSave.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.TsmiSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TsmiSave.Name = "TsmiSave";
            this.TsmiSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.TsmiSave.Size = new System.Drawing.Size(136, 22);
            this.TsmiSave.Text = "&Save";
            this.TsmiSave.Click += new System.EventHandler(this.TsmiSave_Click);
            // 
            // TsmiSaveAs
            // 
            this.TsmiSaveAs.Name = "TsmiSaveAs";
            this.TsmiSaveAs.Size = new System.Drawing.Size(136, 22);
            this.TsmiSaveAs.Text = "Save &As";
            this.TsmiSaveAs.Click += new System.EventHandler(this.TsmiSaveAs_Click);
            // 
            // TsmiSep2
            // 
            this.TsmiSep2.Name = "TsmiSep2";
            this.TsmiSep2.Size = new System.Drawing.Size(133, 6);
            // 
            // TsmiExit
            // 
            this.TsmiExit.Name = "TsmiExit";
            this.TsmiExit.Size = new System.Drawing.Size(136, 22);
            this.TsmiExit.Text = "E&xit";
            this.TsmiExit.Click += new System.EventHandler(this.TsmiExit_Click);
            // 
            // TsmiEdit
            // 
            this.TsmiEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmiUndo,
            this.TsmiSep3,
            this.TsmiCut,
            this.TsmiCopy,
            this.TsmiPaste,
            this.TsmiSep4,
            this.TsmiSelectAll});
            this.TsmiEdit.Name = "TsmiEdit";
            this.TsmiEdit.Size = new System.Drawing.Size(37, 20);
            this.TsmiEdit.Text = "&Edit";
            // 
            // TsmiUndo
            // 
            this.TsmiUndo.Name = "TsmiUndo";
            this.TsmiUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.TsmiUndo.Size = new System.Drawing.Size(153, 22);
            this.TsmiUndo.Text = "&Undo";
            this.TsmiUndo.Click += new System.EventHandler(this.TsmiUndo_Click);
            // 
            // TsmiSep3
            // 
            this.TsmiSep3.Name = "TsmiSep3";
            this.TsmiSep3.Size = new System.Drawing.Size(150, 6);
            // 
            // TsmiCut
            // 
            this.TsmiCut.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
            this.TsmiCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TsmiCut.Name = "TsmiCut";
            this.TsmiCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.TsmiCut.Size = new System.Drawing.Size(153, 22);
            this.TsmiCut.Text = "Cu&t";
            this.TsmiCut.Click += new System.EventHandler(this.TsmiCut_Click);
            // 
            // TsmiCopy
            // 
            this.TsmiCopy.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
            this.TsmiCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TsmiCopy.Name = "TsmiCopy";
            this.TsmiCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.TsmiCopy.Size = new System.Drawing.Size(153, 22);
            this.TsmiCopy.Text = "&Copy";
            this.TsmiCopy.Click += new System.EventHandler(this.TsmiCopy_Click);
            // 
            // TsmiPaste
            // 
            this.TsmiPaste.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
            this.TsmiPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TsmiPaste.Name = "TsmiPaste";
            this.TsmiPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.TsmiPaste.Size = new System.Drawing.Size(153, 22);
            this.TsmiPaste.Text = "&Paste";
            this.TsmiPaste.Click += new System.EventHandler(this.TsmiPaste_Click);
            // 
            // TsmiSep4
            // 
            this.TsmiSep4.Name = "TsmiSep4";
            this.TsmiSep4.Size = new System.Drawing.Size(150, 6);
            // 
            // TsmiSelectAll
            // 
            this.TsmiSelectAll.Name = "TsmiSelectAll";
            this.TsmiSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.TsmiSelectAll.Size = new System.Drawing.Size(153, 22);
            this.TsmiSelectAll.Text = "Select &All";
            this.TsmiSelectAll.Click += new System.EventHandler(this.TsmiSelectAll_Click);
            // 
            // TsmiProject
            // 
            this.TsmiProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmiAddFile,
            this.TsmiBuildType,
            this.TsmiBuild,
            this.TsmiUpload,
            this.TsmiClearMessages});
            this.TsmiProject.Name = "TsmiProject";
            this.TsmiProject.Size = new System.Drawing.Size(52, 20);
            this.TsmiProject.Text = "&Project";
            // 
            // TsmiAddFile
            // 
            this.TsmiAddFile.Name = "TsmiAddFile";
            this.TsmiAddFile.Size = new System.Drawing.Size(181, 22);
            this.TsmiAddFile.Text = "&Add function";
            this.TsmiAddFile.Click += new System.EventHandler(this.TsmiAddFile_Click);
            // 
            // TsmiBuildType
            // 
            this.TsmiBuildType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TsmiBuildType.Items.AddRange(new object[] {
            "Debug",
            "Release"});
            this.TsmiBuildType.Name = "TsmiBuildType";
            this.TsmiBuildType.Size = new System.Drawing.Size(121, 21);
            // 
            // TsmiBuild
            // 
            this.TsmiBuild.Name = "TsmiBuild";
            this.TsmiBuild.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.TsmiBuild.Size = new System.Drawing.Size(181, 22);
            this.TsmiBuild.Text = "&Build";
            this.TsmiBuild.Click += new System.EventHandler(this.TsmiBuild_Click);
            // 
            // TsmiUpload
            // 
            this.TsmiUpload.Name = "TsmiUpload";
            this.TsmiUpload.Size = new System.Drawing.Size(181, 22);
            this.TsmiUpload.Text = "Build and &Upload";
            // 
            // TsmiClearMessages
            // 
            this.TsmiClearMessages.Name = "TsmiClearMessages";
            this.TsmiClearMessages.Size = new System.Drawing.Size(181, 22);
            this.TsmiClearMessages.Text = "&Clear Messages";
            this.TsmiClearMessages.Click += new System.EventHandler(this.TsmiClearMessages_Click);
            // 
            // TsmiTools
            // 
            this.TsmiTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmiCustomize,
            this.TsmiOptions});
            this.TsmiTools.Name = "TsmiTools";
            this.TsmiTools.Size = new System.Drawing.Size(45, 20);
            this.TsmiTools.Text = "&Tools";
            // 
            // TsmiCustomize
            // 
            this.TsmiCustomize.Name = "TsmiCustomize";
            this.TsmiCustomize.Size = new System.Drawing.Size(122, 22);
            this.TsmiCustomize.Text = "&Customize";
            this.TsmiCustomize.Click += new System.EventHandler(this.TsmiCustomize_Click);
            // 
            // TsmiOptions
            // 
            this.TsmiOptions.Name = "TsmiOptions";
            this.TsmiOptions.Size = new System.Drawing.Size(122, 22);
            this.TsmiOptions.Text = "&Options";
            this.TsmiOptions.Click += new System.EventHandler(this.TsmiOptions_Click);
            // 
            // TsmiTrs80
            // 
            this.TsmiTrs80.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmiBackup,
            this.TsmiTerminal});
            this.TsmiTrs80.Name = "TsmiTrs80";
            this.TsmiTrs80.Size = new System.Drawing.Size(56, 20);
            this.TsmiTrs80.Text = "T&RS-80";
            // 
            // TsmiBackup
            // 
            this.TsmiBackup.Name = "TsmiBackup";
            this.TsmiBackup.Size = new System.Drawing.Size(153, 22);
            this.TsmiBackup.Text = "&Backup/Restore";
            // 
            // TsmiTerminal
            // 
            this.TsmiTerminal.Name = "TsmiTerminal";
            this.TsmiTerminal.Size = new System.Drawing.Size(153, 22);
            this.TsmiTerminal.Text = "&Terminal";
            // 
            // TsmiWindow
            // 
            this.TsmiWindow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmiTileHorizontal,
            this.TsmiTileVertical,
            this.TsmiMaximize,
            this.TsmiCascade,
            this.TsmiSep5});
            this.TsmiWindow.Name = "TsmiWindow";
            this.TsmiWindow.Size = new System.Drawing.Size(58, 20);
            this.TsmiWindow.Text = "&Window";
            // 
            // TsmiTileHorizontal
            // 
            this.TsmiTileHorizontal.Name = "TsmiTileHorizontal";
            this.TsmiTileHorizontal.Size = new System.Drawing.Size(141, 22);
            this.TsmiTileHorizontal.Text = "Tile &Horizontal";
            this.TsmiTileHorizontal.Click += new System.EventHandler(this.TsmiTileHorizontal_Click);
            // 
            // TsmiTileVertical
            // 
            this.TsmiTileVertical.Name = "TsmiTileVertical";
            this.TsmiTileVertical.Size = new System.Drawing.Size(141, 22);
            this.TsmiTileVertical.Text = "Tile &Vertical";
            this.TsmiTileVertical.Click += new System.EventHandler(this.TsmiTileVertical_Click);
            // 
            // TsmiMaximize
            // 
            this.TsmiMaximize.Name = "TsmiMaximize";
            this.TsmiMaximize.Size = new System.Drawing.Size(141, 22);
            this.TsmiMaximize.Text = "&Maximize";
            this.TsmiMaximize.Click += new System.EventHandler(this.TsmiMaximize_Click);
            // 
            // TsmiCascade
            // 
            this.TsmiCascade.Name = "TsmiCascade";
            this.TsmiCascade.Size = new System.Drawing.Size(141, 22);
            this.TsmiCascade.Text = "&Cascade";
            this.TsmiCascade.Click += new System.EventHandler(this.TsmiCascade_Click);
            // 
            // TsmiSep5
            // 
            this.TsmiSep5.Name = "TsmiSep5";
            this.TsmiSep5.Size = new System.Drawing.Size(138, 6);
            // 
            // TsmiHelp
            // 
            this.TsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsmiShowHelp,
            this.TsmiShowBasicReference,
            this.TsmiAbout,
            this.TsmiProjectWebsite});
            this.TsmiHelp.Name = "TsmiHelp";
            this.TsmiHelp.Size = new System.Drawing.Size(41, 20);
            this.TsmiHelp.Text = "&Help";
            // 
            // TsmiShowHelp
            // 
            this.TsmiShowHelp.Name = "TsmiShowHelp";
            this.TsmiShowHelp.Size = new System.Drawing.Size(183, 22);
            this.TsmiShowHelp.Text = "&Show IDE Help";
            this.TsmiShowHelp.Click += new System.EventHandler(this.TsmiShowHelp_Click);
            // 
            // TsmiAbout
            // 
            this.TsmiAbout.Name = "TsmiAbout";
            this.TsmiAbout.Size = new System.Drawing.Size(183, 22);
            this.TsmiAbout.Text = "&About";
            this.TsmiAbout.Click += new System.EventHandler(this.TsmiAbout_Click);
            // 
            // TsmiProjectWebsite
            // 
            this.TsmiProjectWebsite.Name = "TsmiProjectWebsite";
            this.TsmiProjectWebsite.Size = new System.Drawing.Size(183, 22);
            this.TsmiProjectWebsite.Text = "&Project Website";
            this.TsmiProjectWebsite.Click += new System.EventHandler(this.TsmiProjectWebsite_Click);
            // 
            // LvErrors
            // 
            this.LvErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ChType,
            this.ChFunction,
            this.ChLineIndex,
            this.ChMessage});
            this.LvErrors.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LvErrors.FullRowSelect = true;
            this.LvErrors.GridLines = true;
            this.LvErrors.HideSelection = false;
            this.LvErrors.Location = new System.Drawing.Point(0, 464);
            this.LvErrors.Name = "LvErrors";
            this.LvErrors.Size = new System.Drawing.Size(629, 97);
            this.LvErrors.TabIndex = 6;
            this.LvErrors.UseCompatibleStateImageBehavior = false;
            this.LvErrors.View = System.Windows.Forms.View.Details;
            this.LvErrors.Visible = false;
            this.LvErrors.DoubleClick += new System.EventHandler(this.LvErrors_DoubleClick);
            // 
            // ChType
            // 
            this.ChType.Text = "Type";
            // 
            // ChFunction
            // 
            this.ChFunction.Text = "Function";
            // 
            // ChLineIndex
            // 
            this.ChLineIndex.Text = "Line";
            // 
            // ChMessage
            // 
            this.ChMessage.Text = "Message";
            // 
            // SplitHandler
            // 
            this.SplitHandler.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SplitHandler.Location = new System.Drawing.Point(0, 452);
            this.SplitHandler.Name = "SplitHandler";
            this.SplitHandler.Size = new System.Drawing.Size(629, 12);
            this.SplitHandler.TabIndex = 7;
            this.SplitHandler.TabStop = false;
            // 
            // TsmiShowBasicReference
            // 
            this.TsmiShowBasicReference.Name = "TsmiShowBasicReference";
            this.TsmiShowBasicReference.Size = new System.Drawing.Size(183, 22);
            this.TsmiShowBasicReference.Text = "Show &Basic Reference";
            this.TsmiShowBasicReference.Click += new System.EventHandler(this.TsmiShowBasicReference_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.SplitHandler);
            this.Controls.Add(this.LvErrors);
            this.Controls.Add(this.ListSplit);
            this.Controls.Add(this.TreeDocuments);
            this.Controls.Add(this.MenuMain);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.MenuMain;
            this.Name = "FrmMain";
            this.Text = "Basic IDE";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.CMS.ResumeLayout(false);
            this.MenuMain.ResumeLayout(false);
            this.MenuMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FontDialog DlgFont;
        private System.Windows.Forms.OpenFileDialog DlgOpen;
        private System.Windows.Forms.SaveFileDialog DlgSave;
        private System.Windows.Forms.TreeView TreeDocuments;
        private System.Windows.Forms.Splitter ListSplit;
        private System.Windows.Forms.MenuStrip MenuMain;
        private System.Windows.Forms.ToolStripMenuItem TsmiFile;
        private System.Windows.Forms.ToolStripMenuItem TsmiNew;
        private System.Windows.Forms.ToolStripMenuItem TsmiOpen;
        private System.Windows.Forms.ToolStripSeparator TsmiSep1;
        private System.Windows.Forms.ToolStripMenuItem TsmiSave;
        private System.Windows.Forms.ToolStripMenuItem TsmiSaveAs;
        private System.Windows.Forms.ToolStripSeparator TsmiSep2;
        private System.Windows.Forms.ToolStripMenuItem TsmiExit;
        private System.Windows.Forms.ToolStripMenuItem TsmiEdit;
        private System.Windows.Forms.ToolStripMenuItem TsmiUndo;
        private System.Windows.Forms.ToolStripSeparator TsmiSep3;
        private System.Windows.Forms.ToolStripMenuItem TsmiCut;
        private System.Windows.Forms.ToolStripMenuItem TsmiCopy;
        private System.Windows.Forms.ToolStripMenuItem TsmiPaste;
        private System.Windows.Forms.ToolStripSeparator TsmiSep4;
        private System.Windows.Forms.ToolStripMenuItem TsmiSelectAll;
        private System.Windows.Forms.ToolStripMenuItem TsmiTools;
        private System.Windows.Forms.ToolStripMenuItem TsmiCustomize;
        private System.Windows.Forms.ToolStripMenuItem TsmiOptions;
        private System.Windows.Forms.ToolStripMenuItem TsmiProject;
        private System.Windows.Forms.ToolStripMenuItem TsmiAddFile;
        private System.Windows.Forms.ToolStripMenuItem TsmiBuild;
        private System.Windows.Forms.ToolStripMenuItem TsmiUpload;
        private System.Windows.Forms.ContextMenuStrip CMS;
        private System.Windows.Forms.ToolStripMenuItem CmsAddFunction;
        private System.Windows.Forms.ToolStripMenuItem CmsRenameFunction;
        private System.Windows.Forms.ToolStripMenuItem CmsDeleteFunction;
        private System.Windows.Forms.ListView LvErrors;
        private System.Windows.Forms.ColumnHeader ChType;
        private System.Windows.Forms.ColumnHeader ChLineIndex;
        private System.Windows.Forms.ColumnHeader ChMessage;
        private System.Windows.Forms.Splitter SplitHandler;
        private System.Windows.Forms.ColumnHeader ChFunction;
        private System.Windows.Forms.ToolStripMenuItem TsmiWindow;
        private System.Windows.Forms.ToolStripMenuItem TsmiTileHorizontal;
        private System.Windows.Forms.ToolStripMenuItem TsmiTileVertical;
        private System.Windows.Forms.ToolStripMenuItem TsmiMaximize;
        private System.Windows.Forms.ToolStripMenuItem TsmiCascade;
        private System.Windows.Forms.ToolStripSeparator TsmiSep5;
        private System.Windows.Forms.ToolStripComboBox TsmiBuildType;
        private System.Windows.Forms.ToolStripMenuItem TsmiClearMessages;
        private System.Windows.Forms.ToolStripMenuItem TsmiTrs80;
        private System.Windows.Forms.ToolStripMenuItem TsmiBackup;
        private System.Windows.Forms.ToolStripMenuItem TsmiTerminal;
        private System.Windows.Forms.ToolStripMenuItem TsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem TsmiShowHelp;
        private System.Windows.Forms.ToolStripMenuItem TsmiAbout;
        private System.Windows.Forms.ToolStripMenuItem TsmiProjectWebsite;
        private System.Windows.Forms.ToolStripMenuItem TsmiShowBasicReference;
    }
}

