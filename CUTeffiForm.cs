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
using System.Threading;

namespace Cuteffi_rebuild
{
    public partial class CUTeffiForm : Form
    {
        public SplashScreen FSS = null;
        public int indexPanelSetting = 0;  //  0 = 設定面板關,  1 = 設定面板開
        public int indexProccessMode = 0;  //  0 = 預設,  1 = 自訂
        public int indexDAQMode = 0; //  0 = 停止,  1 = 擷取
        public int indexProgramState = 0;  //  0 = 初始化,  1 = 擷取中,  2 = 閒置
        public static int indexMaterial = 1; // 1 = 鋁合金,  2 = 不鏽鋼
        public double OperatingSPmax;
        public static int varNtest = 12;
        public static double threshold;
        public static double[] A = new double[4];
        int monitoropen = 0;
        

        
        public CUTeffiForm()
        {
            InitializeComponent();
            this.Size = new Size(795, 600);
            panel1.Visible = false;
            initialCUTeffi();
        }

        private void monitorbutton_Click(object sender, EventArgs e)
        {
            //panel6.Visible = false;
            //panel4.Visible = false;
            //panel10.Visible = false;
            //panelSetting.Visible = false;
            if (monitoropen == 0)
            {
                panel1.Visible = true;
                this.Size = new Size(1510, 600);
                panel1.Location = new Point(782,75);
                monitoropen = 1;
            }
            else 
            {
                panel1.Visible = false;
                this.Size = new Size(795, 600);
                monitoropen = 0;
            }
        }

        private void drillingmodebutton_Click(object sender, EventArgs e)
        {
            panel10.Visible = true;
            panel4.Visible = true;
            panelSetting.Visible = false;
            panel1.Visible = false;
            panel6.Visible = false;
            panel4.Location = new Point(353, 75);
            panel10.Location = new Point(0, 75);

        }

        private void millingmodebutton_Click(object sender, EventArgs e)
        {
            panel6.Location = new Point(0,75);
            panel6.Visible = true;
            panelSetting.Location = new Point(353, 75);
            panelSetting.Visible = true;
            panel1.Visible = false;
            panel4.Visible = false;
            panel10.Visible = false;
        }
                
        private void initialCUTeffi()
        {
            indexProgramState = 0;
            statepanel(indexProgramState);

            textBox4.Text = Convert.ToString(Convert.ToDouble(textBox1.Text) * 0.9);
            textBox2.Text = Convert.ToString(Convert.ToDouble(textBox4.Text) - (varNtest - 1) * 250);

            label4.Text = " ";
            label5.Text = " ";
            label6.Text = " ";
            label24.Text = " ";
            label25.Text = " ";
            label26.Text = " ";
            label27.Text = " ";
            label28.Text = " ";

            buttonStop.Visible = false;
            Thread.Sleep(2000);


            indexProgramState = 2;
            statepanel(indexProgramState);
        }

        private void statepanel(int indexProgramState)
        {
            if (indexProgramState == 0)
            {
                panel5.BackColor = Color.Yellow;
                label14.Text = " 初始化";
            }
            else if (indexProgramState == 1)
            {
                label14.Text = "計算中";
                panel5.BackColor = Color.Yellow;
            }
            else
            {
                panel5.BackColor = Color.GreenYellow;
                label14.Text = "  就緒";
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Visible = false;
            buttonStop.Visible = true;
            indexProgramState = 1;
            statepanel(indexProgramState);
            Refresh();

            OperatingSPmax = Convert.ToDouble(textBox4.Text);
            //threshold = Convert.ToDouble(textBox15.Text);

            

        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            indexProccessMode = 0;
            checkBox1.Checked = true;
            checkBox2.Checked = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;

            if (string.IsNullOrEmpty(textBox1.Text)) { }
            else
            {
                textBox4.Text = Convert.ToString(Convert.ToDouble(textBox1.Text) * 0.9);
                if (Convert.ToDouble(textBox4.Text) - (varNtest - 1) * 250 > 0)
                {
                    textBox2.Text = Convert.ToString(Convert.ToDouble(textBox4.Text) - (varNtest - 1) * 250);
                }
                else
                {
                    textBox2.Text = "0";
                }
            }

        }
        private void checkBox2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                checkBox2.Checked = false;
                MessageBox.Show("請先輸入主軸的最高轉速", "Error");
                return;
            }
            else
            {
                textBox1.Enabled = false;
                indexProccessMode = 1;
                checkBox1.Checked = false;
                checkBox2.Checked = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            }

        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text)) { }
            else
            {
                if (Convert.ToDouble(textBox4.Text) > Convert.ToDouble(textBox1.Text))
                {
                    textBox4.Text = Convert.ToString(Convert.ToDouble(textBox1.Text) * 0.9);
                    if (Convert.ToDouble(textBox4.Text) - (varNtest - 1) * 250 > 0)
                    {
                        textBox2.Text = Convert.ToString(Convert.ToDouble(textBox4.Text) - ((varNtest - 1) * 250));
                    }
                    else
                    {
                        textBox2.Text = "0";
                    }
                }
                else
                {
                    if (Convert.ToDouble(textBox4.Text) - (varNtest - 1) * 250 > 0)
                    {
                        textBox2.Text = Convert.ToString(Convert.ToDouble(textBox4.Text) - (varNtest - 1) * 250);
                    }
                    else
                    {
                        textBox2.Text = "0";
                    }
                }
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(textBox1.Text)) { }
            else
            {
                textBox4.Text = Convert.ToString(Convert.ToDouble(textBox1.Text) * 0.9);
                if (Convert.ToDouble(textBox4.Text) - (varNtest - 1) * 250 > 0)
                {
                    textBox2.Text = Convert.ToString(Convert.ToDouble(textBox4.Text) - (varNtest - 1) * 250);
                }
                else
                {
                    textBox2.Text = "0";
                }
            }

        }
        private void CUTeffi_FormClosed(object sender, FormClosedEventArgs e)
        {
            FSS.Close();
        }

        private void CUTeffi_Shown(object sender, EventArgs e)
        {
            FSS.Hide();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            buttonStart.Visible = true;
            buttonStop.Visible = false;
            indexProgramState = 2;
            statepanel(indexProgramState);
            Refresh();
        }

        

        ///
        public void panelupdate(double[] A)
        {

            label24.Text = Convert.ToString(A[0]);
            label4.Text = Convert.ToString(A[1]);
            label5.Text = Convert.ToString(A[2]);
            label6.Text = Convert.ToString(A[3]);

            double FeedPerFlute = Convert.ToDouble(textBox7.Text);
            double Nunber_Flute = Convert.ToDouble(textBox8.Text);
            label25.Text = Convert.ToString(A[0] * FeedPerFlute * Nunber_Flute);
            label26.Text = Convert.ToString(A[1] * FeedPerFlute * Nunber_Flute);
            label27.Text = Convert.ToString(A[2] * FeedPerFlute * Nunber_Flute);
            label28.Text = Convert.ToString(A[3] * FeedPerFlute * Nunber_Flute);

            indexProgramState = 2;
            statepanel(indexProgramState);
            buttonStart.Visible = true;
            buttonStop.Visible = false;
            Refresh();
        }


       /*
        private void vibrationmonitor()
        {
                allrms0 = new double[d00.Length];
                for (int i = 0; i < d00.Length; i++)
                {
                   allrms0[i] = Math.Sqrt(Math.Pow(d00[i], 2) + Math.Pow(d10[i], 2) + Math.Pow(d20[i], 2));
                }
                allrms1 = rootMeanSquare(allrms0);
                ///監控示波器
                if (indexplot > time.Length)
                {
                    for (int i = 0; i < time.Length-1; i++)
                    {
                        time[i] = time[i + 1];
                        plotrms[i] = plotrms[i + 1];
                        
                    }
                    time[time.Length - 1] = time[time.Length - 1] + Convert.ToDouble(samplepoint.Text) / 12800;
                    plotrms[time.Length - 1] = allrms1;
                }
                else
                {
                    time[0] = starttime;
                    for (int i = 1; i < time.Length; i++)//time.Length
                    {
                        time[i] = starttime + Convert.ToDouble(samplepoint.Text) / 12800 * i;
                    }
                    plotrms[indexplot] = allrms1;
                }
                ///
                indexplot++;
                maxrms[1] = allrms1;
                if (maxrms[0] < maxrms[1])
                {
                    maxrms[0] = maxrms[1];
                }

                time_chart.Series[0].Points.Clear();
                time_chart.Series[1].Points.Clear();
            for (int i = 0; i < time.Length; i++)
            {
                vibration_monitor.Series[0].Points.AddXY(time[i],maxrms[0]);
                vibration_monitor.Series[0].Points.AddXY(time[i],plotrms[i]);
            }
        }
        */

    }


}
