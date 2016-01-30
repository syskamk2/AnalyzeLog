using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LogAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //ファイル選択ボタン
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "ログファイル | *.csv";
            DialogResult ret;
            ret = ofd.ShowDialog();
            if(ret == DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
            else 
            {
                textBox1.Text = "";
            }
        }
        //ログ解析開始ボタン
        private void button2_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(textBox1.Text);
            try
            {
                while(sr.EndOfStream == false)
                {
                    string line = sr.ReadLine();
                    string[] fields = line.Split(',');
                    for(int i = 0; i < fields.Length; i++)
                    {
                        label1.Text += fields[i] + " ";
                    }
                    label1.Text += "\r\n";
                }
            }
            finally
            {
                sr.Close();
            }
        }
    }
}
