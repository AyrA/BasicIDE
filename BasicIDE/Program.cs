using System;
using System.IO;
using System.Windows.Forms;

namespace BasicIDE
{
    public static class Program
    {
        /// <summary>
        /// Event that is fired when the configuration changes
        /// </summary>
        public static event Action ConfigUpdate = delegate { };

        /// <summary>
        /// Current configuration
        /// </summary>
        public static Settings Config { get; private set; }

        /// <summary>
        /// Default settings file name
        /// </summary>
        public static readonly string SettingsFile = Path.Combine(Application.StartupPath, "config.xml");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
#if DEBUG
            Args = new string[] { @"C:\Temp\div.t80" };
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

        /// <summary>
        /// Reloads settings from file
        /// </summary>
        public static void ReloadSettings()
        {
            ReloadSettings(false);
            ConfigUpdate();
        }

        /// <summary>
        /// Reloads settings from file
        /// </summary>
        /// <param name="IsInit">true, if first load</param>
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
                    //Create config if it doesn't exists
                    Config = new Settings();
                    return;
                }
                catch (Exception ex)
                {
                    //Error on initial load can be ignored to apply defaults
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
                        switch (MBox.EARI(string.Join(Environment.NewLine, lines), "Settings unavailable"))
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
                        switch (MBox.ERC(string.Join(Environment.NewLine, lines), "Settings unavailable"))
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

        /// <summary>
        /// Saves settings to file
        /// </summary>
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
                        $"Reason: {ex.Message}"
                    };
                    switch (MBox.ERC(string.Join(Environment.NewLine, lines), "Settings failed to save"))
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
}
