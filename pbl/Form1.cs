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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAddDownload_Click(object sender, EventArgs e)
        {
            new EnterURL().ShowDialog();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var sform = new setting();
            sform.FormClosed += sform_FormClosed;
            sform.ShowDialog();
        }

        void sform_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
    }
}
