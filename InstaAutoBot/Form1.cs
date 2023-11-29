using InstagramBot;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Policy;
using System.Reflection.Emit;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;

namespace InstaAutoBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listView1.ItemSelectionChanged += ListView1_ItemSelectionChanged;
        }

        IWebDriver driver = new ChromeDriver();
        Information info = new Information();
        List<string> item = new List<string>();
        List<string> TakipEtmeyenler = new List<string>();
        List<string> Followers = new List<string>();
        List<string> Following = new List<string>();
        List<string> UnfollowingAccount = new List<string>();
        List<string> FollowersAnyone = new List<string>();
        List<string> FollowingAnyone = new List<string>();
        List<string> UnfollowingAccountAnyone = new List<string>();
        IReadOnlyCollection<IWebElement> UserAnyone;
        bool ListFollowingButton =false;
        bool ListFollowersButton=false;
        int FollowersControl = 0;
        int FollowingControl = 0;
        int FollowersControlAnyone = 0;
        int FollowingControlAnyone = 0;
        string Member;

        private void GoToInstaLoginMenu()
        {
            driver.Navigate().GoToUrl("https://www.instagram.com/");
            listBox1.Invoke((Action)(() => { listBox1.Items.Add("Uploaded to instagram login screen"); }));
            
            Thread.Sleep(2000);
        }

        private void InstaLogin()
        {
            
            //girs sayfasindaki html taglarinin alinmasi
            IWebElement UserName = driver.FindElement(By.Name("username"));
            IWebElement UserPassword = driver.FindElement(By.Name("password"));
            IWebElement LoginButton = driver.FindElement(By.CssSelector("button[class='_acan _acap _acas _aj1-']"));

            //kullanici bilgilerinin set edilmesi
            info.setName(textBox1.Text);
            info.setPassword(textBox2.Text);

            //kullanici bilgilerinin giris ekranina basilmasi
            UserName.SendKeys(textBox1.Text);
            listBox1.Invoke((Action)(() => { listBox1.Items.Add("Logged the username"); }));
            UserPassword.SendKeys(textBox2.Text);
            listBox1.Invoke((Action)(() => { listBox1.Items.Add("Logged the password"); }));

            Thread.Sleep(2000);

            //login butonuna tiklanmasi
            LoginButton.Click();

            Thread.Sleep(2000);

            try
            {
                IWebElement Wrong = driver.FindElement(By.CssSelector("div[class='_ab2z']"));

                if (Wrong.Text == "Üzgünüz, şifren yanlıştı. Lütfen şifreni dikkatlice kontrol et." || Wrong.Text == "Sorry, your password was incorrect. Please check your password carefully.")
                {
                    driver.Navigate().GoToUrl("https://www.instagram.com/");
                    listBox1.Invoke((Action)(() => { listBox1.Items.Add("Wrong Username or Password, please try again"); }));
                }
            }
            catch (Exception)
            {
                listBox1.Invoke((Action)(() => { listBox1.Items.Add("Logged the account"); }));
                Thread.Sleep(8000);
                while(true)
                {
                    try
                    {
                        IWebElement imageDiv = driver.FindElement(By.CssSelector("div[class='x1iyjqo2 xh8yej3']"));
                        IWebElement image = imageDiv.FindElement(By.CssSelector("img[class='xpdipgo x972fbf xcfux6l x1qhh985 xm0m39n xk390pu x5yr21d xdj266r x11i5rnm xat24cr x1mh8g0r xl1xv1r xexx8yu x4uap5 x18d9i69 xkhd6sd x11njtxf xh8yej3']"));
                        pictureBox1.Load(image.GetAttribute("src"));
                        listBox1.Invoke((Action)(() => { listBox1.Items.Add("Got the profile image"); }));

                        Thread.Sleep(2000);
                        ButtonEnabled(true);
                        listBox1.Invoke((Action)(() => { listBox1.Items.Add("Buttons activeted"); }));
                        break;
                    }
                    catch (Exception)
                    {
                        listBox1.Invoke((Action)(() => { listBox1.Items.Add("Account is not connection");}));
                        Thread.Sleep(1000);
                        listBox1.Invoke((Action)(() => { listBox1.Items.Add("Trying connection to account");}));
                    }
                }


            }

        }

        private void GetFollowers()
        {

            if (FollowersControl == 0)
            {
                listBox1.Invoke((Action)(() => { listBox1.Items.Add("Followers listining..."); }));
                string url2 = "https://www.instagram.com/UserName/followers/";
                string FollowingProfileLink = ChangeLink(url2, textBox1.Text);
                driver.Navigate().GoToUrl(FollowingProfileLink);

                Thread.Sleep(3000);

                //scroll down
                ScrollDown();

                Thread.Sleep(2000);

                //Listining Followers
                Followers = Listening(driver);
                FollowersControl++;
            }
            else if (FollowersControl != 0)
            {
                foreach(var i in Followers)
                {
                    listView1.Invoke((Action)(() => { listView1.Items.Add(i); }));
                }
                label8.Invoke(new Action(() => { label8.Text = Followers.Count.ToString(); }));
            }

            listBox1.Invoke((Action)(() => { listBox1.Items.Add("Followers listed"); }));
        }
        private void GetFollowing()
        {
            
            if (FollowingControl == 0)
            {
                listBox1.Invoke((Action)(() => { listBox1.Items.Add("Following listining..."); }));
                string url1 = "https://www.instagram.com/UserName/following/";
                string ProfileLink = ChangeLink(url1, textBox1.Text);
                driver.Navigate().GoToUrl(ProfileLink);

                Thread.Sleep(3000);

                IWebElement FollowerLink = driver.FindElement(By.CssSelector("a[class='x1i10hfl xjbqb8w x6umtig x1b1mbwd xaqea5y xav7gou x9f619 x1ypdohk xt0psk2 xe8uvvx xdj266r x11i5rnm xat24cr x1mh8g0r xexx8yu x4uap5 x18d9i69 xkhd6sd x16tdsg8 x1hl2dhg xggy1nq x1a2a7pz _alvs _a6hd']"));

                Thread.Sleep(2000);

                //scroll down
                ScrollDown();

                Thread.Sleep(2000);

                //Listining Following
                Following = Listening(driver);
                FollowingControl++;
            }
            else if (FollowingControl != 0)
            {
                foreach (var i in Following)
                {
                    listView1.Invoke((Action)(() => { listView1.Items.Add(i); }));
                }
                label8.Invoke(new Action(() => { label8.Text = Following.Count.ToString(); }));
            }

            listBox1.Invoke((Action)(() => { listBox1.Items.Add("Following listed"); }));

        }

        private void GetUnfollowing()
        {
            int num = 0;
            UnfollowingAccount.Clear();
            foreach (var i in Following)
            {
                if (!Followers.Contains(i))
                {
                    UnfollowingAccount.Add(i);

                }

            }
            foreach (var i in UnfollowingAccount)
            {
                listView1.Invoke((Action)(() => { listView1.Items.Add(i); }));
                num++;
            }
            label8.Invoke(new Action(() => { label8.Text = num.ToString(); }));

            listBox1.Invoke((Action)(() => { listBox1.Items.Add("Unfollowing listed"); }));
        }

        private void GetFollowersAnyone()
        {

            if (FollowersControlAnyone == 0)
            {
                //IWebElement UserAnyone = driver.FindElement(By.CssSelector("a[class='x1i10hfl xjbqb8w x6umtig x1b1mbwd xaqea5y xav7gou x9f619 x1ypdohk xt0psk2 xe8uvvx xdj266r x11i5rnm xat24cr x1mh8g0r xexx8yu x4uap5 x18d9i69 xkhd6sd x16tdsg8 x1hl2dhg xggy1nq x1a2a7pz _alvs _a6hd']"));
                UserAnyone = driver.FindElements(By.CssSelector("a[class='x1i10hfl xjbqb8w x6umtig x1b1mbwd xaqea5y xav7gou x9f619 x1ypdohk xt0psk2 xe8uvvx xdj266r x11i5rnm xat24cr x1mh8g0r xexx8yu x4uap5 x18d9i69 xkhd6sd x16tdsg8 x1hl2dhg xggy1nq x1a2a7pz _alvs _a6hd']"));
                UserAnyone.First().Click();

                Thread.Sleep(2000);

                ScrollDown();

                Thread.Sleep(2000);

                FollowersAnyone=Listening(driver);
                FollowersControlAnyone++;
            }
            
            else if(FollowersControlAnyone != 0)
            {
                foreach (var i in FollowersAnyone)
                {
                    listView1.Invoke((Action)(() => { listView1.Items.Add(i); }));
                }
                label8.Invoke(new Action(() => { label8.Text = FollowersAnyone.Count.ToString(); }));
            }

            listBox1.Invoke((Action)(() => { listBox1.Items.Add("Followers listed"); }));

        }
        
        private void GetFollowingAnyone()
        {
            if (FollowingControlAnyone == 0)
            {
                UserAnyone = driver.FindElements(By.CssSelector("a[class='x1i10hfl xjbqb8w x6umtig x1b1mbwd xaqea5y xav7gou x9f619 x1ypdohk xt0psk2 xe8uvvx xdj266r x11i5rnm xat24cr x1mh8g0r xexx8yu x4uap5 x18d9i69 xkhd6sd x16tdsg8 x1hl2dhg xggy1nq x1a2a7pz _alvs _a6hd']"));
                UserAnyone.Last().Click();

                Thread.Sleep(2000);

                ScrollDown();

                Thread.Sleep(2000);

                FollowingAnyone=Listening(driver);
                FollowingControlAnyone++;
            }
            else if(FollowingControlAnyone != 0)
            {
                foreach (var i in FollowingAnyone)
                {
                    listView1.Invoke((Action)(() => { listView1.Items.Add(i); }));
                }
                label8.Invoke(new Action(() => { label8.Text = FollowingAnyone.Count.ToString(); }));
            }

            listBox1.Invoke((Action)(() => { listBox1.Items.Add("Followers listed"); }));

        }

        private void GetUnfollowingAnyone()
        {
            int num = 0;
            UnfollowingAccountAnyone.Clear();
            foreach (var i in FollowingAnyone)
            {
                if (!FollowersAnyone.Contains(i))
                {
                    UnfollowingAccountAnyone.Add(i);

                }

            }
            foreach (var i in UnfollowingAccountAnyone)
            {
                listView1.Invoke((Action)(() => { listView1.Items.Add(i); }));
                num++;
            }
            label8.Invoke(new Action(() => { label8.Text = num.ToString(); }));

            listBox1.Invoke((Action)(() => { listBox1.Items.Add("Unfollowing listed"); }));
        }

        private void GetInfo()
        {
            int ListviewCount=listView1.Items.Count;

            


            for (int i=0;i<ListviewCount;i++)
            {
                if (listView1.SelectedItems.Count>0)
                {
                    ListViewItem item = listView1.SelectedItems[i];

                    
                }



            }


        }

        private void UnfollowTheUser()
        {
            IWebElement UnfollowButton = driver.FindElement(By.CssSelector("div[class='_aacl _aaco _aacw _aad6 _aade']"));
            UnfollowButton.Click();
            Thread.Sleep(2000);

            IReadOnlyCollection<IWebElement> UnfollowButton2 = driver.FindElements(By.CssSelector("span [class='x1lliihq x193iq5w x6ikm8r x10wlt62 xlyipyv xuxw1ft']"));

            foreach (var i in UnfollowButton2)
            {
                if (i.Text == "Takibi Bırak" || i.Text== "Stop following")
                {
                    UnfollowButton2.Last().Click();
                }

            }

            if(UnfollowButton.Text== "İstek Gönderildi")
            {
                //Thread.Sleep(2000);
                IWebElement UnfollowButton3 = driver.FindElement(By.CssSelector("button[class='_a9-- _a9-_']"));
                UnfollowButton3.Click();    
                

            }
            
            listBox1.Invoke((Action)(() => { listBox1.Items.Add("Unfollowed the account!"); }));
            
            if (Followers.Contains(Member) && !Following.Contains(Member))
            {
                Followers.Remove(Member);
            }
            else if(Following.Contains(Member) && !Followers.Contains(Member))
            {
                UnfollowingAccount.Remove(Member);
                Following.Remove(Member);

            }
            else if (Followers.Contains(Member) || Following.Contains(Member))
            {
                Followers.Remove(Member);
                Following.Remove(Member);
            }
        }
        private void FollowTheUser()
        {
            IWebElement UnfollowButton = driver.FindElement(By.CssSelector("div[class='_aacl _aaco _aacw _aad6 _aade']"));
            UnfollowButton.Click();
            listBox1.Invoke((Action)(() => { listBox1.Items.Add("followed the account!"); }));
            Thread.Sleep(2000);
            Following.Add(Member);

        }

        internal void ScrollDown()
        {
            string jsCommand = "" +
            "page = document.querySelector('._aano');" +
            "page.scrollTo(0,page.scrollHeight);" +
            "var pageEnd=page.scrollHeight;" +
            "return pageEnd";

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var pageEnd = Convert.ToInt32(js.ExecuteScript(jsCommand));

            while (true)
            {
                var end = pageEnd;
                Thread.Sleep(2000);
                pageEnd = Convert.ToInt32(js.ExecuteScript(jsCommand));
                if (end == pageEnd)
                    break;
            }
        }

        private List<string> Listening(IWebDriver driver)
        {
            IReadOnlyCollection<IWebElement> Follow = driver.FindElements(By.CssSelector("span[class='_aacl _aaco _aacw _aacx _aad7 _aade']"));
            int num = 1;
            List<string> User=new List<string>();
            foreach (var i in Follow)
            {
                listView1.Invoke((Action)(() => { listView1.Items.Add(i.Text); }));
                User.Add(i.Text);
                
                num++;
                    
            }

            label8.Invoke(new Action(() => { label8.Text = Follow.Count.ToString(); }));
            return User;            
            
        }


        private void CheckTextBoxes()
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                // Her iki TextBox da dolu ise Login butonunu etkinleştir
                button11.Enabled = true;
            }
            else
            {
                // Herhangi bir TextBox boşsa Login butonunu devre dışı bırak
                button11.Enabled = false;
            }
        }

        private void ButtonEnabled(bool enabled)
        {
            foreach (Control btns in panel2.Controls) {
                if(btns is Button)
                    panel2.Invoke((Action)(() => { ((Button)btns).Enabled = enabled; }));
                
            }

            button9.Invoke((Action)(() => { button9.Enabled = false; }));
            button12.Invoke((Action)(() => { button12.Enabled = true; }));
            
        }

        internal void MinimizeToTray()
        {
            // Formu simge durumuna getir
            this.Invoke((Action)(() => { this.WindowState = FormWindowState.Minimized; }));
            

            // NotifyIcon nesnesi oluşturun ve simgeyi ayarlayın
            NotifyIcon notifyIcon = new NotifyIcon();
            //notifyIcon.Icon = new Icon("icon.ico"); // Simgeyi değiştirin veya kendi simgenizi ekleyin
            //notifyIcon.Visible = true;

            // Balon ipucu (tooltip) ayarlayın (isteğe bağlı)
            listBox1.Invoke((Action)(() => { listBox1.Items.Add("Icon minimized"); }));

            // Simgeye tıklanınca formu tekrar göster
            notifyIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.WindowState = FormWindowState.Normal;
                    notifyIcon.Visible = false; // Simgeyi gizle
                }
            };
        }
        
        internal string ChangeLink(string Url, string Name)
        {
            string NewUrl = Url.Replace("UserName", Name);
            return NewUrl;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Thread islem = new Thread(InstaLogin);
            islem.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void button13_Click(object sender, EventArgs e)
        {
            Thread islem = new Thread(MinimizeToTray);
            islem.Start();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            CheckTextBoxes();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread islem = new Thread(GoToInstaLoginMenu);
            islem.Start();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Thread islem = new Thread(GetFollowers);
            islem.Start();
            ListFollowersButton = true;

            if (ListFollowersButton && ListFollowingButton)
            {
                button9.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            label8.Text = "0";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Thread islem = new Thread(GetFollowing);
            islem.Start();
            ListFollowingButton = true;

            if (ListFollowersButton && ListFollowingButton)
            {
                button9.Enabled = true;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Thread islem = new Thread(GetUnfollowing);
            islem.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            Thread islem = new Thread(UnfollowTheUser);
            islem.Start();
            button5.Enabled = true;
            button4.Enabled = false;
            button10.Enabled = false;
            button6.Enabled = false;
            button3.Enabled = false;

        }

        private void ListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // ListView'de öğeye tıklandığında yapılacak işlemi burada tanımlayın
            if (e.IsSelected)
            {
                ListViewItem selected = e.Item;

                Member = selected.Text;

                string UserProfile = "https://www.instagram.com/UserName/";
                string UserProfileLink = ChangeLink(UserProfile, Member);
                driver.Navigate().GoToUrl(UserProfileLink);
                Thread.Sleep(2000);

                try
                {
                    IWebElement imageDiv1 = driver.FindElement(By.CssSelector("div[class='_aarf _aarg']"));
                    IWebElement imageDiv2 = imageDiv1.FindElement(By.CssSelector("span[class='xnz67gz x14yjl9h xudhj91 x18nykt9 xww2gxu x9f619 x1lliihq x2lah0s x6ikm8r x10wlt62 x1n2onr6 x1ykvv32 xougopr x159fomc xnp5s1o x194ut8o x1vzenxt xd7ygy7 xt298gk x1xrz1ek x1s928wv x1n449xj x2q1x1w x1j6awrg x162n7g1 x1m1drc7']"));
                    IWebElement image = imageDiv2.FindElement(By.CssSelector("img[class='xpdipgo x972fbf xcfux6l x1qhh985 xm0m39n xk390pu x5yr21d xdj266r x11i5rnm xat24cr x1mh8g0r xl1xv1r xexx8yu x4uap5 x18d9i69 xkhd6sd x11njtxf xh8yej3']"));
                    pictureBox2.Load(image.GetAttribute("src"));
                
                }
                catch (Exception)
                {
                    try
                    {
                        IWebElement imageDiv1= driver.FindElement(By.CssSelector("div[class='_aarf']"));
                        IWebElement imageDiv2 = imageDiv1.FindElement(By.CssSelector("span[class='xnz67gz x14yjl9h xudhj91 x18nykt9 xww2gxu x9f619 x1lliihq x2lah0s x6ikm8r x10wlt62 x1n2onr6 x1ykvv32 xougopr x159fomc xnp5s1o x194ut8o x1vzenxt xd7ygy7 xt298gk x1xrz1ek x1s928wv x1n449xj x2q1x1w x1j6awrg x162n7g1 x1m1drc7']"));
                        IWebElement image = imageDiv2.FindElement(By.CssSelector("img[class='xpdipgo x972fbf xcfux6l x1qhh985 xm0m39n xk390pu x5yr21d xdj266r x11i5rnm xat24cr x1mh8g0r xl1xv1r xexx8yu x4uap5 x18d9i69 xkhd6sd x11njtxf xh8yej3']"));
                        pictureBox2.Load(image.GetAttribute("src"));

                    }
                    catch (Exception)
                    {
                        IWebElement imageDiv = driver.FindElement(By.CssSelector("img[class='_aadp']"));
                        pictureBox2.Load(imageDiv.GetAttribute("src"));
                    }
                }


                

                listBox1.Invoke((Action)(() => { listBox1.Items.Add("Got the profile image"); }));

                IWebElement AreYouFolloing = driver.FindElement(By.CssSelector("div[class='_aacl _aaco _aacw _aad6 _aade']"));

                if (AreYouFolloing.Text == "Takiptesin")
                {
                    button5.Enabled = false;
                    button4.Enabled = true;
                    button10.Enabled = true;
                    button6.Enabled = true;
                    button3.Enabled = true;
                }
                else if (AreYouFolloing.Text == "Sen de Onu Takip Et" || AreYouFolloing.Text == "Takip Et")
                {
                    button5.Enabled = true;
                    button4.Enabled = false;
                }
                
                try
                {
                    IWebElement AreYouPrivateAccount = driver.FindElement(By.CssSelector("h2[class='_aa_u']"));
                    if (AreYouFolloing.Text == "Sen de Onu Takip Et" || AreYouFolloing.Text == "Takip Et")
                    {
                        if (AreYouPrivateAccount.Text == "Bu Hesap Gizli")
                        {
                            button10.Enabled = false;
                            button6.Enabled = false;
                            button3.Enabled = false;
                        }
                    }
                }
                catch (Exception)
                {
                    //listBox1.Invoke((Action)(() => { listBox1.Items.Add("ERROR!"); }));

                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread islem = new Thread(FollowTheUser);
            islem.Start();
            try
            {
                IWebElement AreYouPrivateAccount = driver.FindElement(By.CssSelector("h2[class='_aa_u']"));
                button5.Enabled = false;
                button4.Enabled = true;
                if (AreYouPrivateAccount.Text == "Bu Hesap Gizli")
                {
                    button10.Enabled = false;
                    button6.Enabled = false;
                    button3.Enabled = false;
                }
            }
            catch (Exception)
            {

                //listBox1.Invoke((Action)(() => { listBox1.Items.Add("ERROR!"); }));
            }
            
            
            

        }

        private void button15_Click(object sender, EventArgs e)
        {
            Thread islem = new Thread(GetFollowersAnyone);
            islem.Start();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Thread islem = new Thread(GetFollowingAnyone);
            islem.Start();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Thread islem = new Thread(GetUnfollowingAnyone);
            islem.Start();
        }
    }
}
