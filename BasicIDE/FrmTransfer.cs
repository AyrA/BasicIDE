using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace BasicIDE
{
    public partial class FrmTransfer : Form
    {
        public string PortName { get; }

        private volatile bool Cancel = false;
        private Thread SerialThread = null;

        public FrmTransfer(string PortName)
        {
            if (string.IsNullOrWhiteSpace(PortName))
            {
                throw new ArgumentException($"'{nameof(PortName)}' cannot be null or whitespace.", nameof(PortName));
            }

            InitializeComponent();
            this.PortName = PortName;
        }

        private void SetStatus(string Msg, bool Add = false)
        {
            if (InvokeRequired)
            {
                Invoke((Action)delegate { SetStatus(Msg, Add); });
            }
            else
            {
                if (Add)
                {
                    LblStatus.Text += "\r\n" + Msg;
                }
                else
                {
                    LblStatus.Text = Msg;
                }
            }
        }

        private void WaitForReady(SerialPort P)
        {
            if (!Program.Config.SerialSettings.PrimitiveCable)
            {
                SetStatus("Waiting for carrier detect on serial port...", true);
                while (!P.CDHolding && P.BytesToRead == 0)
                {
                    Thread.Sleep(500);
                }
            }
        }

        private void SendThread(object o)
        {
            var args = o as object[];
            var Port = (SerialPort)args[0];
            var Filename = (string)args[1];
            FileStream FS = null;
            Cancel = false;
            using (Port)
            {
                try
                {
                    FS = File.OpenRead(Filename);
                }
                catch (Exception ex)
                {
                    SetStatus($"Transfer failed. {ex.Message}");
                    Invoke((Action)delegate
                    {
                        MBox.E("File failed to open\r\n" + ex.Message, "Cannot open file");
                    });
                }
                if (FS != null)
                {
                    using (FS)
                    {
                        if (FS.Length > short.MaxValue)
                        {
                            var Continue = (bool)Invoke((Func<bool>)delegate
                            {
                                var Size = Tools.FormatSize(FS.Length);
                                return MBox.WYN(
                                    $"The file you selected (size: {Size}) is larger than the default maximum memory of the TRS-80 (32KB)",
                                    "File possibly too large") == DialogResult.Yes;
                            });
                            if (!Continue)
                            {
                                SetStatus("Transfer aborted by user");
                                Invoke((Action)delegate { SetCancelMode(false); });
                                SerialThread = null;
                                return;
                            }
                        }
                        int count = 0;
                        byte[] buffer = new byte[Port.WriteBufferSize];
                        WaitForReady(Port);
                        do
                        {
                            count = FS.Read(buffer, 0, buffer.Length);
                            SetStatus($"Sent: {FS.Position}/{FS.Length} ({FS.Position * 100 / FS.Length}%)");
                            Port.BaseStream.Write(buffer, 0, count);
                        } while (!Cancel && count > 0);
                        Port.BaseStream.Write(new byte[] { 0x1A, 0x0A }, 0, 2);
                        Port.BaseStream.Flush();
                    }
                    SetStatus("Waiting for buffers to flush...");
                    Port.Close();
                }
            }
            SetStatus("Transfer complete");
            Invoke((Action)delegate { SetCancelMode(false); });
            SerialThread = null;
        }

        private void ReceiveThread(object o)
        {
            var args = o as object[];
            var Port = (SerialPort)args[0];
            var Filename = (string)args[1];
            FileStream FS = null;
            Cancel = false;
            using (Port)
            {
                try
                {
                    FS = File.Create(Filename);
                }
                catch (Exception ex)
                {
                    SetStatus($"Transfer failed. {ex.Message}");
                    Invoke((Action)delegate
                    {
                        MBox.E("File failed to open\r\n" + ex.Message, "Cannot open file");
                    });
                }
                if (FS != null)
                {
                    using (FS)
                    {
                        try
                        {
                            WaitForReady(Port);
                        }
                        catch (Exception ex)
                        {
                            SetStatus($"Transmission error\r\n{ex.Message}");
                            return;
                        }
                        int Count = 0;
                        int Total = 0;
                        byte[] Data = new byte[100];
                        var Timer = Stopwatch.StartNew();
                        while (!Cancel && Port.IsOpen)
                        {
                            //Increase the buffer if transmission is very fast
                            if (Timer.ElapsedMilliseconds < 250)
                            {
                                Data = new byte[Data.Length * 2];
                            }
                            try
                            {
                                Timer.Restart();
                                while(!Cancel && Port.BytesToRead == 0)
                                {

                                }
                                Count = Port.BaseStream.Read(Data, 0, Data.Length);
                            }
                            catch (Exception ex)
                            {
                                SetStatus($"Transmission error\r\n{ex.Message}");
                                return;
                            }
                            if (Data.IndexOf((byte)0x1A) >= 0)
                            {
                                Count = Data.IndexOf((byte)0x1A);
                                FS.Write(Data, 0, Count);
                                Port.Close();
                            }
                            else
                            {
                                FS.Write(Data, 0, Count);
                            }
                            Total += Count;
                            SetStatus($"Processed bytes: {Total}");
                        }
                        SetStatus("Transfer completed");
                    }
                }
            }
            Invoke((Action)delegate { SetCancelMode(false); });
            SerialThread = null;
        }

        private void SetCancelMode(bool EnableCancelMode)
        {
            Cancel = EnableCancelMode;
            foreach (var btn in Controls.OfType<Button>())
            {
                btn.Enabled = !EnableCancelMode;
            }
            BtnCancelClose.Enabled = true;
            BtnCancelClose.Text = EnableCancelMode ? "&Cancel" : "&Close";
        }

        private SerialPort OpenPort(bool IsBasic, bool IsSendToTrs)
        {
            try
            {
                var Port = Program.Config.SerialSettings.GetPort(PortName, IsBasic, IsSendToTrs);
                Port.Open();
                return Port;
            }
            catch (Exception ex)
            {
                MBox.E(
                    $"Failed to open port {PortName}. Port may be in use.\r\n" +
                    $"Message: {ex.Message}", "Port failed");
            }
            return null;
        }

        private void SendFile(string Filename, bool IsBasic)
        {
            if (SerialThread != null)
            {
                MBox.E("Transmission already in progress");
            }
            if (!IsBasic && Program.Config.SerialSettings.BaudRate > 300)
            {
                if (MBox.WYN("Baud rates over 300 may cause data loss in terminal.\r\n" +
                    "Continue anyway?", "Baud rate") == DialogResult.No)
                {
                    return;
                }
            }
            var Port = OpenPort(IsBasic, true);
            if (Port == null)
            {
                return;
            }
            var Settings = Program.Config.SerialSettings.GetTrsSerialConfig(IsBasic, true);
            var Msg = IsBasic ?
                "Run the following command in BASIC before continuing\r\n" +
                $"LOAD \"com:{Settings}\"" :
                $"Use the STAT command in TELCOM to set the port to {Settings}, " +
                "then press F4 to start the terminal and F2 to store the file";
            if (MBox.IOC(Msg, "Getting your TRS-80 ready") == DialogResult.Cancel)
            {
                Port.Close();
                Port.Dispose();
                return;
            }

            SetCancelMode(true);

            SerialThread = new Thread(SendThread)
            {
                IsBackground = true,
                Name = "TRS-80 file transfer"
            };
            SerialThread.Start(new object[] { Port, Filename });
        }

        private void ReceiveFile(string Filename, bool IsBasic)
        {
            var Port = OpenPort(IsBasic, false);
            if (Port == null)
            {
                return;
            }
            var Settings = Program.Config.SerialSettings.GetTrsSerialConfig(IsBasic, false);
            var Msg = IsBasic ?
                "Run the following command in BASIC:\r\n" +
                $"SAVE \"com:{Settings}\"" :
                $"Use the STAT command in TELCOM to set the port to {Settings}, " +
                "then press F4 to start the terminal and F3 to send the file";
            SetStatus(Msg);
            SetCancelMode(true);
            SerialThread = new Thread(ReceiveThread)
            {
                IsBackground = true,
                Name = "TRS-80 file transfer"
            };
            SerialThread.Start(new object[] { Port, Filename });
        }

        private void BtnUploadDoc_Click(object sender, EventArgs e)
        {
            OFD.Filter = "Documents|*.do;*.txt|All files|*.*";
            OFD.DefaultExt = ".do";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                SendFile(OFD.FileName, false);
            }
        }

        private void BtnUploadBasic_Click(object sender, EventArgs e)
        {
            OFD.Filter = "BASIC programs|*.ba;*.bas|All files|*.*";
            OFD.DefaultExt = ".ba";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                SendFile(OFD.FileName, true);
            }
        }

        private void BtnDownloadDoc_Click(object sender, EventArgs e)
        {
            SFD.Filter = "Documents|*.do;*.txt|All files|*.*";
            OFD.DefaultExt = ".do";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                ReceiveFile(SFD.FileName, false);
            }
        }

        private void BtnDownloadBasic_Click(object sender, EventArgs e)
        {
            SFD.Filter = "BASIC programs|*.ba;*.bas|All files|*.*";
            SFD.DefaultExt = ".ba";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                ReceiveFile(SFD.FileName, true);
            }
        }

        private void BtnCancelClose_Click(object sender, EventArgs e)
        {
            var T = SerialThread;
            if (T != null)
            {
                Cancel = true;
                if (!T.Join(5000))
                {
                    T.Abort();
                }
                SerialThread = null;
                SetCancelMode(false);
            }
            else
            {
                Close();
            }
        }
    }
}
