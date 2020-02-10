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
        public static double threshold = 0.06;
        public static double[] A = new double[4],Alist = new double[12],Glist = new double[12];
        public static Panel Panel1;
        public static CheckBox checkBox_quality, checkBox_effect;
        nidaqAPI nidaq = new nidaqAPI();



        public CUTeffiForm()
        {
            InitializeComponent();
            this.Size = new Size(795, 600);
            panel1.Visible = true;
            checkBox_quality = this.checkBox3;
            checkBox_effect = this.checkBox4;
            Panel1 = this.panel1;
            initialCUTeffi();
        }

        private void monitorbutton_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
            panel11.Visible = false;
            panel4.Visible = false;
            panel13.Visible = false;
            panel10.Visible = false;
            panel3.Visible = false;
            panel1.Visible = true;
            panelSetting.Visible = false;
            buttonSetting.Visible = false;
            buttonExport.Visible = false;
            indexPanelSetting = 0;
            

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
            panel6.Location = new Point(352, 75);
            panel6.Visible = true;
            panel3.Location = new Point(0, 75);
            panel3.Visible = true;
            panel1.Visible = false;
            panel4.Visible = false;
            panel10.Visible = false;
            buttonSetting.Visible = true;
            buttonExport.Visible = true;
            buttonSetting.Location = new Point(352, 507);
            buttonExport.Location = new Point(518, 507);
            panel13.Location = new Point(352, 457);
            panel13.Visible = true;
            millingstart.Visible = true;
            millingstop.Visible = false;
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
            label48.Text = " ";
            label49.Text = " ";
            label50.Text = " ";
            label51.Text = " ";
            label59.Text = " "; label60.Text = " "; label61.Text = " "; label62.Text = " "; label63.Text = " "; label64.Text = " "; label65.Text = " "; label66.Text = " "; label67.Text = " "; label68.Text = " ";
            label69.Text = " "; label70.Text = " "; label71.Text = " "; label72.Text = " "; label73.Text = " "; label74.Text = " "; label75.Text = " "; label76.Text = " "; label77.Text = " "; label78.Text = " ";
            label79.Text = " "; label80.Text = " "; label81.Text = " "; label82.Text = " "; label83.Text = " "; label84.Text = " "; label85.Text = " "; label86.Text = " "; label87.Text = " "; label88.Text = " ";
            label89.Text = " "; label90.Text = " "; label91.Text = " "; label92.Text = " "; label93.Text = " "; label94.Text = " ";

            buttonStop.Visible = false;
            nidaq.StartDAQ(10000);
            Thread.Sleep(2000);
            nidaq.StopDAQ();


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
            monitorbutton.Enabled = false;
            millingmodebutton.Enabled = false;
            //drillingmodebutton.Enabled = false;
            buttonSetting.Enabled = false;
            buttonExport.Enabled = false;
            statepanel(indexProgramState);
            Refresh();

            OperatingSPmax = Convert.ToDouble(textBox4.Text);
            nidaq.StartDAQ(OperatingSPmax);


            

        }
        private void buttonStop_Click(object sender, EventArgs e)
        {
            nidaq.StopDAQ();
            buttonStart.Visible = true;
            buttonStop.Visible = false;
            monitorbutton.Enabled = true;
            millingmodebutton.Enabled = true;
            //drillingmodebutton.Enabled = true;
            indexProgramState = 2;
            buttonSetting.Enabled = true;
            buttonExport.Enabled = true;
            statepanel(indexProgramState);
            Refresh();
            
        }
        private void buttonSetting_Click(object sender, EventArgs e)
        {
            if (indexPanelSetting == 0)
            {
                panelSetting.Visible = true;
                panel6.Visible = false;
                panel13.Visible = false;
                indexPanelSetting = 1;
                panelSetting.Location = new Point(352, 75);
            }
            else
            {
                panelSetting.Visible = false;
                panel6.Visible = true;
                panel13.Visible = true;
                indexPanelSetting = 0;
                if (checkBox3.Checked == true && checkBox4.Checked ==false)
                {
                    panel11.Visible = false;
                    panel6.Visible = true;
                    panel6.Location = new Point(352, 75);
                }
                else
                {
                    panel6.Visible = false;
                    panel11.Visible = true;
                    panel11.Location = new Point(352, 75);
                }
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            string[] SaveSP = new string[4];
            string[] SaveFeed = new string[4];
            string[] SaveG = new string[4];
            SaveSP[0] = label24.Text;
            SaveSP[1] = label4.Text;
            SaveSP[2] = label5.Text;
            SaveSP[3] = label6.Text;
            SaveFeed[0] = label25.Text;
            SaveFeed[1] = label26.Text;
            SaveFeed[2] = label27.Text;
            SaveFeed[3] = label28.Text;
            SaveG[0] = label48.Text;
            SaveG[1] = label49.Text;
            SaveG[2] = label50.Text;
            SaveG[3] = label51.Text;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = "c:\\";
            sfd.Filter = "txt files (*.txt)|*.txt";
            sfd.ShowDialog();
            string saveFileName = sfd.FileName;
            if (string.IsNullOrEmpty(saveFileName)) { }
            else
            {
                using (StreamWriter sw = new StreamWriter(saveFileName))
                {
                    sw.Write("刀號: ");
                    sw.WriteLine(textBox5.Text);
                    sw.Write("刀具直徑: ");
                    sw.Write(textBox6.Text);
                    sw.WriteLine(" mm");
                    sw.WriteLine("");
                    sw.WriteLine("建議參數");
                    sw.Write("S: ");
                    sw.Write(SaveSP[0]);
                    sw.Write(" RPM, ");
                    sw.Write("F: ");
                    sw.Write(SaveFeed[0]);
                    sw.WriteLine(" mm/min");
                    sw.WriteLine("");
                    sw.WriteLine("其他參數");
                    for (int i = 1; i < 4; i++)
                    {
                        sw.Write("S: ");
                        sw.Write(SaveSP[i]);
                        sw.Write(" RPM, ");
                        sw.Write("F: ");
                        sw.Write(SaveFeed[i]);
                        sw.WriteLine(" mm/min");
                    }
                }
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            indexProccessMode = 0;
            checkBox1.Checked = true;
            checkBox2.Checked = false;
            textBox2.Enabled = false;
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
                textBox2.Enabled = true;
                textBox4.Enabled = true;
            }

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text)) { }
            else
            {
                if (checkBox4.Checked == true) { }
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
        ///
        public void panelupdate(double[] A,double[] G)
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
            label48.Text = Convert.ToString(Math.Round(G[0],2,MidpointRounding.AwayFromZero)); 
            label49.Text = Convert.ToString(Math.Round(G[1],2,MidpointRounding.AwayFromZero)); 
            label50.Text = Convert.ToString(Math.Round(G[2], 2, MidpointRounding.AwayFromZero)); 
            label51.Text = Convert.ToString(Math.Round(G[3], 2, MidpointRounding.AwayFromZero)); 
            indexProgramState = 2;
            statepanel(indexProgramState);
            millingstart.Visible = true;
            millingstop.Visible = false;
            monitorbutton.Enabled = true;
            millingmodebutton.Enabled = true;
            //drillingmodebutton.Enabled = true;
            buttonSetting.Enabled = true;
            buttonExport.Enabled = true;
            aGauge1.Value = 0;
            Refresh();
        }
        public void panelupdate2(double[] Alist, double[] Glist)
        {
            double FeedPerFlute = Convert.ToDouble(textBox7.Text);
            double Nunber_Flute = Convert.ToDouble(textBox8.Text);
            label59.Text = Convert.ToString(Alist[0]); label60.Text = Convert.ToString(Alist[0] * FeedPerFlute * Nunber_Flute); label61.Text = Convert.ToString(Math.Round(Glist[0], 2, MidpointRounding.AwayFromZero));
            label62.Text = Convert.ToString(Alist[1]); label63.Text = Convert.ToString(Alist[1] * FeedPerFlute * Nunber_Flute); label64.Text = Convert.ToString(Math.Round(Glist[1], 2, MidpointRounding.AwayFromZero)); 
            label65.Text = Convert.ToString(Alist[2]); label66.Text = Convert.ToString(Alist[2] * FeedPerFlute * Nunber_Flute); label67.Text = Convert.ToString(Math.Round(Glist[2], 2, MidpointRounding.AwayFromZero)); 
            label68.Text = Convert.ToString(Alist[3]); label69.Text = Convert.ToString(Alist[3] * FeedPerFlute * Nunber_Flute); label70.Text = Convert.ToString(Math.Round(Glist[3], 2, MidpointRounding.AwayFromZero));
            label71.Text = Convert.ToString(Alist[4]); label72.Text = Convert.ToString(Alist[4] * FeedPerFlute * Nunber_Flute); label73.Text = Convert.ToString(Math.Round(Glist[4], 2, MidpointRounding.AwayFromZero)); 
            label74.Text = Convert.ToString(Alist[5]); label75.Text = Convert.ToString(Alist[5] * FeedPerFlute * Nunber_Flute); label76.Text = Convert.ToString(Math.Round(Glist[5], 2, MidpointRounding.AwayFromZero)); 
            label77.Text = Convert.ToString(Alist[6]); label78.Text = Convert.ToString(Alist[6] * FeedPerFlute * Nunber_Flute); label79.Text = Convert.ToString(Math.Round(Glist[6], 2, MidpointRounding.AwayFromZero)); 
            label80.Text = Convert.ToString(Alist[7]); label81.Text = Convert.ToString(Alist[7] * FeedPerFlute * Nunber_Flute); label82.Text = Convert.ToString(Math.Round(Glist[7], 2, MidpointRounding.AwayFromZero)); 
            label83.Text = Convert.ToString(Alist[8]); label84.Text = Convert.ToString(Alist[8] * FeedPerFlute * Nunber_Flute); label85.Text = Convert.ToString(Math.Round(Glist[8], 2, MidpointRounding.AwayFromZero)); 
            label86.Text = Convert.ToString(Alist[9]); label87.Text = Convert.ToString(Alist[9] * FeedPerFlute * Nunber_Flute); label88.Text = Convert.ToString(Math.Round(Glist[9], 2, MidpointRounding.AwayFromZero)); 
            label89.Text = Convert.ToString(Alist[10]); label90.Text = Convert.ToString(Alist[10] * FeedPerFlute * Nunber_Flute); label91.Text = Convert.ToString(Math.Round(Glist[10], 2, MidpointRounding.AwayFromZero)); 
            label92.Text = Convert.ToString(Alist[11]); label93.Text = Convert.ToString(Alist[11] * FeedPerFlute * Nunber_Flute); label94.Text = Convert.ToString(Math.Round(Glist[11], 2, MidpointRounding.AwayFromZero));
            indexProgramState = 2;
            statepanel(indexProgramState);
            millingstart.Visible = true;
            millingstop.Visible = false;
            monitorbutton.Enabled = true;
            millingmodebutton.Enabled = true;
            //drillingmodebutton.Enabled = true;
            buttonSetting.Enabled = true;
            buttonExport.Enabled = true;
            aGauge1.Value = 0;
            Refresh();
        }
        public void vibrationmonitor()
        {
            time_chart.Series[0].Points.Clear();
            time_chart.Series[1].Points.Clear();
            time_chart.Series[2].Points.Clear();
            time_chart.Series[3].Points.Clear();
            time_chart.Series[4].Points.Clear();

            for (int i = 0; i < nidaq.time.Length; i++)
            {
                time_chart.Series[0].Enabled = true;
                time_chart.Series[0].Points.AddXY(Math.Round((nidaq.time[i] - nidaq.starttime), 2), nidaq.plotrms[i]);
                time_chart.Series[1].Enabled = true;
                time_chart.Series[1].Points.AddXY(Math.Round((nidaq.time[i] - nidaq.starttime), 2), nidaq.alarm);
                time_chart.Series[2].Enabled = true;
                time_chart.Series[2].Points.AddXY(Math.Round((nidaq.time[i] - nidaq.starttime), 2), nidaq.warning);
                time_chart.Series[3].Enabled = true;
                time_chart.Series[3].Points.AddXY(Math.Round((nidaq.time[i] - nidaq.starttime), 2), nidaq.maxrms[0]);
                time_chart.ChartAreas[0].AxisY.Minimum = 0;
                time_chart.ChartAreas[0].AxisY.Maximum = 5;
            }
            if (nidaq.maxrms[0] >= 5)
            {
                nidaq.maxrms[0] = 5;
            }
            //紅綠燈 可變
            if (nidaq.allrms1 >= nidaq.alarm)//maxrms[0] >= alarm  allrms1
            {
                redlight.Visible = true;
                greenlight.Visible = false;
                yellowlight.Visible = false;
            }
            else if (nidaq.allrms1 >= nidaq.warning && nidaq.allrms1 < nidaq.alarm)//maxrms[0] >= warning && maxrms[0] < alarm
            {
                redlight.Visible = false;
                greenlight.Visible = false;
                yellowlight.Visible = true;
            }
            else
            {
                redlight.Visible = false;
                greenlight.Visible = true;
                yellowlight.Visible = false;
            }
            aGauge1.Value = Convert.ToSingle(nidaq.allrms1);
        }

        private void millingstart_Click(object sender, EventArgs e)
        {
            millingstart.Visible = false;
            millingstop.Visible = true;
            indexProgramState = 1;
            monitorbutton.Enabled = false;
            millingmodebutton.Enabled = false;
            drillingmodebutton.Enabled = false;
            buttonSetting.Enabled = false;
            buttonExport.Enabled = false;
            statepanel(indexProgramState);
            Refresh();

            OperatingSPmax = Convert.ToDouble(textBox4.Text);
            nidaq.StartDAQ(OperatingSPmax);

        }

        private void millingstop_Click(object sender, EventArgs e)
        {
            nidaq.StopDAQ();
            millingstart.Visible = true;
            millingstop.Visible = false;
            monitorbutton.Enabled = true;
            millingmodebutton.Enabled = true;
            //drillingmodebutton.Enabled = true;
            indexProgramState = 2;
            buttonSetting.Enabled = true;
            buttonExport.Enabled = true;
            statepanel(indexProgramState);
            Refresh();
            aGauge1.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = Application.StartupPath;////

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = "c:\\";
            sfd.Filter = "txt files (*.txt)|*.txt";
            sfd.ShowDialog();
            string saveNC = sfd.FileName;

            if (string.IsNullOrEmpty(saveNC)) { }
            else
            {
                EditNC nc = new EditNC();
                double maxSP = Convert.ToDouble(textBox4.Text);
                double CuttingDepth = Convert.ToDouble(textBox3.Text);
                double ToolNumber = Convert.ToDouble(textBox5.Text);
                double mmperflute = Convert.ToDouble(textBox7.Text);
                double flutenumber = Convert.ToDouble(textBox8.Text);
                double Xhalflength = Convert.ToDouble(textBox14.Text) / 2;
                double Yhalflength = Convert.ToDouble(textBox15.Text) / 2;
                double CuttingWidth = Convert.ToDouble(textBox16.Text);
                double Tooldiameter = Convert.ToDouble(textBox6.Text);
                nc.editNC(maxSP, CuttingDepth, ToolNumber, mmperflute, flutenumber, Xhalflength, Yhalflength, CuttingWidth, Tooldiameter, s, saveNC);  //textbox4: maximum spindle speed, textbox3: cutting depth, textbox5: tool number
                Thread.Sleep(1000);
            }
        }
        private void checkBox3_Click(object sender, EventArgs e)
        {//品質checkbox
            checkBox3.Checked = true;
            checkBox4.Checked = false;
            
            //textBox4.Text = Convert.ToString(Convert.ToDouble(textBox1.Text) * 0.9);
            //if (Convert.ToDouble(textBox4.Text) - (varNtest - 1) * 250 > 0)
            //{
            //    textBox2.Text = Convert.ToString(Convert.ToDouble(textBox4.Text) - (varNtest - 1) * 250);
            //}
            //else
            //{
            //    textBox2.Text = "0";
            //}

        }

        private void checkBox4_Click(object sender, EventArgs e)
        {//效率checkbox  
            checkBox3.Checked = false;
            checkBox4.Checked = true;
           
            
          
        }

        private void CUTeffi_FormClosed(object sender, FormClosedEventArgs e)
        {
            FSS.Close();
        }

        private void CUTeffi_Shown(object sender, EventArgs e)
        {
            FSS.Hide();
        }
    }
}
