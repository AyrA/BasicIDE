using System;
using System.IO;
using System.Windows.Forms;

namespace BasicIDE
{
    public static class Program
    {
        public static event Action ConfigUpdate = delegate { };

        public static Settings Config { get; private set; }

        public static readonly string SettingsFile = Path.Combine(Application.StartupPath, "config.xml");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
#if DEBUG
            //Args = new string[] { @"C:\Temp\help.t80" };
#endif
            ReloadSettings(true);
            if (Config != null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmMain(Args.Length > 0 ? Args[0] : null));
                Environment.Exit(0);
            }
            else
            {
                Environment.Exit(1);
            }
        }

        public static void ReloadSettings()
        {
            ReloadSettings(false);
            ConfigUpdate();
        }

        private static void ReloadSettings(bool IsInit)
        {
            while (true)
            {
                try
                {
                    Config = Settings.Deserialize(File.ReadAllText(SettingsFile));
                    return;
                }
                catch (FileNotFoundException)
                {
                    Config = new Settings();
                    return;
                }
                catch (Exception ex)
                {
                    if (IsInit)
                    {
                        var lines = new string[]
                        {
                            "Configuration failed to load.",
                            $"Reason: {ex.Message}",
                            "",
                            "[Abort] Close application",
                            "[Retry] Try to load again",
                            "[Ignore] Use default configuration"
                        };
                        switch (MessageBox.Show(string.Join(Environment.NewLine, lines), "Settings unavailable", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error))
                        {
                            case DialogResult.Abort:
                                Config = null;
                                return;
                            case DialogResult.Retry:
                                break;
                            case DialogResult.Ignore:
                                Config = new Settings();
                                return;
                        }
                    }
                    else
                    {
                        var lines = new string[]
                        {
                            "Configuration failed to reload.",
                            $"Reason: {ex.Message}",
                            "",
                            "[Retry] Try to load again",
                            "[Cancel] Skip loading and use existing configuration in memory"
                        };
                        switch (MessageBox.Show(string.Join(Environment.NewLine, lines), "Settings unavailable", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
                        {
                            case DialogResult.Retry:
                                break;
                            case DialogResult.Cancel:
                                return;
                        }
                    }
                }
            }
        }

        public static void SaveSettings()
        {
            while (true)
            {
                try
                {
                    File.WriteAllText(SettingsFile, Config.Serialize());
                    ConfigUpdate();
                    return;
                }
                catch (Exception ex)
                {
                    var lines = new string[]
                        {
                            "Configuration failed to save.",
                            $"Reason: {ex.Message}",
                            "",
                            "Try again?"
                        };
                    switch (MessageBox.Show(string.Join(Environment.NewLine, lines), "Settings failed to save", MessageBoxButtons.YesNo, MessageBoxIcon.Error))
                    {
                        case DialogResult.Yes:
                            break;
                        case DialogResult.No:
                            return;
                    }
                }
            }
        }
    }
}
