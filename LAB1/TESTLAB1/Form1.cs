using System;
using System.Windows.Forms;

namespace TESTLAB1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGrille_Click(object sender, EventArgs e)
        {
            using (var f = new GrilleForm())
            {
                f.ShowDialog();
            }
        }

        private void btnVigenere_Click(object sender, EventArgs e)
        {
            using (var f = new VigenereForm())
            {
                f.ShowDialog();
            }
        }
    }
}
