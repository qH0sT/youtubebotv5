using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Net;
using System.Linq;
using OpenQA.Selenium.Interactions;
using System.Drawing.Drawing2D;

namespace InstaBot
{
    public partial class Form1 : Form
    {
        private bool basildi = false;
        public static bool ch_suc = false;
        public Form1()
        {
           
            InitializeComponent();
            new Intromantizma().ShowDialog();
            if (ch_suc == false)
            {
                Environment.Exit(0);
            }
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.FromArgb(144, 144, 144);//Color.FromArgb(31,31, 31);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(144, 144, 144); //Color.FromArgb(31,31, 31);
            CheckForIllegalCrossThreadCalls = false;
            comboBox2.SelectedIndex = 0;         
            Oku();
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31,31, 31);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Rockwell", 9, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(78, 184, 206);
            dataGridView1.EnableHeadersVisualStyles = false;
            //
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 31, 31);
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Rockwell", 9, FontStyle.Bold);
            dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(78, 184, 206);
            dataGridView2.EnableHeadersVisualStyles = false;
        }
        public static string Base64Sifrele(string metin)
        {
            byte[] baytlar = Encoding.UTF8.GetBytes(metin);
            return Convert.ToBase64String(baytlar);
        }
        public static string Base64Desifrele(string sifreli_metin)
        {
            byte[] sifreli_baytlar = Convert.FromBase64String(sifreli_metin);
            return Encoding.UTF8.GetString(sifreli_baytlar);
        }
        int i = 0;
        string channel_or_videos = "ytd-video-renderer";
        IWebDriver driver;
        string secili_item;
        string yrm;
        string ml;
        string sf;
        string emil;
        string pw;
        List<string> hatali_mai = new List<string>();
        List<string> hatali_sfr = new List<string>();
        public async Task Kontrollieren()
        {
            await Task.Run(async()=>{ 
            if (listView3.Items.Count > 0)
            {
                i = 0;
                await KanalDevam();
            }
            else if(listView3.Items.Count == 0)
            {
                if (listView2.Items.Count == 1)
                {
                        Invoke((MethodInvoker)delegate
                        {
                            label3.ForeColor = Color.Green;
                            label3.Text = "İşlem Bitti. " + string.Format("{0:HH:mm:ss}", DateTime.Now);
                        });
                }
                else if (listView2.Items.Count > 1)
                {
                   await Devam();

                }
                else if (listView2.Items.Count == 0)
                {
                        Invoke((MethodInvoker)delegate
                        {
                            label3.ForeColor = Color.Green;
                            label3.Text = "İşlem Bitti. " + string.Format("{0:HH:mm:ss}", DateTime.Now);            
                        });         
                }
            }
                await Task.Delay(500);
            });
        }      
        public async Task DigerVideo()
        {
            await Task.Run(async () => {
            try
            {           
                i += 1;             
                secili_item = dataGridView2.Rows[i].Cells[2].Value.ToString();             
               await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
               await YorumYap();                            
            }
            catch (Exception)
            {
                    if (checkBox11.Checked)
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            label3.ForeColor = Color.Green;
                            label3.Text = "İşlem Bitti. " + string.Format("{0:HH:mm:ss}", DateTime.Now);
                            MessageBox.Show("Dİnamik yorum özelliği açık olduğu için diğer kanallar ile devam edilmedi.","İşlem bitmiştir",MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        });
                    }
                    else
                    {
                        await Kontrollieren();
                    }              
            }
            });
        }
        public async Task DislikeBas()
        {
            await Task.Run(async() =>
            {
                await Task.Delay(2000);
                List<IWebElement> likelist = new List<IWebElement>(driver.FindElements(By.TagName("ytd-toggle-button-renderer")));
                try
                {
                    string aria_lebil = likelist[1].FindElement(By.TagName("yt-formatted-string")).GetAttribute("aria-label");
                    if (aria_lebil.Length > 0) { }

                    IWebElement button = likelist[1].FindElement(By.TagName("button"));
                    if (button.GetAttribute("aria-pressed") == "false")
                    {
                        button.Click();
                        listBox2.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "][BEĞENİLMEDİ] Video İsmi: " +
                    dataGridView2.Rows[i].Cells[1].Value.ToString() +
                    " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() +
                     " Hesap: " + listView2.Items[0].Text + " Kanal: " + SuankiKanal);
                    }
                    else
                    {
                        listBox2.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "][Zaten dislike atılmış] Video İsmi: " +
                    dataGridView2.Rows[i].Cells[1].Value.ToString() +
                    " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() +
                     " Hesap: " + listView2.Items[0].Text + " Kanal: " + SuankiKanal);
                    }
                }
                catch(Exception)
                {
                    listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "][Beğeni Ayarları Kapalı Video] Video İsmi: " +
dataGridView2.Rows[i].Cells[1].Value.ToString() +
" Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() +
" Hesap: " + listView2.Items[0].Text + " Kanal: " + SuankiKanal);
                }
            });
        }
        public async Task LikeBas()
        {                 
            await Task.Run(async() =>
            {
                await Task.Delay(2000);
              List<IWebElement> likelist = new List<IWebElement>(driver.FindElements(By.TagName("ytd-toggle-button-renderer")));
            try
            {
                string aria_lebil = likelist[1].FindElement(By.TagName("yt-formatted-string")).GetAttribute("aria-label");
                if (aria_lebil.Length > 0) { }

                IWebElement button = likelist[0].FindElement(By.TagName("button"));
                if(button.GetAttribute("aria-pressed") == "false")
                {
                    button.Click();
                        listBox2.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "][BEĞENİLDİ] Video İsmi: " +
                    dataGridView2.Rows[i].Cells[1].Value.ToString() +
                    " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() +
                     " Hesap: " + listView2.Items[0].Text + " Kanal: " + SuankiKanal);
                    }
                else
                {
                    listBox2.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "][Zaten like atılmış] Video İsmi: " +
                dataGridView2.Rows[i].Cells[1].Value.ToString() +
                " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() +
                 " Hesap: " + listView2.Items[0].Text + " Kanal: " + SuankiKanal);
                }
                }
                catch (Exception) {
                    listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "][Beğeni Ayarları Kapalı Video] Video İsmi: " +
 dataGridView2.Rows[i].Cells[1].Value.ToString() +
 " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() +
  " Hesap: " + listView2.Items[0].Text + " Kanal: " + SuankiKanal);
                }
            });
        }
       
        bool findet = true;
        public static Image CemberResim(byte[] image_bayt)
        {
            Image img;
            using (var ms = new MemoryStream(image_bayt))
            {
                img = Image.FromStream(ms);
            }
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            using (GraphicsPath gpImg = new GraphicsPath())
            {
                gpImg.AddEllipse(0, 0, img.Width, img.Height);
                using (Graphics grp = Graphics.FromImage(bmp))
                {
                    grp.Clear(Color.FromArgb(31, 31, 31));
                    grp.SetClip(gpImg);
                    grp.DrawImage(img, Point.Empty);
                }
            }
            return bmp;
        }

        private static readonly Random rnd = new Random(Guid.NewGuid().GetHashCode());
        private string KanalID()
        {
            string secilen = "";
            DataGridViewRow rw = dataGridView1.Rows[rnd.Next(0, dataGridView1.Rows.Count)];
                foreach (DataGridViewRow rovlar in dataGridView1.Rows) { rovlar.Cells[5].Value = ""; }
                rw.Cells[5].Value = "Şu anki kanal";
                rw.Selected = true; dataGridView1.Select();
                secilen = rw.Cells[4].Value.ToString();
                SuankiKanal = rw.Cells[1].Value.ToString();
            return secilen;
        }
        private async Task FarkliKanalYorum(List<IWebElement> alan, int sira_no, bool en_yeni)
        {
            try
            {  
                await Task.Run(async () =>
                {
                    string cenil = KanalID();
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
                    await Task.Delay(750);
                    driver.SwitchTo().Window(driver.WindowHandles.Last());
                    await Task.Delay(750);                                    
                        driver.Navigate().GoToUrl(cenil);
                        await Task.Delay(2000);
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.close();");
                        driver.SwitchTo().Window(driver.WindowHandles.First());
                        await Task.Delay(1000);
                        driver.Navigate().Refresh();
                        await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                        if (dataGridView2.Rows[i].Cells[4].Value.ToString() == "Like")  //Eğer like demişsek like kodlarının olduğu butona tıklıyoruz. Neden kodları direk yazmadığımı sonra açıklarım.
                        {
                            await LikeBas();
                        }
                        else if (dataGridView2.Rows[i].Cells[4].Value.ToString() == "Dislike")
                        {
                            await DislikeBas();
                        }
                        if (checkBox1.Checked)
                        {
                            await Task.Delay(1000);
                            Sakla();
                            await Task.Delay(2000);
                            IJavaScriptExecutor js2 = (IJavaScriptExecutor)driver;
                            js2.ExecuteScript("window.scrollBy(0,850);");
                            await Task.Delay(3000);
                            IJavaScriptExecutor js3 = (IJavaScriptExecutor)driver;
                            js3.ExecuteScript("window.scrollBy(0,150);");
                        }
                        else
                        {
                            IJavaScriptExecutor js2 = (IJavaScriptExecutor)driver;
                            js2.ExecuteScript("window.scrollBy(0,700);");
                        }
                        await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                    authorlar = new List<string>();
                    kime_yazdiq = new List<IWebElement>(driver.FindElements(By.TagName("a")));
                    for (int k = 0; k < kime_yazdiq.Count; k++)
                    {
                        if (kime_yazdiq[k].GetAttribute("id") == "author-text")
                        {
                            authorlar.Add(kime_yazdiq[k].Text);
                        }
                    }
                    if (en_yeni)
                    {
                        List<IWebElement> siralama = new List<IWebElement>(driver.FindElements(By.TagName("paper-button")));
                        foreach (IWebElement sirala in siralama)
                        {
                            if (sirala.GetAttribute("class") == "dropdown-trigger style-scope yt-dropdown-menu")
                            {
                                sirala.Click(); break;
                            }
                        }
                        await Task.Delay(700);
                        List<IWebElement> sira_tik = new List<IWebElement>(driver.FindElements(By.TagName("a")));
                        foreach (IWebElement tik in sira_tik)
                        {
                            if (tik.GetAttribute("class") == "yt-simple-endpoint style-scope yt-dropdown-menu")
                            {
                                tik.Click(); break;
                            }
                        }
                        await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                    }                    
                    List<IWebElement> yanitla = new List<IWebElement>(driver.FindElements(By.TagName("paper-button")));
                    for (int sago = 0; sago < yanitla.Count; sago++)
                    {
                        if (yanitla[sago].GetAttribute("class") == "style-scope ytd-button-renderer style-text size-default")
                        {
                            yanitla[sago].Click();
                        }
                    }
                    await Task.Delay(3500);
                    yanit_alani = new List<IWebElement>(driver.FindElements(By.Id("contenteditable-root")));
                    alan = new List<IWebElement>(driver.FindElements(By.Id("contenteditable-root")));        
                    foreach (char s in dataGridView2.Rows[i].Cells[3].Value.ToString())
                    {
                        if (s.ToString() == " ") { await Task.Delay(400); }
                        alan[sira_no].SendKeys(s.ToString());
                        await Task.Delay(150);
                    }
                    if (checkBox12.Checked)
                    {
                        string zaman = " " + DateTime.Now.ToString("HH:mm:ss");
                        foreach (var c in zaman)
                        {
                            alan[sira_no].SendKeys(c.ToString());
                            await Task.Delay(150);
                        }
                    }
                    await Task.Delay(100);
                });
            }
            catch (Exception) { }
        }
        List<IWebElement> kime_yazdiq;
        List<string> authorlar;
        List<IWebElement> yanit_alani;
        public async Task YorumYap()
        {         
                try
                {
                    await Task.Run(async () =>
                    {
                        label3.Text = "İşleniyor..";
                        if (checkBox11.Checked) { checkBox11.Enabled = false; }
                        secili_item = dataGridView2.Rows[i].Cells[2].Value.ToString();
                        dataGridView2.Rows[i].Selected = true; dataGridView2.Select();
                        driver.Navigate().GoToUrl(secili_item); //Seçtiğimiz videoya gönder bizi.
                        foreach(DataGridViewRow rw in dataGridView1.Rows)
                        {
                            try
                            {
                                if (rw.Cells[5].Value.ToString() == "Şu anki kanal")
                                {
                                    rw.Selected = true;
                                    dataGridView1.Select();
                                    break;
                                }
                            }
                            catch (Exception) { }
                        }
                        await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);

                        if (dataGridView2.Rows[i].Cells[4].Value.ToString() == "Like")  //Eğer like demişsek like kodlarının olduğu butona tıklıyoruz. Neden kodları direk yazmadığımı sonra açıklarım.
                        {
                            await LikeBas();
                        }
                        else if (dataGridView2.Rows[i].Cells[4].Value.ToString() == "Dislike")
                        {
                            await DislikeBas();
                        }
                        
                        if (checkBox7.Checked == false)
                        {
                            if (checkBox1.Checked)
                            {
                                await Task.Delay(1000);
                                Sakla();
                                await Task.Delay(2000);
                                IJavaScriptExecutor js2 = (IJavaScriptExecutor)driver;
                                js2.ExecuteScript("window.scrollBy(0,850);");
                                await Task.Delay(3000);
                                IJavaScriptExecutor js3 = (IJavaScriptExecutor)driver;
                                js3.ExecuteScript("window.scrollBy(0,150);");
                            }
                            else
                            {
                                IJavaScriptExecutor js2 = (IJavaScriptExecutor)driver;
                                js2.ExecuteScript("window.scrollBy(0,700);");
                            }//Yorum Alanının olduğu konuma indiriyoruz sayfayı, yoksa elementi bulamayıp hata veriyor.
                            await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);

                            try
                            {
                                IWebElement test = driver.FindElement(By.Id("message")); //Eğer yorum kapalıysa bu etiketi çekiyoruz ve stringe gönderiyoruz. eğer yorum kapalıysa string =>
                                yrm = test.Text; //Lengt'i 0 dan büyük olacak ve yorum yapmıcaz eğer 0 sa yorum açık olucak ve yorumumuzu yapıcaz.
                            }
                            catch (Exception) { yrm = string.Empty; }

                            await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);

                        if (yrm.Length == 0 || yrm != "Yorumlar kapalı.")
                        {
                            try
                            {
                                if (checkBox8.Checked)
                                {
                                    try
                                    {
                                        await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                                        if (checkBox9.Checked)
                                        {
                                            List<IWebElement> siralama = new List<IWebElement>(driver.FindElements(By.TagName("paper-button")));
                                            foreach (IWebElement sirala in siralama)
                                            {
                                                if (sirala.GetAttribute("class") == "dropdown-trigger style-scope yt-dropdown-menu")
                                                {
                                                    sirala.Click(); break;
                                                }
                                            }
                                            await Task.Delay(700);
                                            List<IWebElement> sira_tik = new List<IWebElement>(driver.FindElements(By.TagName("a")));
                                            foreach (IWebElement tik in sira_tik)
                                            {
                                                if (tik.GetAttribute("class") == "yt-simple-endpoint style-scope yt-dropdown-menu")
                                                {
                                                    tik.Click(); break;
                                                }
                                            }
                                            await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                                        }

                                        List<IWebElement> yanitla = new List<IWebElement>(driver.FindElements(By.TagName("paper-button")));
                                        for (int sago = 0; sago < yanitla.Count; sago++)
                                        {
                                            if (yanitla[sago].GetAttribute("class") == "style-scope ytd-button-renderer style-text size-default")
                                            {
                                                yanitla[sago].Click();
                                            }
                                        }
                                        await Task.Delay(3000);
                                           authorlar = new List<string>();
                                           kime_yazdiq  = new List<IWebElement>(driver.FindElements(By.TagName("a")));
                                           for(int k =0; k<kime_yazdiq.Count; k++) {
                                                if(kime_yazdiq[k].GetAttribute("id") == "author-text")
                                                {
                                                    authorlar.Add(kime_yazdiq[k].Text);
                                                }
                                            }

                                        yanit_alani = new List<IWebElement>(driver.FindElements(By.Id("contenteditable-root")));
                                        for (int sago1 = 0; sago1 < yanit_alani.Count; sago1++)
                                        {
                                            try
                                            {   
                                                    if (checkBox10.Checked)
                                                    {
                                                        if (checkBox11.Checked && sago1 !=0)
                                                        {
                                                            if (checkBox9.Checked)
                                                            {                                                              
                                                                await FarkliKanalYorum(yanit_alani, sago1, true);
                                                                await Task.Delay((int)numericUpDown2.Value * 1000);
                                                            }
                                                            else
                                                            {
                                                                await FarkliKanalYorum(yanit_alani, sago1, false);
                                                                await Task.Delay((int)numericUpDown2.Value * 1000);
                                                            }
                                                        }
                                                        else if (checkBox11.Checked == false || sago1 == 0)
                                                        {
                                                            if (sago1 % 4 == 0) { await Task.Delay(15000); }
                                                            foreach (char s in dataGridView2.Rows[i].Cells[3].Value.ToString())
                                                            {
                                                                if (s.ToString() == " ") { await Task.Delay(400); }
                                                                yanit_alani[sago1].SendKeys(s.ToString());
                                                                await Task.Delay(250);
                                                            }
                                                            if (checkBox12.Checked)
                                                            {
                                                                string zaman = " " + DateTime.Now.ToString("HH:mm:ss");
                                                                foreach (var c in zaman)
                                                                {
                                                                    yanit_alani[sago1].SendKeys(c.ToString());
                                                                    await Task.Delay(250);
                                                                }
                                                            }
                                                        }                                                  
                                                    }
                                                    else
                                                    {
                                                        yanit_alani[sago1].SendKeys(dataGridView2.Rows[i].Cells[3].Value.ToString());
                                                    }
                                                await Task.Delay(750);
                                                Actions builder = new Actions(driver);
                                                builder.SendKeys(OpenQA.Selenium.Keys.Tab).Perform();
                                                builder.SendKeys(OpenQA.Selenium.Keys.Enter).Perform();
                                                await Task.Delay((int)numericUpDown2.Value * 1000);
                                                Invoke((MethodInvoker)delegate {
                                                    listBox2.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "][Yanıtlandı] Video İsmi: " +
            dataGridView2.Rows[i].Cells[1].Value.ToString() +
            " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() +
             " Hesap: " + listView2.Items[0].Text + " Kanal: " + SuankiKanal + " Kime: " + authorlar[sago1] +" Yanıt: " + textBox2.Text);
                                                });
                                            }
                                            catch (Exception) {
                                                listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "][Yanıtlanamadı] Video İsmi: " +
            dataGridView2.Rows[i].Cells[1].Value.ToString() +
            " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() +
             " Hesap: " + listView2.Items[0].Text + " Kanal: " + SuankiKanal + " Kime: " + authorlar[sago1] + " Yanıt gönderilemedi.");
                                            }
                                        }
                                        if (yanitla.Count > 0 && yanit_alani.Count > 0)
                                        {
                                            findet = true;
                                        }
                                        else
                                        {

                                            findet = false;
                                            listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "][Yanıt Yok] Video İsmi: " +
                                        dataGridView2.Rows[i].Cells[1].Value.ToString() +
                                        " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() +
                                         " Hesap: " + listView2.Items[0].Text + " Kanal: " + SuankiKanal + " Yanıtlanıcak yorum yok.");
                                        }
                                    }
                                    catch (Exception eks)
                                    {
                                        findet = false;
                                        listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "][Yanıt Yok/Hata] Video İsmi: " +
                                        dataGridView2.Rows[i].Cells[1].Value.ToString() +
                                        " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() +
                                         " Hesap: " + listView2.Items[0].Text + " Kanal: " + SuankiKanal + " Ayrıntılar: "+eks.Message +  " Yanıtlanıcak yorum yok. Veya bir hata oluştu.");
                                    }

                                }
                                    await Task.Delay(4000);
                                    //rastgele_yorum.Clear();
                                IWebElement webt = driver.FindElement(By.Id("placeholder-area"));
                                if (checkBox8.Checked && findet == true)
                                {
                                    Actions actions = new Actions(driver);
                                    actions.MoveToElement(webt);
                                    actions.Perform();
                                    await Task.Delay(500);
                                        IJavaScriptExecutor js2 = (IJavaScriptExecutor)driver;
                                        js2.ExecuteScript("window.scrollBy(0,50);");
                                    }
                                webt.Click();
                                await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                                IWebElement web = driver.FindElement(By.Id("contenteditable-root")); //Ve yoruma açılmış olan alana yorumumuzu gönderiyoruz.
                                    if (checkBox10.Checked)
                                    {
                                        foreach (char c in dataGridView2.Rows[i].Cells[3].Value.ToString())
                                        {
                                            if(c.ToString() == " ") { await Task.Delay(500); }
                                            web.SendKeys(c.ToString());
                                            await Task.Delay(300);
                                        }
                                        if (checkBox12.Checked)
                                        {
                                            string zaman = " " + DateTime.Now.ToString("HH:mm:ss");
                                            foreach (var c in zaman)
                                            {
                                                web.SendKeys(c.ToString());
                                                await Task.Delay(250);
                                            }
                                        }
                                    }
                                    else { web.SendKeys(dataGridView2.Rows[i].Cells[3].Value.ToString()); }
                                    await Task.Delay(100);
                                IWebElement elemente_ = driver.FindElement(By.Id("submit-button"));
                                elemente_.Click();
                                findet = true;
                                    listBox2.Items.Add("["+ DateTime.Now.ToString("HH:mm:ss") + "][Normal Yorum] Video İsmi: " + dataGridView2.Rows[i].Cells[1].Value.ToString() + " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() + " Hesap: " + listView2.Items[0].Text + " Kanal: " + SuankiKanal +
                                        " Yapılan Yorum: " + dataGridView2.Rows[i].Cells[3].Value);
                                await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                                await DigerVideo();
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.Message + "\n" + ex.StackTrace + "\n" + ex.GetBaseException(),"alt taraf");
                                listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "]Yorum Yapılamadı - Video İsmi: " + dataGridView2.Rows[i].Cells[1].Value.ToString() + " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() + " Hesap: " + listView2.Items[0].Text + " Hata Kodu: "
                                + ex.Message);
                                await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                                await DigerVideo();
                            }
                        }
                         else if (yrm.Length > 0 && yrm == "Yorumlar kapalı.")
                            {

                                listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "]Yorum Kapalı Video - İsmi: " + dataGridView2.Rows[i].Cells[1].Value.ToString() + " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() + " Hesap: " + listView2.Items[0].Text);
                                await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                                await DigerVideo();

                            }
                        }
                        else
                        {
                            //listBox2.Items.Add("Video İsmi: " + dataGridView2.Rows[i].Cells[1].Value.ToString() + " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() + " Hesap: " + listView2.Items[0].Text);
                            await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                            await DigerVideo();
                        }

                    });

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message + "\n" + ex.StackTrace + "\n"  +ex.GetBaseException(),"en alt");
                  listBox1.Items.Add("[" + DateTime.Now.ToString("HH:mm:ss") + "]Genel Hata - Video İsmi: " + dataGridView2.Rows[i].Cells[1].Value.ToString() + " Link: " + dataGridView2.Rows[i].Cells[2].Value.ToString() + " Hesap: " +
                    listView2.Items[0].Text + " Açıklama: " + ex.Message);
                    await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                    await DigerVideo();
                }
        }
        private async void button3_Click(object sender, EventArgs e)
        {
            await Task.Run(async() => { 
            if ( dataGridView2.Rows.Count != 0 && button6.Enabled == false)
            {
                    Invoke((MethodInvoker)delegate
                    {

                        button10.Enabled = false;
                        button6.Enabled = false;
                        checkBox11.Enabled = false;
                        checkBox6.Checked = false;
                        button12.Enabled = false;
                        contextMenuStrip1.Enabled = false;
                        button3.Enabled = false;
                        button8.Enabled = false;
                        listBox1.Items.Clear();
                        listBox2.Items.Clear();
                    });
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                        label3.Text = "İşlem sürüyor..";
                        await YorumYap();
                }
                else
                {
                    MessageBox.Show("video ismini veya yorum kısmını boş bırakmayınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Listede en az bir video olmalı ve oturum açmalısınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            });
        }
        private void seçToolStripMenuItem_Click(object sender, EventArgs e) //İstediğimiz videoyu veya videoları listeden kaldırıyoruz.
        {
            foreach (DataGridViewRow itemler in dataGridView2.SelectedRows)
            {
               
                dataGridView2.Rows.Remove(itemler);
            }
            label6.Text = dataGridView2.Rows.Count.ToString();
          
        }
        private void button4_Click(object sender, EventArgs e) //Hangi videoda like veya dislike atacağımızı bu eventhandler'da belirliyoruz.
        {
            if (dataGridView2.Rows.Count == 0)
            {
                MessageBox.Show("Lütfen listeden bir video seç.","Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
            else
            {
                dataGridView2.Rows[i].Cells[3].Value = textBox2.Text;
                if (radioButton1.Checked)
                {
                    dataGridView2.Rows[i].Cells[4].Value = "Like";
                }
                else
                    dataGridView2.Rows[i].Cells[4].Value = "Dislike";
            }
        }     
        public async Task YotubedaAra()
        {
            await Task.Run(async() => { 
            if (comboBox2.SelectedIndex > -1)
            {

                switch (comboBox2.SelectedIndex)
                {

                    case 0:
                            try
                            {
                                Invoke((MethodInvoker)delegate { 
                                    dataGridView2.Rows.Clear();
                                });
                                if (driver.Url.ToString() != "https://www.youtube.com/")
                                    {
                                        driver.Navigate().GoToUrl("https://www.youtube.com/");
                                    }

                                    IWebElement elemente = driver.FindElement(By.Id("search"));
                                    elemente.SendKeys(textBox1.Text);

                                    IWebElement elementen = driver.FindElement(By.Id("search-icon-legacy"));
                                    elementen.Click();

                                    await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);

                                    List<IWebElement> elements = new List<IWebElement>(driver.FindElements(By.TagName(channel_or_videos)));

                                    for (int j = 0; j < elements.Count; j++)
                                    {
                                        try
                                        {
                                            string thumb_uri = elements[j].FindElement(By.Id("thumbnail")).FindElement(By.Id("img")).GetAttribute("src");
                                            Invoke((MethodInvoker)delegate
                                            {

                                                dataGridView2.Rows.Add(
                                                new WebClient().DownloadData(thumb_uri.Substring(0, thumb_uri.IndexOf("?"))),
                                                elements[j].FindElement(By.Id("video-title")).Text,
                                                elements[j].FindElement(By.Id("video-title")).GetAttribute("href"),
                                                "",
                                                "");
                                              
                                            });

                                        }
                                        catch (Exception) { }
                                    }
                                    
                                    Invoke((MethodInvoker)delegate
                                    {
                                        button4.Enabled = true;
                                        foreach (DataGridViewRow row in dataGridView2.Rows)
                                        {
                                            row.DefaultCellStyle.BackColor = Color.FromArgb(31,31, 31);
                                            row.DefaultCellStyle.ForeColor = Color.FromArgb(78, 184, 206);
                                            row.DefaultCellStyle.Font = new Font("Century Schoolbook", 10, FontStyle.Bold);
                                            row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                            row.Cells[3].Value = textBox2.Text;
                                            if (checkBox3.Checked)
                                            {
                                                row.Cells[4].Value = "Like";
                                            }
                                            else if (checkBox2.Checked)
                                            {
                                                row.Cells[4].Value = "Dislike";
                                            }
                                            if (row.Cells[1].Value.ToString() == "" ||
                                               row.Cells[2].Value.ToString() == "")
                                            {
                                                dataGridView2.Rows.Remove(row);

                                            }

                                        }
                                        label6.Text = dataGridView2.Rows.Count.ToString();
                                        button3.Enabled = true;
                                    });
                                    
                                    if (checkBox6.Checked && basildi == false)
                                    {
                                        await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                                        await YorumYap();
                                        basildi = true;

                                    }
                            }
                            catch (Exception) { }

                        break;

                    case 1:
                        try
                        {
                                Invoke((MethodInvoker)delegate
                                {
                                    dataGridView2.Rows.Clear();
                                });

                                if (textBox1.Text.Contains("/channel/") == false)
                                {
                                    driver.Navigate().GoToUrl("https://www.youtube.com/results?search_query=" + textBox1.Text);
                                    await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                                    IWebElement elementex = driver.FindElement(By.XPath("//*[@id='contents']/ytd-channel-renderer/a"));
                                    driver.Navigate().GoToUrl(elementex.GetAttribute("href") + "/videos");
                                }
                                else
                                {
                                    driver.Navigate().GoToUrl(textBox1.Text);
                                }
                                await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);

                                List<IWebElement> element = new List<IWebElement>(driver.FindElements(By.TagName("ytd-grid-video-renderer")));

                                for (int j = 0; j < element.Count; j++)
                                {
                                    try { 
                                    string thumb_uri = element[j].FindElement(By.Id("thumbnail")).FindElement(By.Id("img")).GetAttribute("src");
                                        Invoke((MethodInvoker)delegate
                                        {
                                            dataGridView2.Rows.Add(
                                            new WebClient().DownloadData(thumb_uri.Substring(0, thumb_uri.IndexOf("?"))),
                                            element[j].FindElement(By.Id("video-title")).Text,
                                            element[j].FindElement(By.Id("video-title")).GetAttribute("href"),
                                            "",
                                            ""
                                            );
                                        });
                                    }
                                    catch (Exception) { }
                                }
                                Invoke((MethodInvoker)delegate
                                {
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        row.DefaultCellStyle.BackColor = Color.FromArgb(31,31, 31);
                                        row.DefaultCellStyle.ForeColor = Color.FromArgb(78, 184, 206);
                                        row.DefaultCellStyle.Font = new Font("Century Schoolbook", 10, FontStyle.Bold);
                                        row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                        row.Cells[3].Value = textBox2.Text;
                                        if (checkBox3.Checked)
                                        {
                                            row.Cells[4].Value = "Like";
                                        }
                                        else if (checkBox2.Checked)
                                        {
                                            row.Cells[4].Value = "Dislike";
                                        }
                                        if (row.Cells[1].Value.ToString() == "" || row.Cells[2].Value.ToString() == "")
                                        {
                                            dataGridView2.Rows.Remove(row);

                                        }
                                    }
                                    label6.Text = dataGridView2.Rows.Count.ToString();
                                    button3.Enabled = true;
                                });
                               
                           
                            if (checkBox6.Checked)
                            {
                               await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                               await YorumYap();                         
                            }
                        }
                        catch (Exception) { }
                        break;
                }
            }
            else
                MessageBox.Show("Lütfen video veya kanal seçin", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          });
        }
        string SuankiKanal = "Ana Kanal";
        public async Task KanalDevam()
        {
            await Task.Run(async() => { 
            await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
            SuankiKanal = listView3.Items[0].Text;
            channel[0] = SuankiKanal+"|"+listView3.Items[0].SubItems[1].Text;
            driver.Navigate().GoToUrl(listView3.Items[0].SubItems[1].Text);
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value.ToString() == listView3.Items[0].Text)
                    {
                        foreach(DataGridViewRow r in dataGridView1.Rows) { r.Cells[5].Value = ""; }
                        row.Cells[5].Value = "Şu anki kanal";
                        row.Selected = true;
                        dataGridView1.Select();
                        break;
                    }
                }
            listView3.Items.Remove(listView3.Items[0]);
            await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
            if (dataGridView2.Rows.Count == 0)
            {
                    await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                    await YotubedaAra();
            }
            else
            {
                if (button3.Enabled == false)
                {
                        await YorumYap();
                }
                else if (checkBox6.Checked)
                    {
                        await YorumYap(); //eğer manuel video varsa ve oto. mod açıksa direkt yoruma git.
                    }
            }
            });
        }
        public async Task KanalCek()
        {
            await Task.Run(async() =>
            {
                button8.Enabled = false;
                Invoke((MethodInvoker)delegate { dataGridView1.Rows.Clear(); listView3.Items.Clear(); });
                driver.Navigate().GoToUrl("https://www.youtube.com/channel_switcher");
                await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                List<IWebElement> element2 = new List<IWebElement>(driver.FindElements(By.TagName("a")));
                foreach (IWebElement isimler in element2)
                {
                    Invoke((MethodInvoker)delegate {
                    //Karikatür komedya koydum adını buranın.               
                    ListViewItem lvi = new ListViewItem();
                    if (isimler.GetAttribute("href").Contains("feature=channel_switcher"))
                    {
                            dataGridView1.Rows.Add(CemberResim(new WebClient().DownloadData(isimler.FindElement(
                             By.TagName("img")).GetAttribute("src"))),

                              isimler.Text.Split(new[] { Environment.NewLine },
                                   StringSplitOptions.None)[0],
                              isimler.Text.Split(new[] { Environment.NewLine },
                                   StringSplitOptions.None)[1],
                              isimler.Text.Split(new[] { Environment.NewLine },
                                   StringSplitOptions.None)[2],
                              isimler.GetAttribute("href"),
                              ""
                              );
                       //seçil ve seç civarlarında tek mi kaldı Kajmeran?
                        dataGridView1.Rows[0].Selected = true;
                            dataGridView1.Rows[0].Cells[5].Value = "Şu anki kanal";
                        lvi.Text = isimler.Text.Split(new[] { Environment.NewLine },
                                   StringSplitOptions.None)[0];
                        lvi.SubItems.Add(isimler.GetAttribute("href"));
                    }
                    listView3.Items.Add(lvi);
                    foreach(DataGridViewRow row in dataGridView1.Rows)
                    {
                        row.DefaultCellStyle.BackColor = Color.FromArgb(31,31, 31);
                        row.DefaultCellStyle.ForeColor = Color.FromArgb(78, 184, 206);
                        row.DefaultCellStyle.Font = new Font("Century Schoolbook", 10, FontStyle.Bold);
                        row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                    
                    foreach (ListViewItem ll in listView3.Items)
                    {
                        if (ll.Text == "" || ll.SubItems[1].Text == "")
                        {
                            listView3.Items.Remove(ll);
                        }
                    }
                  });
                }
                if (dataGridView1.Rows.Count == 1) { checkBox11.Checked = false; }
                await Task.Delay(3000);
                await KanalDevam();
            });
        }
        public async Task Baslat()
        {
            await Task.Run(async() => { 
            if (listView2.Items.Count == 0)
            {
                MessageBox.Show("Lütfen en az bir tane hesap ekleyin.", "Hesap Yok", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                try
                {
                        Invoke((MethodInvoker)delegate
                        {
                            label3.Text = "İşlem başladı..";
                            button4.Enabled = true;
                            button10.Enabled = true;
                            button6.Enabled = false;
                            button3.Enabled = true;
                            button8.Enabled = true;
                        });
                        driver = new ChromeDriver();
                        driver.Manage().Window.Maximize(); //Pencereyi tam ekran yapıyoruz.                       
                        driver.Navigate().GoToUrl("https://accounts.google.com/ServiceLogin");
                       
                    IWebElement webte = driver.FindElement(By.Id("identifierId"));
                    webte.SendKeys(listView2.Items[0].Text);
                    IWebElement ww = driver.FindElement(By.Id("identifierNext"));
                    ww.Click();
                   await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                    try
                    {
                        IWebElement element = driver.FindElement(By.ClassName("o6cuMc"));
                        ml = element.Text;
                    }
                    catch (Exception) { ml = string.Empty; }

                    if (ml.Length == 0)
                    {

                        await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);

                        IWebElement webt = driver.FindElement(By.XPath("//*[@id='password']/div[1]/div/div[1]/input"));
                        webt.SendKeys(Base64Desifrele(listView2.Items[0].SubItems[1].Text));

                        IWebElement wwe = driver.FindElement(By.Id("passwordNext"));
                        wwe.Click();
                        await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                        try
                        {
                            IWebElement element = driver.FindElement(
                           By.CssSelector("#view_container > div > div > div.pwWryf.bxPAYd > div > div.WEQkZc > div > form > span > section > div > div > div.Xk3mYe.VxoKGd.Jj6Lae > div.xgOPLd.uSvLId > div:nth-child(2)"));
                            sf = element.Text;
                        }
                        catch (Exception) { sf = string.Empty; }


                        if (sf.Length == 0)
                        {

                            if (checkBox6.Checked)
                            {
                                    await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                                    Invoke((MethodInvoker)delegate
                                    {                                       
                                        button6.Enabled = false;
                                        button10.Enabled = false;
                                        button3.Enabled = false;
                                        button8.Enabled = false;
                                    });
                                    await KanalCek();
                            }
                        }
                        else
                        {


                            StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\Hatali_sifre.txt");
                            hatali_sfr.Add(listView2.Items[0].Text + " Şifre: " + Base64Desifrele(listView2.Items[0].SubItems[1].Text));
                            foreach (string isim_bulamadim in hatali_sfr)
                            {
                                sw.WriteLine(isim_bulamadim);
                            }
                            sw.Flush();
                            sw.Close();
                            driver.Quit();
                            listView2.Items.Remove(listView2.Items[0]);
                            if (listView2.Items.Count >= 1)
                            {
                                    await Baslat();
                              
                            }
                        }

                    }
                    else
                    {

                        StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\Hatali_mailler.txt");
                        hatali_mai.Add(listView2.Items[0].Text + " Şifre: " + Base64Desifrele(listView2.Items[0].SubItems[1].Text));
                        foreach (string dgsken_ismi_bulamiyorum in hatali_mai)
                        {
                            sw.WriteLine(dgsken_ismi_bulamiyorum);
                        }
                        sw.Flush();
                        sw.Close();
                        driver.Quit();
                        listView2.Items.Remove(listView2.Items[0]);
                        if (listView2.Items.Count >= 1)
                        {
                                await Baslat();

                       }
                    }

                }
                catch (Exception ex) { MessageBox.Show("Lütfen botu yeniden başlatın: "+ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
           });
        }      
        private async void button6_Click(object sender, EventArgs e)
        {
            await Baslat();
            
        }

        public async Task EkVideoEkle()
        {
            await Task.Run(() => { 
            try
            {             
                    bool dvm = MessageBox.Show("Yeni videolar görene kadar sayfayı aşağı indirin, hazır olunca Tamam'a basın. Vazgeçmek isterseniz İptal'e basın.", "Uyarı", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK;
                    if (dvm)
                    {
                        List<IWebElement> element = new List<IWebElement>(driver.FindElements(By.TagName(channel_or_videos)));

                        for (int j = 0; j < element.Count; j++)
                        {
                            try { 
                            string thumb_uri = element[j].FindElement(By.Id("thumbnail")).FindElement(By.Id("img")).GetAttribute("src");
                                Invoke((MethodInvoker)delegate
                                {

                                bool bulundu = false;
                                    foreach (DataGridViewRow row in dataGridView2.Rows)
                                    {
                                        if (row.Cells[2].Value.ToString() == element[j].FindElement(By.Id("video-title")).GetAttribute("href"))
                                        {
                                            bulundu = true;
                                            break;
                                        }
                                    }
                                    if (bulundu == false)
                                    {
                                        dataGridView2.Rows.Add(
                            new WebClient().DownloadData(thumb_uri.Substring(0, thumb_uri.IndexOf("?"))),
                            element[j].FindElement(By.Id("video-title")).Text,
                            element[j].FindElement(By.Id("video-title")).GetAttribute("href"),
                            "",
                            ""
                            );
                                    }
                                });
                            }
                            catch (Exception) { }
                        }

                        Invoke((MethodInvoker)delegate
                        {
                            foreach (DataGridViewRow row in dataGridView2.Rows)
                            {
                                row.DefaultCellStyle.BackColor = Color.FromArgb(31,31, 31);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(78, 184, 206);
                                row.DefaultCellStyle.Font = new Font("Century Schoolbook", 10, FontStyle.Bold);
                                row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                row.Cells[3].Value = textBox2.Text;
                                if (checkBox3.Checked)
                                {
                                    row.Cells[4].Value = "Like";
                                }
                                else if (checkBox2.Checked)
                                {
                                    row.Cells[4].Value = "Dislike";
                                }
                                if (row.Cells[1].Value.ToString() == "" || row.Cells[2].Value.ToString() == "")
                                {
                                    dataGridView2.Rows.Remove(row);

                                }
                            }
                            label6.Text = dataGridView2.Rows.Count.ToString();
                        });
                    }
            }
            catch (Exception) { MessageBox.Show("Lütfen ilk önce video çekme işlemini yapınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); };
            });
        }
        private async void button10_Click(object sender, EventArgs e)
        {
            await EkVideoEkle();
           
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                for (int n = 0; n < dataGridView2.Rows.Count; n++)
                {
                    dataGridView2.Rows[n].Cells[4].Value = "Like";
                }
              
            }
            else if (checkBox2.Checked)
            {
                for (int n = 0; n < dataGridView2.Rows.Count; n++)
                {
                    dataGridView2.Rows[n].Cells[3].Value = "Dislike";
                }
            }
        }   
        private void button12_Click(object sender, EventArgs e)
        {
            string e_posta = Microsoft.VisualBasic.Interaction.InputBox("E-Mail", "Bilgi Girişi", "Gmail hesabınız", -1, -1);
            
                string sifre = Microsoft.VisualBasic.Interaction.InputBox("Şifre", "Bilgi Girişi", "Şifrenizi yazın", -1, -1);


                if (!string.IsNullOrEmpty(e_posta) || !string.IsNullOrEmpty(sifre))
                {
                    ListViewItem lvi = new ListViewItem(e_posta);
                    lvi.SubItems.Add(Base64Sifrele(sifre));
                    listView2.Items.Add(lvi);
                }
            
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.SelectedItems)
            {
                listView2.Items.Remove(item);
            }
            e_mail_kayit(listView2,"|");
        }
        private void e_mail_kayit(ListView lv, string splitter)
        {
            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory.ToString() + "\\e-postalar.txt"))
            {
                foreach (ListViewItem item in lv.Items)
                {
                    try
                    {
                        sw.WriteLine("{0}{1}{2}", item.SubItems[0].Text, splitter, item.SubItems[1].Text);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        public void Oku()
        {
            try
            {
                var lines = File.ReadAllLines(Environment.CurrentDirectory.ToString() + "\\e-postalar.txt");
                foreach (string line in lines)
                {
                    string[] fileItems = line.Split('|');
                    ListViewItem lv = new ListViewItem();
                    lv.Text = fileItems[0].ToString();
                    lv.SubItems.Add(fileItems[1].ToString());
                    listView2.Items.Add(lv);
                }
            }
            catch (Exception)
            {

            }
        }        
        private void label3_TextChanged(object sender, EventArgs e)
        {
            if (label3.Text.Contains("İşlem Bitti"))
            {
                label3.ForeColor = Color.Green;
               
                using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\LoglarYTBOT.txt"))
                {
                    foreach (string hatalar in listBox1.Items)
                    {                
                        sw.WriteLine("Başarısız İşlemler: "+hatalar);
                    }
                    sw.WriteLine(Environment.NewLine);
                    foreach (string basarili in listBox2.Items)
                    {
                        sw.WriteLine("Başarılı İşlemler: "+basarili);                     
                    }
                    sw.WriteLine(Environment.NewLine);
                    for (int d = 0; d < dataGridView2.Rows.Count; d++)
                    {
                        sw.WriteLine(d.ToString()+"-)Videolar: "+dataGridView2.Rows[d].Cells[1].Value.ToString());
                    }
                    sw.WriteLine(Environment.NewLine);
                    foreach (string c in hatali_mai)
                    {
                        sw.WriteLine("Yanlış Mail: "+ c);
                    }
                    sw.WriteLine(Environment.NewLine);
                    foreach (string x in hatali_sfr)
                    {
                        sw.WriteLine("Yanlış Şifre: " + x);
                    }
                }
                Invoke((MethodInvoker)delegate
                {               
                    listView2.Items.Clear();
                    button3.Enabled = false;
                    button10.Enabled = false;
                    button12.Enabled = true;
                    checkBox11.Enabled = true;
                    button6.Enabled = true;
                    button8.Enabled = false;
                    contextMenuStrip1.Enabled = true;
                    dataGridView2.Rows.Clear();
                    dataGridView1.Rows.Clear();
                    i = 0;
                    label6.Text = "0";
                    SuankiKanal = "Ana Kanal";
                    hatali_mai.Clear();
                    hatali_sfr.Clear();
                    driver.Quit();
                });
                if (checkBox5.Checked)
                {
                  Process.Start("shutdown.exe", "-s");                
                }
            }
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (checkBox4.Checked)
                {
                    foreach (DataGridViewRow dr in dataGridView2.Rows)
                    {
                        dr.Cells[3].Value = textBox2.Text;
                    }
                }
                else
                    foreach (DataGridViewRow dr in dataGridView2.Rows)
                    {
                        dr.Cells[3].Value = "";
                    }
            });
        }
        private void listeyiTemizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                dataGridView2.Rows.Clear();
                label6.Text = (0).ToString();
            });
        }
        private void listeyiTemizleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (listBox2.Items.Count > 0 || listBox1.Items.Count > 0)
                {
                    if (!File.Exists("LoglarYTBOT.txt"))
                    {
                        using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\LoglarYTBOT.txt"))
                        {
                            foreach (string hatalar in listBox1.Items)
                            {
                                sw.WriteLine("Başarısız İşlemler: " + hatalar);
                            }
                            sw.WriteLine(Environment.NewLine);
                            foreach (string basarili in listBox2.Items)
                            {
                                sw.WriteLine("Başarılı İşlemler: " + basarili);
                            }
                            sw.WriteLine(Environment.NewLine);
                            for (int d = 0; d < dataGridView2.Rows.Count; d++)
                            {
                                sw.WriteLine(d.ToString() + "-)Videolar: " + dataGridView2.Rows[d].Cells[1].Value.ToString());
                            }
                            sw.WriteLine(Environment.NewLine);
                            foreach (string c in hatali_mai)
                            {
                                sw.WriteLine("Yanlış Mail: " + c);
                            }
                            sw.WriteLine(Environment.NewLine);
                            foreach (string x in hatali_sfr)
                            {
                                sw.WriteLine("Yanlış Şifre: " + x);
                            }
                        }
                    }
                }
                if (driver != null)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        driver.Quit();
                    });
                }
            }
            catch (Exception) { }
        }
        public async Task Devam()
        {
            await Task.Run(async () => {            
            listView2.Items.Remove(listView2.Items[0]);
            i = 0;
            driver.Navigate().GoToUrl("https://mail.google.com/mail/logout?hl=en");
            await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
            driver.Navigate().GoToUrl("https://accounts.google.com/ServiceLogin/signinchooser?continue=https%3A%2F%2Fmail.google.com%2Fmail%2F&osid=1&service=mail&ss=1&ltmpl=default&rm=false&flowName=GlifWebSignIn&flowEntry=AddSession");          
            try
            {
                IWebElement ed = driver.FindElement(By.ClassName("eARute"));
                ed.Click();
            }
            catch (Exception) { }              
            await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);             
            IWebElement webte = driver.FindElement(By.Id("identifierId"));
            webte.SendKeys(listView2.Items[0].Text);
            IWebElement ww = driver.FindElement(By.Id("identifierNext"));
            ww.Click();
            await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
            try
            {
                IWebElement element = driver.FindElement(By.ClassName("o6cuMc")); //email hatası var mı
                emil = element.Text;
            }
            catch (Exception) { emil = string.Empty; }
            await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
            if (emil.Length == 0) //eğer hata yoksa
            {
                IWebElement webt = driver.FindElement(By.XPath("//*[@id='password']/div[1]/div/div[1]/input")); //emailde hata yoksa şifreye geç
                webt.SendKeys(Base64Desifrele(listView2.Items[0].SubItems[1].Text));
                IWebElement wwe = driver.FindElement(By.Id("passwordNext"));
                wwe.Click();
                try
                {
                        IWebElement element = driver.FindElement(
                               By.CssSelector("#view_container > div > div > div.pwWryf.bxPAYd > div > div.WEQkZc > div > form > span > section > div > div > div.Xk3mYe.VxoKGd.Jj6Lae > div.xgOPLd.uSvLId > div:nth-child(2)"));
                        pw = element.Text;
                    }
                catch (Exception) { pw = string.Empty; }
                if (pw.Length == 0) //eğer şifrede hata yoksa
                {

                        await Task.Delay(Convert.ToInt32(numericUpDown1.Value) * 1000);
                        await KanalCek();
                  
                }
                else if (pw.Length != 0)
                {
                    if(listView2.Items.Count >1)
                    {
                            await Devam();
                    }
                    else if (listView2.Items.Count == 1)
                    {
                        label3.Text = "İşlem Bitti. " + string.Format("{0:HH:mm:ss}", DateTime.Now);
                    }
                   
                    hatali_sfr.Add(listView2.Items[0].Text);                  
                }             
            }
            else if (emil.Length != 0)
            {
                if (listView2.Items.Count > 1)
                {
                    await Devam();
                }
                else if (listView2.Items.Count == 1)
                {
                    label3.Text = "İşlem Bitti. " + string.Format("{0:HH:mm:ss}", DateTime.Now);
                }
                
                    hatali_mai.Add(listView2.Items[0].Text);
          }
            });
        }    
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                comboBox2.SelectedIndex = 0;
                button3.Enabled = false;
                button8.Enabled = false;
            }
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Oku();
        }
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            e_mail_kayit(listView2, "|");
        }     
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                textBox2.Enabled = false;
                checkBox3.Checked = true;
                checkBox8.Checked = false;
                checkBox8.Enabled = false;
                checkBox4.Enabled = false;
                checkBox1.Checked = false;
                checkBox1.Enabled = false;
            }
            else
            {
                textBox2.Enabled = true;
                checkBox3.Checked = false;
                checkBox8.Enabled = true;
                checkBox8.Checked = true;
                checkBox4.Enabled = true;
                checkBox1.Enabled = true;
                checkBox1.Checked = true;
            }
        }
        private async void button8_Click(object sender, EventArgs e)
        {
            await KanalCek();
            label3.Text = "Kanallar çekildi.";
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView3.SelectedItems)
            {
                listView3.Items.Remove(item);
            }
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            listView3.Items.Clear();
        }
        public void Sakla()
        {
           
                try
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;  //Ana video
                    js.ExecuteScript(
                    @"document.getElementById('player-container').remove();"
                    );
                     
                    js.ExecuteScript(
                    @"document.getElementById('related').remove();"
                    );
                }
                catch (Exception) { }
           
        }
        private void dışaAktarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\LogHATALAR.txt"))
            {
                foreach (string hatalar in listBox1.Items)
                {
                    sw.WriteLine(hatalar);
                }
            }
        }
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\LogBAŞARILI.txt"))
            {
                foreach (string hatalar in listBox2.Items)
                {
                    sw.WriteLine(hatalar);
                }
            }
        }
        string layk = "";
        private void içeAktarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "Metin Belgesi (.txt) | *.txt";
                if (op.ShowDialog() == DialogResult.OK)
                {
                    var lines = File.ReadAllLines(op.FileName);
                    foreach (string line in lines)
                    {                        
                        Invoke((MethodInvoker)delegate
                        {
                            bool bulundu = false;
                            foreach (DataGridViewRow row in dataGridView2.Rows)
                            {
                                if (row.Cells[2].Value.ToString() == line)
                                {
                                    bulundu = true;
                                    break;
                                }
                            }
                            if (bulundu == false)
                            {
                                if (checkBox3.Checked) { layk = "Like"; }else if (checkBox2.Checked) { layk = "Dislike"; }
                                dataGridView2.Rows.Add(File.ReadAllBytes("default.png"),
                                           "Manuel eklenmiş video",
                                           line,
                                           textBox2.Text,
                                           layk
                                           );
                                layk = "";
                            }
                            foreach (DataGridViewRow row in dataGridView2.Rows)
                            {
                                row.DefaultCellStyle.BackColor = Color.FromArgb(31, 31, 31);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(78, 184, 206);
                                row.DefaultCellStyle.Font = new Font("Century Schoolbook", 10, FontStyle.Bold);
                                row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            }
                        });
                    }
                    button4.Enabled = true;
                }
                label6.Text = dataGridView2.Rows.Count.ToString();
            }
            catch (Exception) {  }          
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
            if (checkBox3.Checked)
            {
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        row.Cells[4].Value = "Like";                       
                    }
                    checkBox2.Checked = false;
                }
            else
            {
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        row.Cells[4].Value = "";
                    }

                }

            });
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                if (checkBox2.Checked)
                {
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        row.Cells[4].Value = "Dislike";
                    }
                    checkBox3.Checked = false;
                }
                else
                {
                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        row.Cells[4].Value = "";
                    }

                }

            });
        }
        string[] channel = { "" };

        private async Task SwitchChannel(bool isDelete)
        {
            await Task.Run(() => { 
            if (isDelete == false)
            {                  
                    if (dataGridView1.SelectedRows[0].Cells[5].Value.ToString() == "")
                    {
                        driver.Navigate().GoToUrl(dataGridView1.SelectedRows[0].Cells[4].Value.ToString());
                        foreach (DataGridViewRow kontrolieren in dataGridView1.Rows)
                        {
                            kontrolieren.Cells[5].Value = "";
                        }
                        dataGridView1.SelectedRows[0].Cells[5].Value = "Şu anki kanal";
                        SuankiKanal = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                        IEnumerable<ListViewItem> dtgr = listView3.Items.Cast<ListViewItem>().Where(b => b.Text ==
                        dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
                        if (dtgr.Count() > 0)
                        {
                            dtgr.FirstOrDefault().Remove();
                        }
                        ListViewItem lvi = new ListViewItem(channel[0].Split('|')[0]);
                        lvi.SubItems.Add(channel[0].Split('|')[1]);
                        if (!listView3.Items.Contains(lvi))
                        {
                            listView3.Items.Add(lvi);
                        }

                        channel[0] = dataGridView1.SelectedRows[0].Cells[1].Value.ToString() + "|"
                            + dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Zaten bu kanal ile işlem yapılacak.", "Uyarı", MessageBoxButtons.OK,
               MessageBoxIcon.Exclamation);
                    }
            }
            else
            {              
                        IEnumerable<ListViewItem> dtgr = listView3.Items.Cast<ListViewItem>().Where(b => b.Text ==
                        dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
                        if (dtgr.Count() == 0)
                        {
                        if (button3.Enabled)
                        {
                            int j = new Random().Next(0, dataGridView1.Rows.Count - 1);
                            dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                            SuankiKanal = dataGridView1.Rows[j].Cells[1].Value.ToString();
                            dataGridView1.Rows[j].Cells[5].Value = "Şu anki kanal";
                            driver.Navigate().GoToUrl(dataGridView1.Rows[j].Cells[4].Value.ToString());
                            channel[0] = dataGridView1.Rows[j].Cells[1].Value.ToString() + "|"
                             + dataGridView1.Rows[j].Cells[4].Value.ToString();
                            dataGridView1.Rows[j].Selected = true; dataGridView1.Select();
                            IEnumerable<ListViewItem> sil = listView3.Items.Cast<ListViewItem>().Where(b => b.Text ==
                            dataGridView1.Rows[j].Cells[1].Value.ToString());
                            if (sil.Count() > 0)
                            {
                                sil.FirstOrDefault().Remove();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Bot çalışıyorken bu işlem yapılamaz.", "Uyarı", MessageBoxButtons.OK,
              MessageBoxIcon.Exclamation);
                        }
                        }
                        else
                        {
                            dtgr.FirstOrDefault().Remove();
                            dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                        }               
            }
            });
        }
        private async void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (button3.Enabled)
                {
                    if (dataGridView1.Rows.Count > 0)
                        await SwitchChannel(false);
                }
                else
                {
                    MessageBox.Show("Bot çalışıyorken bu işlem yapılamaz.", "Uyarı", MessageBoxButtons.OK,
                   MessageBoxIcon.Exclamation);
                }
            }         
         }
        private async void kaldırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (listView3.Items.Count > 0)
                {
                    await SwitchChannel(true);
                }
                else
                {
               MessageBox.Show("Bu işlem için en az 2 kanalınızın olması lazım.", "Uyarı", MessageBoxButtons.OK,
               MessageBoxIcon.Exclamation);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox2.SelectedIndex == 0)
            {
                channel_or_videos = "ytd-video-renderer";
            }
            else if( comboBox2.SelectedIndex == 1)
            {
                channel_or_videos = "ytd-grid-video-renderer";
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                checkBox9.Visible = true;
                checkBox11.Visible = true;
            }
            else { checkBox9.Visible = false; checkBox11.Visible = false; }
        }

        private void tekLinkEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked)
            {
                if (dataGridView1.Rows.Count != 0)
                {
                    if (dataGridView1.Rows.Count == 1)
                    {
                        MessageBox.Show("Bu işlem için minumum 2 kanal gerek.", "Dinamik Yorum", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        checkBox11.Checked = false;
                    }
                }
            }
        }
    }
}
