using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pbl
{
    public partial class setting : Form
    {
        public setting()
        {
            InitializeComponent();
            folderBrowserDialog1 = new FolderBrowserDialog(); // Khởi tạo FolderBrowserDialog

        }
        private FolderBrowserDialog folderBrowserDialog1; // Khai báo đối tượng FolderBrowserDialog

        private void setting_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtExtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void chkChrome_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtSaveFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnOpenFBD_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại chọn thư mục
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Nếu người dùng chọn thư mục, cập nhật đường dẫn vào ô textbox
                txtSaveFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }



        private void btnSaveSettings_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
