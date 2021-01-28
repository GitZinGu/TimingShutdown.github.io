/*********************************************************
*   Author:Zingu                                         *
*   Create Time:2010年8月4日12:12:24                     *
*   Code Info:Windows Forms BaseOpertion                 *
*   Other:Noting....                                     *
**********************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading; 

namespace TimingClose
{
    public partial class Form1 : Form
    { 
        private bool STATE = false;
        private long sec;
        private long ms;
        private string  type="";
        System.Timers.Timer t1 = new System.Timers.Timer(1000); 
        System.Timers.Timer t2 =  new System.Timers.Timer(1000);
        System.Timers.Timer t3 = new System.Timers.Timer(1);
        //System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        public Form1()
        {
            InitializeComponent(); 
            t1.Elapsed += new System.Timers.ElapsedEventHandler(timeout1);
            t2.Elapsed += new System.Timers.ElapsedEventHandler(timeout2);
            t3.Elapsed += new System.Timers.ElapsedEventHandler(timeout3); 
           // this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
           // t.Interval = 1;
           // t.Enabled = true;
           // this.t.Tick += new System.EventHandler(this.t_Tick);
            LoadCommbox();
            Init();
        }
        private void t_Tick(object sender, System.EventArgs e)
        {
            if (STATE)
            {
                ShowCountDown();
                ShowCountDownER();
            }
        }
        public void timeout1(object source, System.Timers.ElapsedEventArgs e)
        { 
            ShowNowTime();  
        }
        public void timeout2(object source, System.Timers.ElapsedEventArgs e)
        {
            if (STATE)
            {
                ShowCountDown();
            } 
        }
        public void timeout3(object source, System.Timers.ElapsedEventArgs e)
        {
            if (STATE)
            {
                ShowCountDownER();
            } 
        }
        /// <summary>
        /// 当前时间
        /// </summary>
        private void ShowNowTime()
        { 
            Thread.Sleep(1000);
            lbNowTime.Text = "北京时间:" + DateTime.Now.ToLongDateString().ToString() + DateTime.Now.ToLongTimeString().ToString();  
        }
        /// <summary>
        /// 倒计时
        /// </summary>
        private void ShowCountDown()
        {
            //计算多少秒 
            int days = Convert.ToInt32(sec / (24 * 3600));
            int hour = Convert.ToInt32((sec - days * 24 * 3600) / 3600);
            int mint = Convert.ToInt32((sec - days * 24 * 3600 - hour * 3600) / 60);
            int secs = Convert.ToInt32((sec - days * 24 * 3600 - hour * 3600 - mint*60));
            lbCountdown.Text = "倒计时:" + days + "天" + hour + "时" + mint + "分" + secs + "秒";
            sec--; 

        }
 
        private long GetSecond()
        {
            long lo;
            if (radioButton1.Checked)//倒计时
            {
                lo = long.Parse((numericUpDownHour.Value * 3600 + numericUpDownMin.Value * 60 + numericUpDownSec.Value).ToString());
            }
            else//固定时间
            {
                DateTime dt1 = DateTime.Now;
                DateTime dt2 = GetSetDateTime(); 
                TimeSpan ts = dt1.Subtract(dt2).Duration();
                lo = ts.Days * 24 * 3600 + ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds; 
            }
            return lo;
        }

        private DateTime GetSetDateTime()
        { 
            return DateTime.Parse(dateTimePicker1.Text).AddHours(double.Parse(comboBox2.Text)).AddMinutes(double.Parse(comboBox3.Text)).AddSeconds(double.Parse(comboBox4.Text));
        }

        /// <summary>
        /// 到计时器
        /// </summary>
        private void ShowCountDownER()
        { 
            lbCountdownER.Text = "倒计时器:" + ms + "ms";
            TimeOut();
            ms--; 
        }

        /// <summary>
        /// 到点执行
        /// </summary>
        private void TimeOut()
        {
            if (ms == 0)
            {
                STATE = false;
                t2.Stop();
                t3.Stop();
                //t.Stop();
                WaitMinute wm = new WaitMinute(type); 
                if (wm.ShowDialog() == DialogResult.OK)
                {
                    Doit();
                }
                else
                {
                    SetEnable();
                    button1.Text = "开始";
                    t2.Stop();
                    t3.Stop(); 
                }
                return;
            } 
        }

        /// <summary>
        /// 加载下拉
        /// </summary>
        private void LoadCommbox()
        {
            //时
            for (int i = 0; i < 24; i++)
            {
                comboBox2.Items.Add(i.ToString().PadLeft(2, '0'));
            }
            //分秒
            for (int i = 0; i < 60; i++)
            {
                comboBox3.Items.Add(i.ToString().PadLeft(2,'0'));
                comboBox4.Items.Add(i.ToString().PadLeft(2,'0'));
            } 
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void Init()
        {
            //this.MinimizeBox = false;
            this.MaximizeBox = false;
            comboBox1.SelectedIndex = 0;

            comboBox2.SelectedIndex = DateTime.Now.Hour - 1;
            comboBox3.SelectedIndex = DateTime.Now.Minute - 1;
            comboBox4.SelectedIndex = DateTime.Now.Second - 1;
            radioButton1.Select();
            t1.Start();  
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        private void Doit()
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "关机":
                    Process.Start("shutdown.exe", "-s");
                    break;
                case "重启":
                    Process.Start("shutdown.exe", "-r -t 10");
                    break;
                case "注销":
                    Process.Start("shutdown.exe", "-l");
                    break;
                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //检查
            if (!CheckSetting()) return;  
            if (button1.Text == "开始")
            {
                SetEnable();
                button1.Text = "停止";
                t2.Start();
                t3.Start();
            }
            else
            {
                SetEnable();
                button1.Text = "开始";
                t2.Stop();
                t3.Stop(); 
            }
            
            type = comboBox1.SelectedItem.ToString();
            STATE = true;
            sec = GetSecond();
            ms = sec * 100; 
            
           
             
        }

        /// <summary>
        /// 禁止交互
        /// </summary>
        private void SetEnable()
        {
            groupBox2.Enabled = !groupBox2.Enabled;
            groupBox3.Enabled = !groupBox3.Enabled;
            groupBox5.Enabled = !groupBox5.Enabled; 
        }

        /// <summary>
        /// 检查选中时间不能小于当前时间
        /// </summary>
        private bool CheckSetting()
        {
            try
            { 
                if (radioButton2.Checked && GetSetDateTime() < DateTime.Now)
                {
                    MessageBox.Show("设定时间小于当前时间,关个锤子？");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        /// <summary>
        /// 折叠-展开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "折叠")
            {
                button2.Text = "展开";
                
                for (int i = 0; i < 14; i++)
                {
                    this.Height= this.Height - i;
                    Thread.Sleep(100);
                }
            }
            else
            {
                button2.Text = "折叠";
                for (int i = 0; i < 14; i++)
                {
                    this.Height = this.Height + i;
                    Thread.Sleep(100);
                }
            }
        }

        /*
        private void timer1_Tick(object sender, EventArgs e)
        {
            ShowNowTime();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            ShowCountDown();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            ShowCountDownER();
        }
        */ 

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
             this.Visible=   true; 
             this.WindowState = FormWindowState.Normal;  
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = true; 
            WindowState = FormWindowState.Normal;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr; 
            dr = MessageBox.Show("确认退出吗", "确认对话框", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes) 
            {
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            } 
            else
            { 
                e.Cancel = true; 
            } 
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定退出？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.OK)
            {
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            }
        } 

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            } 
        }
  
    }
}
