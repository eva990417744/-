using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;


namespace 课设
{
    public partial class Form1 : Form
    {
        public static string urlmp3;
        public static string urlpic;
        public static int flag;
        public static string[] urlmp3baidu;
        public static string[] urllrc;
        string[] list = { "38574765",
                             "28193068",
                             "26085650",
                             "33781004",
                             "30352891",
                             "784853",
                             "31134592",
                             "28547102",
                             "22737187",
                             "849131",
                             "688067",
                             "792354",
                             "22737185",
                             "802176",
                             "27582615",
                             "812926",
                             "836342",
                             "27709046",
                             "832618",
                             "813962",
                             "834063",
                             "836406",
                             "728916",
                             "32451554",
                             "27795934",
                             "26092806",
                             "406238",
                             "35270988"};

        public Form1()
        {
            InitializeComponent();
            string a = "姓名：田丰溥\n学号：2014031103053\n姓名：叶江南\n学号：2014031103073";
            MessageBox.Show(a);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.stretchToFit = true; // 自动缩放。
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;//歌词设置为居中

            nextsong.Enabled = true;
        }

        private string WangYi(string id)//网易云音乐API GET报文拼接 按歌曲id查找
        {
            string get = "http://music.163.com/api/song/detail/?id=" + id + "&ids=%5B" + id + "%5D";
            return get;
        }

        private string BaiDu(string name)//百度云音乐API GET报文拼接 可按(歌名&歌手||歌名)查找
        {
            string get = "http://box.zhangmen.baidu.com/x?op=12&count=1&title=" + name + "$$";
            return get;
        }

        private string BaiDu(string name,string singer)
        {
            string get = "http://box.zhangmen.baidu.com/x?op=12&count=1&title=" + name + "$$" + singer + "$$$$";
            return get;
        }

        private void BG_set(string BG)   //专辑图片地址 
        {
            pictureBox1.ImageLocation = @BG;
        }

        private string lrc_set(string lrc_bd)
        {
            int a = Convert.ToInt32(lrc_bd);
            a /= 100;
            string get = "http://box.zhangmen.baidu.com/bdlrc/" + a.ToString() + "/" + lrc_bd + ".lrc";
            return get;
        }

        public void Regular(string responseText)//正则表达式
        {
            Regex reg = new Regex(@"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?", RegexOptions.IgnoreCase);
            MatchCollection matchs = reg.Matches(responseText);
            string[] arr = new string[matchs.Count];
            int i = 0;
            foreach (Match item in matchs)
            {
                if (item.Success)
                {
                    arr[i] = item.Value; //这里就是匹配到的项
                    i++;
                }
            }
            urlmp3 = arr[0];
            urlpic = arr[matchs.Count - 1];
        }

        public void Regularbaidu(string responseText)//正则表达式
        {
            Regex reg = new Regex(@"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?", RegexOptions.IgnoreCase);
            Regex regg = new Regex(@"<decode>.{0,}?<.decode>", RegexOptions.IgnoreCase);
            MatchCollection matchs = reg.Matches(responseText);
            string[] arrr = new string[matchs.Count];
            int ii = 0;
            foreach (Match item in matchs)
            {
                if (item.Success)
                {
                    arrr[ii] = item.Value; //这里就是匹配到的项
                    ii++;
                }
            }
            MatchCollection matchss = regg.Matches(responseText);
            string[] arr = new string[matchss.Count];
            int i = 0;
            foreach (Match item in matchss)
            {
                if (item.Success)
                {
                    arr[i] = item.Value; //这里就是匹配到的项
                    i++;
                }
            }
            for (int v = 0; v < matchss.Count; v++)
            {
                arr[v] = arr[v].Substring(17, arr[v].Length - 17);
                arr[v] = arr[v].Substring(0, arr[v].Length - 12);
                arr[v] = arr[v].Replace(" ", "");
            }

            for (int vv = 0; vv < matchs.Count; vv++)
            {
                arrr[vv] = arrr[vv].Substring(0, arrr[vv].Length - 3);
                arrr[vv] = arrr[vv].Replace(" ", "");
                int vvvv = 0;
                for (int vvv = 0; vvv < arrr[vv].Length; vvv++)
                {
                    if (arrr[vv][vvv] == '/')
                    {
                        vvvv = vvv;
                    }
                }
                arrr[vv] = arrr[vv].Substring(0, arrr[vv].Length - (arrr[vv].Length - vvvv) + 1);
                arrr[vv] += arr[vv];
            }
            urlmp3baidu = arrr;
            Regex reggg = new Regex(@"lrc.{0,}?lrc", RegexOptions.IgnoreCase);
            MatchCollection matchsss = reggg.Matches(responseText);
            string[] arrrr = new string[matchsss.Count];
            int iii = 0;
            foreach (Match item in matchsss)
            {
                if (item.Success)
                {
                    arrrr[iii] = item.Value; //这里就是匹配到的项
                    iii++;
                }
            }
            for (int v = 0; v < matchss.Count; v++)
            {
                arrrr[v] = arrrr[v].Substring(6, arrrr[v].Length - 6);
                arrrr[v] = arrrr[v].Substring(0, arrrr[v].Length - 5);
                arrrr[v] = arrrr[v].Replace(" ", "");
            }
            urllrc = arrrr;
        }


        private string Get(string strURL)//实现http Get请求
        {
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.Default);
            string responseText = myreader.ReadToEnd();
            myreader.Close();
            return responseText;
        }

        private string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)//实现http Post请求
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (flag == 1)
            {
                string wy = textBox1.Text;
                string s = Get(WangYi(wy));
                Regular(s);

                axWindowsMediaPlayer1.currentPlaylist.clear();//清除播放的内容
                string mp3 = urlmp3;
                axWindowsMediaPlayer1.currentPlaylist.appendItem(axWindowsMediaPlayer1.newMedia(mp3));//添加播放的地址 
                axWindowsMediaPlayer1.Ctlcontrols.play();//立即播放

                string pic = urlpic;
                BG_set(pic);
                richTextBox1.Clear();
            }
            else if(flag== 2)
            {
                string bd = textBox1.Text;
                if (bd.Replace(" ", "").Length != bd.Length)
                {
                    string[] sArray = bd.Split(' ');
                    string a = Get(BaiDu(sArray[0], sArray[1]));
                    Regularbaidu(a);
                }
                else
                {
                    string b = Get(BaiDu(bd));
                    Regularbaidu(b);
                }

                axWindowsMediaPlayer1.currentPlaylist.clear();//清除播放的内容
                string mp3 = urlmp3baidu[0];
                axWindowsMediaPlayer1.currentPlaylist.appendItem(axWindowsMediaPlayer1.newMedia(mp3));//添加播放的地址 
                axWindowsMediaPlayer1.Ctlcontrols.play();//立即播放

                string lrc = Get(lrc_set(urllrc[0]));
                richTextBox1.Text = lrc;

                listbox();
            }

            axWindowsMediaPlayer1.settings.setMode("loop", false);
            nextsong.Enabled = false;

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(flag == 2||flag == 3)
            {
                button3.ForeColor = Color.Black;
                button4.ForeColor = Color.Black;
            }
            flag = 1;
            button2.ForeColor = Color.Red;
            listBox2.Items.Clear();
            textBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (flag == 1||flag == 3)
            {
                button2.ForeColor = Color.Black;
                button4.ForeColor = Color.Black;
            }
            flag = 2;
            button3.ForeColor = Color.Yellow;
            listBox2.Items.Clear();
            textBox1.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(flag == 1 || flag == 2)
            {
                button4.ForeColor = Color.Black;
            }
            flag = 3;
            button4.ForeColor = Color.Pink;
            axWindowsMediaPlayer1.currentPlaylist.clear();//清除播放的内容
            string a;
            for (int i = 0; i < list.Length; i++)
            {
                a = Get(WangYi(list[i]));
                Regular(a);
                axWindowsMediaPlayer1.currentPlaylist.appendItem(axWindowsMediaPlayer1.newMedia(urlmp3));
            }
            DT();
            nextsong.Enabled = true;
            richTextBox1.Clear();
            listBox2.Items.Clear();
            textBox1.Clear();
        }


        private void DT()
        {
            axWindowsMediaPlayer1.currentPlaylist.clear();//清除播放的内容
            string url = urlmp3;
            string link = urlpic;
            BG_set(link);
            axWindowsMediaPlayer1.currentPlaylist.appendItem(axWindowsMediaPlayer1.newMedia(url));//添加播放的地址 
            axWindowsMediaPlayer1.Ctlcontrols.play();//立即播放          
        }

        private void loopbutton_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.setMode("loop", true);
        }

        private void nextsong_Click(object sender, EventArgs e)
        {
            Random abc = new Random();
            string dt = list[abc.Next(0, 27)];
            string s = Get(WangYi(dt));
            Regular(s);
            DT();

            axWindowsMediaPlayer1.settings.setMode("loop", false);
        }
        private void listbox()
        {
            listBox2.Items.Clear();
            for(int i=0;i < urlmp3baidu.Count();i++)
            {
                listBox2.Items.Add(urlmp3baidu[i]);
            }
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
            int index = listBox2.IndexFromPoint(e.X, e.Y);
            listBox2.SelectedIndex = index;
            if(listBox2.SelectedIndex!=-1)
            {
                axWindowsMediaPlayer1.currentPlaylist.clear();//清除播放的内容
                axWindowsMediaPlayer1.currentPlaylist.appendItem(axWindowsMediaPlayer1.newMedia(urlmp3baidu[Convert.ToInt32(listBox2.SelectedIndex.ToString())]));//添加播放的地址 
                axWindowsMediaPlayer1.Ctlcontrols.play();//立即播放
                
            }
        }
    }
}

