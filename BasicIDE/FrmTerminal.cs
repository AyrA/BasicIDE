using System;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

namespace BasicIDE
{
    public partial class FrmTerminal : Form
    {
        private readonly SerialInfo info;
        private readonly SerialPort Port;
        private readonly bool HasRts;
        private readonly bool PrimitiveMode;
        private readonly string portName;
        private bool FixLF = false;

        public string PortName { get => portName; }

        public FrmTerminal(string PortName)
        {
            portName = PortName ?? throw new ArgumentNullException(nameof(PortName));

            var C = Program.Config;
            InitializeComponent();

            info = (SerialInfo)C.SerialSettings.Clone();
            Port = C.SerialSettings.GetPort(PortName, false, false);
            HasRts = Port.Handshake == Handshake.RequestToSend || Port.Handshake == Handshake.RequestToSendXOnXOff;
            PrimitiveMode = C.SerialSettings.PrimitiveCable;
            SetLineMode("CRLF");
            Port.PinChanged += Port_PinChanged;
            Port.DataReceived += Port_DataReceived;

            Program.ConfigUpdate += Program_ConfigUpdate;

            TbSend.Font = TbTerminal.Font = C.EditorFont.GetFont();
            TsLblMode.Text = C.SerialSettings.GetTrsSerialConfig(false, false);
            Text += ": " + PortName;
        }

        private void ShowPinInfo()
        {
            if (!Port.IsOpen)
            {
                DisableLabel();
                return;
            }
            SetLabel(TsLblBrk, !Port.BreakState);
            if (PrimitiveMode)
            {
                SetLabel(TsLblCd, null);
                SetLabel(TsLblCts, null);
                SetLabel(TsLblDsr, null);
                SetLabel(TsLblDtr, null);
            }
            else
            {
                SetLabel(TsLblCd, Port.CDHolding);
                SetLabel(TsLblCts, Port.CtsHolding);
                SetLabel(TsLblDsr, Port.DsrHolding);
                SetLabel(TsLblDtr, Port.DtrEnable);
            }
            if (!HasRts && !PrimitiveMode)
            {
                SetLabel(TsLblRts, Port.RtsEnable);
            }
            else
            {
                SetLabel(TsLblRts, null);
            }
        }

        private void DisableLabel()
        {
            if (Port.IsOpen)
            {
                return;
            }
            var F = new Font(SerialInfoStrip.Font, FontStyle.Strikeout);
            foreach (var Label in SerialInfoStrip.Items.OfType<ToolStripStatusLabel>())
            {
                Label.Font = F;
            }
        }

        private void SetLabel(ToolStripStatusLabel Lbl, bool? Status)
        {
            if (Status.HasValue)
            {
                Lbl.BackColor = Status.Value ? Color.Lime : Color.Red;
            }
            else
            {
                Lbl.BackColor = Color.Gray;
            }
        }

        private void ShowPrimitiveModeMessage()
        {
            MessageBox.Show("Primitive mode is enabled in the settings.\r\n" +
                "This assumes that your cable only has the two data lines, " +
                "but none of the others.\r\n" +
                "To set and receive the signal pins you must change your settings first.",
                "Primitive mode",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);

        }

        private void SetLineMode(string NewMode)
        {
            foreach (var Item in TsDropDown.DropDownItems.OfType<ToolStripMenuItem>())
            {
                Item.Checked = Item.Tag.ToString() == NewMode;
            }
            TsDropDown.Text = $"Send Line Ending: {NewMode}";
            Port.NewLine = NewMode.Replace("CR", "\r").Replace("LF", "\n");
        }

        #region Events

        private void Program_ConfigUpdate()
        {
            if (!Program.Config.SerialSettings.Equals(info))
            {
                MessageBox.Show(
                    $"Your new serial settings will have no effect on the terminal of port {portName}.\r\n" +
                    "Close and re-open the terminal for the new settings to take effect",
                    "New serial settings",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                var Data = Port.ReadExisting();

                //Handle the possibility that CRLF may be cut into two pieces
                if (FixLF && Data.StartsWith("\n"))
                {
                    Data = Data.Substring(1);
                }
                FixLF = Data.EndsWith("\r");


                //Correct line endings
                TbTerminal.Text += Data
                    .Replace("\r\n", "\r")
                    .Replace('\n', '\r')
                    .Replace("\r", "\r\n");
                TbTerminal.Select(TbTerminal.Text.Length, 0);
                TbTerminal.ScrollToCaret();
            });
        }

        private void Port_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)ShowPinInfo);
            }
            else
            {
                ShowPinInfo();
            }
        }

        private void TbSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Port.IsOpen)
            {
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                if (PrimitiveMode || Port.CDHolding)
                {
                    if (Port.BreakState)
                    {
                        if (MessageBox.Show(
                            "Port is in break state and not usable this way.\r\n" +
                            "Change state and send line?",
                            "Port in break state",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation) == DialogResult.No)
                        {
                            return;
                        }
                        Port.BreakState = false;
                        ShowPinInfo();
                    }
                    Port.WriteLine(TbSend.Text);
                }
                else
                {
                    MessageBox.Show(
                        "Port not ready: Carrier signal is not present on the line.\r\n" +
                        "If your device is turned on and plugged in you may have a bad serial cable. " +
                        "Try enabling \"Primitive mode\" in the settings. " +
                        "If this solves the problem you should toss the cable and get a better one.",
                        "No \"Carrier Detect\" signal present",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
                TbSend.SelectAll();
            }
        }

        private void FrmTerminal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.ConfigUpdate -= Program_ConfigUpdate;
            if (Port.IsOpen)
            {
                Port.Close();
            }
            Port.Dispose();
        }

        private void TsLblBrk_Click(object sender, EventArgs e)
        {
            if (!Port.IsOpen)
            {
                return;
            }
            //Break state is not actually a signal
            //but merely pulling the TX line high
            Port.BreakState = !Port.BreakState;
            ShowPinInfo();
        }

        private void TsLblDtr_Click(object sender, EventArgs e)
        {
            if (!Port.IsOpen)
            {
                return;
            }
            if (PrimitiveMode)
            {
                ShowPrimitiveModeMessage();
                return;
            }
            Port.DtrEnable = !Port.DtrEnable;
            Port.BreakState = false;
            ShowPinInfo();
        }

        private void TsLblRts_Click(object sender, EventArgs e)
        {
            if (!Port.IsOpen)
            {
                return;
            }
            if (PrimitiveMode)
            {
                ShowPrimitiveModeMessage();
                return;
            }
            if (!HasRts)
            {
                Port.RtsEnable = !Port.RtsEnable;
                Port.BreakState = false;
                ShowPinInfo();
            }
            else
            {
                MessageBox.Show(
                    "Port handshaking is enabled.\r\n" +
                    "This means the RTS line is under system control " +
                    "and cannot be changed or read manually.",
                    "RTS under system control",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void FrmTerminal_Load(object sender, EventArgs e)
        {
            try
            {
                Port.Open();
            }
            catch (UnauthorizedAccessException)
            {
                var Err = "The port seems to be in use. Close any window that may be using it and try opening the terminal again.";
                MessageBox.Show(
                    Err,
                    "Port busy",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                TbTerminal.Text = Err;
            }
            catch (Exception ex)
            {
                var Err = "Failed to open the port.\r\n" +
                    ex.Message;
                MessageBox.Show(
                    Err,
                    "Port failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                TbTerminal.Text = Err;
            }
            if (Port.IsOpen)
            {
                Port.BreakState = false;
                if (!HasRts)
                {
                    Port.RtsEnable = true;
                }
                Port.DtrEnable = true;
                if (Port.CDHolding || PrimitiveMode)
                {
                    Port.WriteLine("Terminal ready");
                }
                TbSend.Enabled = TbTerminal.Enabled = Port.IsOpen;
                TbSend.Focus();
            }
            ShowPinInfo();
        }

        private void TsLblMode_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "This is the serial port mode on the TRS-80 that matches your current settings.",
                "Mode string",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UnusablePin_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "This pin cannot be set by the user.\r\n" +
                "It's a pin signal that is received from the TRS-80.",
                "Pin is readonly",
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ChangeLineEnding_Click(object sender, EventArgs e)
        {
            SetLineMode(((ToolStripMenuItem)sender).Tag.ToString()); ;
        }

        #endregion
    }
}
