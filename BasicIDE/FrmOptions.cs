using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicIDE
{
    public partial class FrmOptions : Form
    {
        public FrmOptions()
        {
            InitializeComponent();
            AddItems(Tools.BaudRates, DdBaudrate);
            AddItems(Tools.StopBits, DdStopBits);
            AddItems(Tools.Parity, DdParity);

            DdBaudrate.Items.Add("Auto");

            UpdateSettings();
            Program.ConfigUpdate += UpdateSettings;
        }

        private void UpdateSettings()
        {
            SetValues(Program.Config.SerialSettings);
        }

        private void SetValues(SerialInfo Info)
        {
            if (Info.BaudRate != 0)
            {
                DdBaudrate.SelectedItem = Info.BaudRate;
            }
            else
            {
                DdBaudrate.SelectedIndex = DdBaudrate.Items.Count - 1;
            }
            DdStopBits.SelectedItem = Info.StopBits;
            DdParity.SelectedItem = Info.Parity;
            CbXON.Checked = Info.XonXoff;
            CbPrimitiveMode.Checked = Info.PrimitiveCable;
        }

        private void AddItems(IEnumerable Items, ComboBox Box)
        {
            Box.Items.Clear();
            foreach (var I in Items)
            {
                Box.Items.Add(I);
            }
        }

        private void FrmOptions_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.ConfigUpdate -= UpdateSettings;
        }

        private void BtnSerialDefaults_Click(object sender, EventArgs e)
        {
            SetValues(new SerialInfo());
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var C = Program.Config.SerialSettings;
            C.BaudRate = DdBaudrate.SelectedIndex == Tools.BaudRates.Length ? 0 : (int)DdBaudrate.SelectedItem;
            C.Parity = (string)DdParity.SelectedItem;
            C.StopBits = (int)DdStopBits.SelectedItem;
            C.XonXoff = CbXON.Checked;
            C.PrimitiveCable = CbPrimitiveMode.Checked;
            Program.SaveSettings();
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
