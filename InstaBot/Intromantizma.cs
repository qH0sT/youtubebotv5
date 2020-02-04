using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstaBot
{
    public partial class Intromantizma : Form
    {
        public Intromantizma()
        {
            InitializeComponent();
        }
        int j = 3;
        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text += "•";
            if (j-- == 0)
            {
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "creature" && textBox2.Text == "sweatcorse")
            {
                Form1.ch_suc = true;
                suc = true;
                if (checkBox2.Checked)
                {
                    using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\login_pin.info"))
                    {
                        sw.Write(Convert.ToBase64String(Encoding.UTF8.GetBytes(textBox1.Text + ":" + textBox2.Text)));
                    }
                }
                Size = new Size(377, 150); timer1.Start();
            }
            else
            {
                timer1.Stop();
                Size = new Size(377, 259);
                MessageBox.Show("Kullanıcı adı veya parola hatalı!", "Giriş Hatası", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //Environment.Exit(0);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            }
            else { textBox2.PasswordChar = '●'; }
        }
        bool suc = false;
        private void Intromantizma_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (suc == false)
            {
                Form1.ch_suc = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == false)
            {
                if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\login_pin.info"))
                {
                    try
                    {
                        File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\login_pin.info");
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Hata"); }
                }
            }
        }

        private void Intromantizma_Load(object sender, EventArgs e)
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\login_pin.info"))
            {
                textBox1.Text = Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\login_pin.info")[0])).Split(':')[0];
                textBox2.Text = Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\login_pin.info")[0])).Split(':')[1];
                button1.PerformClick();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
