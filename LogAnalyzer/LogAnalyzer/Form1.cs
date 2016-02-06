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
using System.Windows.Forms.DataVisualization.Charting;

namespace LogAnalyzer
{
    public partial class Form1 : Form
    {
        enum LogItem
        {
            IDX_SERIAL,
            IDX_NPIX,
            IDX_DIFFMAX,
            IDX_DIFFAVE,
            IDX_SIZE
        }
        Dictionary<string, int> logitem = new Dictionary<string, int>()
        {
            {"serial", (int)LogItem.IDX_SERIAL},
            {"nPix", (int)LogItem.IDX_NPIX},
            {"DiffMax", (int)LogItem.IDX_DIFFMAX},
            {"DiffAve", (int)LogItem.IDX_DIFFAVE},
        };
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
            if (ret == DialogResult.OK)
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
            const int numlog = (int)LogItem.IDX_SIZE;
            Series[] logs = new Series[numlog];
            bool[] showlist = new bool[numlog];
            chart1.Series.Clear();
            label1.ResetText();
            foreach (var val in logitem)
            {
                showlist[val.Value] = false;
                logs[val.Value] = new Series();
                logs[val.Value].Name = val.Key;
                logs[val.Value].ChartType = SeriesChartType.StepLine;
            }
           foreach(object idx in checkedListBox1.CheckedItems)
           {
               string item = idx.ToString();
               if(logitem.ContainsKey(item))
               {
                   showlist[logitem[item]] = true;
               }
           }
           StreamReader sr = null;
            try
            {
                sr = new StreamReader(textBox1.Text);
                while (sr.EndOfStream == false)
                {
                    string line = sr.ReadLine();
                    string[] fields = line.Split(',');
                    for (int i = 0; i < fields.Length; i++)
                    {
                        label1.Text += fields[i] + " ";
                    }
                    label1.Text += "\r\n";
                    if (fields.Length >= (int)LogItem.IDX_SIZE)
                    {
                        foreach (var val in logitem)
                        {
                            if (val.Value == (int)LogItem.IDX_SERIAL)
                            {
                                continue;
                            }
                            logs[val.Value].Points.AddXY(
                                int.Parse(fields[(int)LogItem.IDX_SERIAL]),
                                int.Parse(fields[val.Value]));
                        }
                    }
                }
                foreach (var val in logitem)
                {
                    if (val.Value == (int)LogItem.IDX_SERIAL)
                    {
                        continue;
                    }
                    if (showlist[val.Value])
                    {
                        chart1.Series.Add(logs[val.Value]);
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("ファイルが開けません");
                System.Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }
    }
}
