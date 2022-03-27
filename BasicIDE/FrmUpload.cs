using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BasicIDE
{
    public partial class FrmUpload : Form
    {
        private const int StatusBegin = int.MinValue;
        private const int StatusAbort = StatusBegin + 1;
        private const int StatusEnd = StatusAbort + 1;
        private const int StatusWait = StatusEnd + 1;

        private volatile bool Abort;

        private readonly Thread T;
        private SerialPort SP;
        private readonly string serialPort;
        private readonly string[] code;

        public FrmUpload(string SerialPort, string[] Code)
        {
            if (string.IsNullOrWhiteSpace(SerialPort))
            {
                throw new ArgumentException($"'{nameof(SerialPort)}' cannot be null or whitespace.", nameof(SerialPort));
            }

            if (Code == null || Code.Length == 0)
            {
                throw new ArgumentNullException(nameof(Code));
            }
            serialPort = SerialPort;
            code = Code;

            InitializeComponent();

            PbStatus.Maximum = code.Length;

            LblStatus.Text = "Waiting for upload...";

            var PortStr = Program.Config.SerialSettings.GetTrsSerialConfig(true, true);
            LblTransferInfo.Text = $"To begin upload, run this BASIC command: LOAD \"COM:{PortStr}\"";

            T = new Thread(Upload)
            {
                IsBackground = true,
                Name = $"Upload to {SerialPort}",
                Priority = ThreadPriority.BelowNormal
            };
            if (Program.Config.SerialSettings.PrimitiveCable)
            {
                MessageBox.Show(
                    "Primitive mode is enabled and thus it's not possible to detect when the TRS-80 is ready.\r\n" +
                    "Run the BASIC command\r\n" +
                    $"LOAD \"COM:{PortStr}\"\r\n" +
                    "before confirming this message.",
                    "Primitive mode",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            T.Start();
        }

        private void SetStatus(int Status, string CustomMessage = null)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    SetStatus(Status, CustomMessage);
                });
            }
            else
            {
                if (CustomMessage == null)
                {
                    var Progress = Math.Min(code.Length, Math.Max(Status, 0));
                    switch (Status)
                    {
                        case StatusWait:
                            CustomMessage = "Waiting for serial ready signal...";
                            break;
                        case StatusAbort:
                            CustomMessage = "Transfer aborted";
                            BtnClose.Text = "&Close";
                            break;
                        case StatusBegin:
                            CustomMessage = "Transfer started";
                            break;
                        case StatusEnd:
                            CustomMessage = "Transfer complete. Wait for BASIC to say \"Ok\"";
                            BtnClose.Text = "&Close";
                            break;
                        default:
                            CustomMessage = $"Transferring line {Status} of {code.Length}";
                            PbStatus.Value = Progress;
                            break;
                    }
                }
                LblStatus.Text = CustomMessage;
            }
        }

        private void Upload()
        {
            Abort = false;
            int offset = 0;
            try
            {
                using (SP = Program.Config.SerialSettings.GetPort(serialPort, true, true))
                {
                    SP.WriteBufferSize = 2048;
                    SP.Open();
                    if (!Program.Config.SerialSettings.PrimitiveCable)
                    {
                        SetStatus(StatusWait);
                        do
                        {
                            Thread.Sleep(500);
                        }
                        while (!Abort && !SP.CDHolding && !SP.CtsHolding);
                    }
                    if (!Abort)
                    {
                        SetStatus(StatusBegin);
                        while (!Abort && offset < code.Length)
                        {
                            SP.WriteLine(code[offset++]);
                            SetStatus(offset);
                        }
                        SP.WriteLine("\x1A");
                    }
                    SP.Close();
                    SetStatus(StatusEnd);
                }
            }
            catch (ThreadAbortException)
            {
                //NOOP
            }
            catch (UnauthorizedAccessException)
            {
                Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show(
                        "The port seems to be in use. \r\n" +
                        "Please close any window that may be communicating with the TRS-80 and try again.",
                        "Port busy",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                });
                SetStatus(StatusAbort, "Port busy");
            }
            catch (Exception ex)
            {
                SetStatus(StatusAbort, "Transfer aborted. " + ex.Message);
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Abort = true;
            if (T.IsAlive)
            {
                BtnClose.Text = "Stopping...";
                BtnClose.Enabled = false;
                //Need a new thread because Form.Invoke will block otherwise
                new Thread(delegate ()
                {
                    try
                    {
                        T.Join(5000);
                        using (SP)
                        {
                            SP.Close();
                        }
                    }
                    catch
                    {
                        //NOOP: Race condition
                    }
                    if (!T.Join(2000))
                    {
                        T.Abort();
                    }
                    Invoke((Action)delegate
                    {
                        BtnClose.Text = "&Close";
                        BtnClose.Enabled = true;
                        SetStatus(StatusAbort, "Transfer aborted");
                        MBox.W(
                            "Transfer aborted by user. If your TRS-80 is now stuck at loading, " +
                            "press SHIFT+PAUSE (next to cursor left key) on it to abort the serial data load.\r\n" +
                            "Run the commands CLEAR and NEW to reset the interpreter.",
                            "Transfer aborted");
                    });
                })
                {
                    IsBackground = true
                }.Start();
            }
            else
            {
                DialogResult = Abort ? DialogResult.Cancel : DialogResult.OK;
            }
        }
    }
}
