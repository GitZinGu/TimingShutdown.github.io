using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TimingClose
{
    public partial class WaitMinute : Form
    {  
        private int i = 30;
        Timer _timer = new System.Windows.Forms.Timer();
        public WaitMinute()
        {
            InitializeComponent(); 
        }
        public WaitMinute(string picktype)
        {
            InitializeComponent();
            label1.Text = "正在进行'" + picktype + "'操作...."; 
            _timer.Interval = 1000;
            _timer.Enabled = true;
            this._timer.Tick += new System.EventHandler(this.TS_tick);
            
        }
        private void TS_tick(object sender, System.EventArgs e)
        {
            button1.Text = "确认(" + i-- + ")";  
            if (i == 0)
            {
                _timer.Stop(); 
                this.button1.PerformClick();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }  

    }
}
