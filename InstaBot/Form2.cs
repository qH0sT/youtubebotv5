using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace InstaBot
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        string layk = "";
        private void button1_Click(object sender, EventArgs e)
        {
            if (((Form1)Application.OpenForms["Form1"]).checkBox3.Checked)
            {
                layk = "Like";
            }
            else if(((Form1)Application.OpenForms["Form1"]).checkBox2.Checked)
            {
                layk = "Dislike";
            }
            ((Form1)Application.OpenForms["Form1"]).dataGridView2.Rows.Add(
                File.ReadAllBytes("default.png"),
                                           "Manuel eklenmiş video",
                                           textBox1.Text,
                                           ((Form1)Application.OpenForms["Form1"]).textBox2.Text,
                                           layk
                );
            foreach (DataGridViewRow row in ((Form1)Application.OpenForms["Form1"]).dataGridView2.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.FromArgb(31, 31, 31);
                row.DefaultCellStyle.ForeColor = Color.FromArgb(78, 184, 206);
                row.DefaultCellStyle.Font = new Font("Century Schoolbook", 10, FontStyle.Bold);
                row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            layk = "";
        }
    }
}
