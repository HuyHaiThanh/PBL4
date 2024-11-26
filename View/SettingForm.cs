using Core;
using System;
using System.Windows.Forms;

namespace View
{
    public partial class SettingForm : Form
    {
        private FolderBrowserDialog folderBrowserDialog1;

        public SettingForm()
        {
            InitializeComponent();
            folderBrowserDialog1 = new FolderBrowserDialog();

        }


        private void setting_Load(object sender, EventArgs e)
        {
            SettingInfo settingInfo = Utilities.LoadFromJson();
            if (settingInfo == null) return;
            txtSaveFolder.Text = settingInfo.pathSave;
            numericUpDown1.Value = settingInfo.connectionLimit;
            txtIP.Text = settingInfo.serverIP;
            txtPort.Text = (settingInfo.serverPort).ToString();
        }
        private void btnOpenFBD_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtSaveFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }



        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            SettingInfo settingInfo = new SettingInfo()
            {
                pathSave = txtSaveFolder.Text,
                connectionLimit = int.Parse(numericUpDown1.Value.ToString()),
                serverPort = int.Parse(txtPort.Text),
                serverIP = txtIP.Text,
            };
            Utilities.SaveToJson(settingInfo);
            this.Close();
        }

    }
}
