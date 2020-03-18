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
        public double alarm_start,warning_start,normal_start, alarm_stop, warning_stop, normal_stop;
        public static int varNtest = 12;
        public static double threshold, time_chart_max,alarm_threshold,warning_threshold;
        public static double[] A = new double[12],Alist = new double[12],Glist = new double[12];
        public static Panel Panel1;
        public static CheckBox checkBox_quality, checkBox_effect;
        nidaqAPI nidaq = new nidaqAPI();
        

        public CUTeffiForm()
        {
            InitializeComponent();
            this.Size = new Size(795, 600);
            panel1.Visible = true;
            Panel1 = this.panel1;
            initialCUTeffi();
        }

        private void monitorbutton_Click(object sender, EventArgs e)
        {
            panel11.Visible = false;
            panel13.Visible = false;
            panel3.Visible = false;
            panel1.Visible = true;
            panelSetting.Visible = false;
            buttonSetting.Visible = false;
            buttonExport.Visible = false;
            indexPanelSetting = 0;
            openhistorydata_button.Enabled = true;
            chart_maximum.Enabled = true;
            Alarm_threshold.Enabled = true;
            Warning_threshold.Enabled = true;
            label57.Visible = true; label58.Visible = true; label95.Visible = true; chart_maximum.Visible = true; Alarm_threshold.Visible = true; Warning_threshold.Visible = true;
            time_chart.Series[0].Points.Clear();
            time_chart.Series[1].Points.Clear();
            time_chart.Series[2].Points.Clear();
            time_chart.Series[3].Points.Clear();
            alarm_chart.Series[0].Points.Clear();
            alarm_chart.Series[1].Points.Clear();
            alarm_chart.Series[2].Points.Clear();
        }
        private void millingmodebutton_Click(object sender, EventArgs e)
        {
            panel11.Location = new Point(352, 75);
            panel11.Visible = true;
            panel3.Location = new Point(0, 75);
            panel3.Visible = true;
            panel1.Visible = false;
            buttonSetting.Visible = true;
            buttonExport.Visible = true;
            buttonSetting.Location = new Point(352, 507);
            buttonExport.Location = new Point(518, 507);
            panel13.Location = new Point(352, 457);
            panel13.Visible = true;
            millingstart.Visible = true;
            millingstop.Visible = false;
            chart_maximum.Enabled = false;
            Alarm_threshold.Enabled = false;
            Warning_threshold.Enabled = false;
            openhistorydata_button.Enabled = false;
            label57.Visible = false;label58.Visible = false;label95.Visible = false;
            chart_maximum.Visible = false;
            Alarm_threshold.Visible = false;
            Warning_threshold.Visible = false;
        }

        private void initialCUTeffi()
        {
            indexProgramState = 0;
            statepanel(indexProgramState);

            textBox4.Text = Convert.ToString(Convert.ToDouble(textBox1.Text) * 0.9);
            textBox2.Text = Convert.ToString(Convert.ToDouble(textBox4.Text) - (varNtest - 1) * 250);
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
            openhistorydata_button.Enabled = false;
            buttonSetting.Enabled = false;
            buttonExport.Enabled = false;
            chart_maximum.Enabled = false;
            Alarm_threshold.Enabled = false;
            Warning_threshold.Enabled = false;
            time_chart_max = Convert.ToDouble(chart_maximum.Text);
            alarm_threshold = Convert.ToDouble(Alarm_threshold.Text);
            warning_threshold = Convert.ToDouble(Warning_threshold.Text);
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
            indexProgramState = 2;
            buttonSetting.Enabled = true;
            buttonExport.Enabled = true;
            openhistorydata_button.Enabled = true;
            chart_maximum.Enabled = true;
            Alarm_threshold.Enabled = true;
            Warning_threshold.Enabled = true;
            statepanel(indexProgramState);
            alarm_start = 0;
            warning_start = 0;
            normal_start = 0;
            alarm_stop = 0;
            warning_stop = 0;
            normal_stop = 0;
            Refresh();
            

        }
        private void buttonSetting_Click(object sender, EventArgs e)
        {
            if (indexPanelSetting == 0)
            {
                panelSetting.Visible = true;
                panel11.Visible = false;
                panel13.Visible = false;
                indexPanelSetting = 1;
                panelSetting.Location = new Point(352, 75);
            }
            else
            {
                panelSetting.Visible = false;
                panel11.Visible = true;
                panel13.Visible = true;
                indexPanelSetting = 0;
                //if (checkBox3.Checked == true && checkBox4.Checked ==false)
                //{
                //    panel11.Visible = false;
                //    panel6.Visible = true;
                //    panel6.Location = new Point(352, 75);
                //}
                //else
                //{
                //    panel6.Visible = false;
                //    panel11.Visible = true;
                //    panel11.Location = new Point(352, 75);
                //}
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            //if (checkBox_quality.Checked == true && checkBox_effect.Checked == false)
            //{
            //    string[] SaveSP = new string[4];
            //    string[] SaveFeed = new string[4];
            //    string[] SaveG = new string[4];
            //    SaveSP[0] = label24.Text;
            //    SaveSP[1] = label4.Text;
            //    SaveSP[2] = label5.Text;
            //    SaveSP[3] = label6.Text;
            //    SaveFeed[0] = label25.Text;
            //    SaveFeed[1] = label26.Text;
            //    SaveFeed[2] = label27.Text;
            //    SaveFeed[3] = label28.Text;
            //    SaveG[0] = label48.Text;
            //    SaveG[1] = label49.Text;
            //    SaveG[2] = label50.Text;
            //    SaveG[3] = label51.Text;
            //    SaveFileDialog sfd = new SaveFileDialog();
            //    sfd.InitialDirectory = "c:\\";
            //    sfd.Filter = "txt files (*.txt)|*.txt";
            //    sfd.ShowDialog();
            //    string saveFileName = sfd.FileName;
            //    if (string.IsNullOrEmpty(saveFileName)) { }
            //    else
            //    {
            //        using (StreamWriter sw = new StreamWriter(saveFileName))
            //        {
            //            sw.Write("刀號: ");
            //            sw.WriteLine(textBox5.Text);
            //            sw.Write("刀具直徑: ");
            //            sw.Write(textBox6.Text);
            //            sw.WriteLine(" mm");
            //            sw.WriteLine("");
            //            sw.WriteLine("建議參數");
            //            sw.Write("S: ");
            //            sw.Write(SaveSP[0]);
            //            sw.Write(" RPM, ");
            //            sw.Write("F: ");
            //            sw.Write(SaveFeed[0]);
            //            sw.Write(" mm/min,");
            //            sw.Write("Vibration: ");
            //            sw.Write(SaveG[0]);
            //            sw.WriteLine(" g, ");
            //            sw.WriteLine("");
            //            sw.WriteLine("其他參數");
            //            for (int i = 1; i < 4; i++)
            //            {
            //                sw.Write("S: ");
            //                sw.Write(SaveSP[i]);
            //                sw.Write(" RPM, ");
            //                sw.Write("F: ");
            //                sw.Write(SaveFeed[i]);
            //                sw.Write(" mm/min,");
            //                sw.Write("Vibration: ");
            //                sw.Write(SaveG[i]);
            //                sw.WriteLine(" g, ");
            //            }
            //        }
            //    }
            //}
            //else
            //{
                string[] SaveSP = new string[12];
                string[] SaveFeed = new string[12];
                string[] SaveG = new string[12];
                SaveSP[0] = label59.Text; SaveSP[4] = label71.Text; SaveSP[8] = label83.Text;
                SaveSP[1] = label62.Text; SaveSP[5] = label74.Text; SaveSP[9] = label86.Text;
                SaveSP[2] = label65.Text; SaveSP[6] = label77.Text; SaveSP[10] = label89.Text;
                SaveSP[3] = label68.Text; SaveSP[7] = label80.Text; SaveSP[11] = label92.Text;
                SaveFeed[0] = label60.Text; SaveFeed[4] = label72.Text; SaveFeed[8] = label84.Text;
                SaveFeed[1] = label63.Text; SaveFeed[5] = label75.Text; SaveFeed[9] = label87.Text;
                SaveFeed[2] = label66.Text; SaveFeed[6] = label78.Text; SaveFeed[10] = label90.Text;
                SaveFeed[3] = label69.Text; SaveFeed[7] = label81.Text; SaveFeed[11] = label93.Text;
                SaveG[0] = label61.Text; SaveG[4] = label73.Text; SaveG[8] = label85.Text;
                SaveG[1] = label64.Text; SaveG[5] = label76.Text; SaveG[9] = label88.Text;
                SaveG[2] = label67.Text; SaveG[6] = label79.Text; SaveG[10] = label91.Text;
                SaveG[3] = label70.Text; SaveG[7] = label82.Text; SaveG[11] = label94.Text;
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
                        for (int i = 0; i < 12; i++)
                        {
                            sw.Write("S: ");
                            sw.Write(SaveSP[i]);
                            sw.Write(" RPM, ");
                            sw.Write("F: ");
                            sw.Write(SaveFeed[i]);
                            sw.Write(" mm/min,");
                            sw.Write("Vibration: ");
                            sw.Write(SaveG[i]);
                            sw.WriteLine(" g, ");
                        }
                    }
                }
            //}
            
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
                textBox2.Enabled = false;
                textBox4.Enabled = true;
            }

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text)) { }
            else
            {
                //if (checkBox4.Checked == true) { }
                //else
                //{
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
                //}
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
        //public void panelupdate(double[] A, double[] G)
        //{
        //    label24.Text = Convert.ToString(A[0]);
        //    label4.Text = Convert.ToString(A[1]);
        //    label5.Text = Convert.ToString(A[2]);
        //    label6.Text = Convert.ToString(A[3]);
        //    double FeedPerFlute = Convert.ToDouble(textBox7.Text);
        //    double Nunber_Flute = Convert.ToDouble(textBox8.Text);
        //    label25.Text = Convert.ToString(A[0] * FeedPerFlute * Nunber_Flute);
        //    label26.Text = Convert.ToString(A[1] * FeedPerFlute * Nunber_Flute);
        //    label27.Text = Convert.ToString(A[2] * FeedPerFlute * Nunber_Flute);
        //    label28.Text = Convert.ToString(A[3] * FeedPerFlute * Nunber_Flute);
        //    label48.Text = Convert.ToString(Math.Round(G[0], 2, MidpointRounding.AwayFromZero));
        //    label49.Text = Convert.ToString(Math.Round(G[1], 2, MidpointRounding.AwayFromZero));
        //    label50.Text = Convert.ToString(Math.Round(G[2], 2, MidpointRounding.AwayFromZero));
        //    label51.Text = Convert.ToString(Math.Round(G[3], 2, MidpointRounding.AwayFromZero));
        //    indexProgramState = 2;
        //    statepanel(indexProgramState);
        //    millingstart.Visible = true;
        //    millingstop.Visible = false;
        //    monitorbutton.Enabled = true;
        //    millingmodebutton.Enabled = true;
        //    drillingmodebutton.Enabled = true;
        //    buttonSetting.Enabled = true;
        //    buttonExport.Enabled = true;
        //    aGauge1.Value = 0;
        //    Refresh();
        //}
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
            buttonSetting.Enabled = true;
            buttonExport.Enabled = true;
            aGauge1.Value = 0;
            Refresh();
        }

        private void Feed_choise_Click(object sender, EventArgs e)
        {
            nidaq.SPOptimization(2);
        }

        private void Quality_choise_Click(object sender, EventArgs e)
        {
            nidaq.SPOptimization(1);
        }

        public void vibrationmonitor()
        {
            time_chart.Series[0].Points.Clear();
            time_chart.Series[1].Points.Clear();
            time_chart.Series[2].Points.Clear();
            time_chart.Series[3].Points.Clear();
            time_chart.ChartAreas[0].AxisY.Minimum = 0;
            time_chart.ChartAreas[0].AxisY.Maximum = time_chart_max;
            time_chart.ChartAreas[0].AxisX.Minimum = Math.Round((nidaq.time[0] - nidaq.starttime), 2);
            time_chart.ChartAreas[0].AxisX.Maximum = Math.Round((nidaq.time[0] - nidaq.starttime), 2) + 10;
            for (int i = 0; i < nidaq.time.Length; i++)
            {
                time_chart.Series[0].Enabled = true;
                time_chart.Series[0].Points.AddXY(Math.Round((nidaq.time[i] - nidaq.starttime), 2), nidaq.plotrms[i]);
                time_chart.Series[1].Enabled = true;
                time_chart.Series[1].Points.AddXY(Math.Round((nidaq.time[i] - nidaq.starttime), 2), Convert.ToDouble(Alarm_threshold.Text));
                time_chart.Series[2].Enabled = true;
                time_chart.Series[2].Points.AddXY(Math.Round((nidaq.time[i] - nidaq.starttime), 2), Convert.ToDouble(Warning_threshold.Text));
                time_chart.Series[3].Enabled = true;
                time_chart.Series[3].Points.AddXY(Math.Round((nidaq.time[i] - nidaq.starttime), 2), nidaq.maxrms[0]);
            }
            if (nidaq.maxrms[0] >= time_chart_max)
            {
                nidaq.maxrms[0] = time_chart_max;
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
            buttonSetting.Enabled = false;
            buttonExport.Enabled = false;
            statepanel(indexProgramState);
            label59.Text = " "; label60.Text = " "; label61.Text = " "; label62.Text = " "; label63.Text = " "; label64.Text = " "; label65.Text = " "; label66.Text = " "; label67.Text = " "; label68.Text = " ";
            label69.Text = " "; label70.Text = " "; label71.Text = " "; label72.Text = " "; label73.Text = " "; label74.Text = " "; label75.Text = " "; label76.Text = " "; label77.Text = " "; label78.Text = " ";
            label79.Text = " "; label80.Text = " "; label81.Text = " "; label82.Text = " "; label83.Text = " "; label84.Text = " "; label85.Text = " "; label86.Text = " "; label87.Text = " "; label88.Text = " ";
            label89.Text = " "; label90.Text = " "; label91.Text = " "; label92.Text = " "; label93.Text = " "; label94.Text = " ";
            Refresh();
            threshold = Convert.ToDouble(Measure_threshold.Text);
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
                double Cuttingcoordinate = Convert.ToDouble(cutting_coordinate.Text);
                nc.editNC(maxSP, CuttingDepth, ToolNumber, mmperflute, flutenumber, Xhalflength, Yhalflength, CuttingWidth, Tooldiameter, Cuttingcoordinate, s, saveNC);  //textbox4: maximum spindle speed, textbox3: cutting depth, textbox5: tool number
                Thread.Sleep(1000);
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
        public void ForAlarmChart()
        {
            alarm_chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            alarm_chart.ChartAreas[0].AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            alarm_chart.Series[0].Points.Clear();
            alarm_chart.Series[1].Points.Clear();
            alarm_chart.Series[2].Points.Clear();
            alarm_chart.ChartAreas[0].AxisX.Minimum = -1;
            alarm_chart.ChartAreas[0].AxisX.Maximum = 3;
            alarm_chart.ChartAreas[0].AxisY.Minimum = Math.Round((nidaq.time[0] - nidaq.starttime), 2);
            alarm_chart.ChartAreas[0].AxisY.Maximum = Math.Round((nidaq.time[0] - nidaq.starttime), 2) + 10;
            double[] alarmindex = new double[nidaq.time.Length], warningindex = new double[nidaq.time.Length], normalindex = new double[nidaq.time.Length];
            for (int i = 0; i < nidaq.time.Length; i++)
            {
                if (nidaq.plotrms[i] >= nidaq.alarm)
                {
                    alarmindex[i] = Math.Round((nidaq.time[i] - nidaq.starttime), 2);
                }
                if (nidaq.plotrms[i] >= nidaq.warning && nidaq.plotrms[i] < nidaq.alarm)
                {
                    warningindex[i] = Math.Round((nidaq.time[i] - nidaq.starttime), 2);
                }
                if (nidaq.plotrms[i] < nidaq.warning)
                {
                    normalindex[i] = Math.Round((nidaq.time[i] - nidaq.starttime), 2);
                }
            }
            for (int i = 0; i < alarmindex.Length; i++)
            {
                alarm_chart.Series[0].Points.AddXY(2, alarmindex[i] , alarmindex[i] + 0.1 );
                alarm_chart.Series[1].Points.AddXY(2, alarmindex[i], alarmindex[i] + 0.1);
                alarm_chart.Series[2].Points.AddXY(2, alarmindex[i], alarmindex[i] + 0.1);
                alarm_chart.Series[0].Points.AddXY(1, warningindex[i], warningindex[i] + 0.1);
                alarm_chart.Series[1].Points.AddXY(1, warningindex[i], warningindex[i] + 0.1);
                alarm_chart.Series[2].Points.AddXY(1, warningindex[i], warningindex[i] + 0.1);
                alarm_chart.Series[0].Points.AddXY(0, normalindex[i], normalindex[i] + 0.1);
                alarm_chart.Series[1].Points.AddXY(0, normalindex[i], normalindex[i] + 0.1);
                alarm_chart.Series[2].Points.AddXY(0, normalindex[i], normalindex[i] + 0.1);
            }
            alarm_chart.Series[0].Points[1].Color = Color.Red;
            alarm_chart.Series[1].Points[1].Color = Color.Orange; 
            alarm_chart.Series[2].Points[1].Color = Color.Green;
            for (int i = 0; i < alarm_chart.Series[2].Points.Count; i += 3)
            {
                alarm_chart.Series[0].Points[i].Color = Color.Red;
                alarm_chart.Series[1].Points[i].Color = Color.Red;
                alarm_chart.Series[2].Points[i].Color = Color.Red;
                alarm_chart.Series[0].Points[i+1].Color = Color.Orange;
                alarm_chart.Series[1].Points[i+1].Color = Color.Orange;
                alarm_chart.Series[2].Points[i+1].Color = Color.Orange;
                alarm_chart.Series[0].Points[i+2].Color = Color.Green;
                alarm_chart.Series[1].Points[i+2].Color = Color.Green;
                alarm_chart.Series[2].Points[i+2].Color = Color.Green;
            }            
        }
        private void openhistorydata_button_Click(object sender, EventArgs e)
        {
            alarm_threshold = Convert.ToDouble(Alarm_threshold.Text);
            warning_threshold = Convert.ToDouble(Warning_threshold.Text);
            //time_chart.Series.Clear();
            time_chart.Series[0].Points.Clear();
            time_chart.Series[1].Points.Clear();
            time_chart.Series[2].Points.Clear();
            time_chart.Series[3].Points.Clear();
            alarm_chart.Series[0].Points.Clear();
            alarm_chart.Series[1].Points.Clear();
            alarm_chart.Series[2].Points.Clear();
            load_historydata();
        }
        private void load_historydata()
        {
            string s =Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +"\\logdata";////
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.InitialDirectory = s;
            sfd.Filter = "txt files (*.txt)|*.txt";
            sfd.ShowDialog();
            string openhistory = sfd.FileName;
            int counter = 0;
            string line;
            if (string.IsNullOrEmpty(openhistory)) { }
            else
            {
                StreamReader history_data = new StreamReader(openhistory);
                List<List<double>> DataRead = new List<List<double>>();

                while ((line = history_data.ReadLine()) != null)
                {
                    string[] split;
                    split = line.Split(new char[] { ',' });
                    DataRead.Add(new List<double>() { Convert.ToDouble(split[0]), Convert.ToDouble(split[1])});//Convert.ToDouble(split[2]), Convert.ToDouble(split[3])
                    //Console.WriteLine(DataRead[counter]);
                    counter++;
                }
                double[] vecTime = new double[counter];
                double[] historydata = new double[counter];
                for (int i = 0; i < counter; i++)
                {
                    vecTime[i] = DataRead[i][0];
                    historydata[i] = DataRead[i][1];
                }
                for (int i = 0; i < counter; i++)
                {
                    time_chart.Series[0].Points.AddXY(vecTime[i]-vecTime[0], historydata[i]);
                }
                time_chart.ChartAreas[0].AxisY.Maximum = Double.NaN;
                time_chart.ChartAreas[0].AxisY.Minimum = Double.NaN;
                time_chart.ChartAreas[0].AxisX.Minimum = 0;
                time_chart.ChartAreas[0].AxisX.Maximum = vecTime[vecTime.Length-1] - vecTime[0];
                ///
                alarm_chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                alarm_chart.ChartAreas[0].AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
                alarm_chart.Series[0].Points.Clear();
                alarm_chart.Series[1].Points.Clear();
                alarm_chart.Series[2].Points.Clear();
                alarm_chart.ChartAreas[0].AxisX.Minimum = -1;
                alarm_chart.ChartAreas[0].AxisX.Maximum = 3;
                alarm_chart.ChartAreas[0].AxisY.Minimum = 0;
                alarm_chart.ChartAreas[0].AxisY.Maximum = Math.Round(vecTime[vecTime.Length - 1] - vecTime[0],2);
                double[] alarmindex = new double[vecTime.Length], warningindex = new double[vecTime.Length], normalindex = new double[vecTime.Length];
                for (int i = 0; i < vecTime.Length; i++)
                {
                    if (historydata[i] >= alarm_threshold)
                    {
                        alarmindex[i] = Math.Round(vecTime[i] - vecTime[0], 2);
                    }
                    if (historydata[i] >= warning_threshold && historydata[i] < alarm_threshold)
                    {
                        warningindex[i] = Math.Round(vecTime[i] - vecTime[0], 2);
                    }
                    if (historydata[i] < warning_threshold)
                    {
                        normalindex[i] = Math.Round(vecTime[i] - vecTime[0], 2);
                    }
                }
                for (int i = 0; i < alarmindex.Length; i++)
                {
                    alarm_chart.Series[0].Points.AddXY(2, alarmindex[i], alarmindex[i] + 0.1);
                    alarm_chart.Series[1].Points.AddXY(2, alarmindex[i], alarmindex[i] + 0.1);
                    alarm_chart.Series[2].Points.AddXY(2, alarmindex[i], alarmindex[i] + 0.1);
                    alarm_chart.Series[0].Points.AddXY(1, warningindex[i], warningindex[i] + 0.1);
                    alarm_chart.Series[1].Points.AddXY(1, warningindex[i], warningindex[i] + 0.1);
                    alarm_chart.Series[2].Points.AddXY(1, warningindex[i], warningindex[i] + 0.1);
                    alarm_chart.Series[0].Points.AddXY(0, normalindex[i], normalindex[i] + 0.1);
                    alarm_chart.Series[1].Points.AddXY(0, normalindex[i], normalindex[i] + 0.1);
                    alarm_chart.Series[2].Points.AddXY(0, normalindex[i], normalindex[i] + 0.1);
                }
                alarm_chart.Series[0].Points[1].Color = Color.Red;
                alarm_chart.Series[1].Points[1].Color = Color.Orange;
                alarm_chart.Series[2].Points[1].Color = Color.Green;
                for (int i = 0; i < alarm_chart.Series[2].Points.Count; i += 3)
                {
                    alarm_chart.Series[0].Points[i].Color = Color.Red;
                    alarm_chart.Series[1].Points[i].Color = Color.Red;
                    alarm_chart.Series[2].Points[i].Color = Color.Red;
                    alarm_chart.Series[0].Points[i + 1].Color = Color.Orange;
                    alarm_chart.Series[1].Points[i + 1].Color = Color.Orange;
                    alarm_chart.Series[2].Points[i + 1].Color = Color.Orange;
                    alarm_chart.Series[0].Points[i + 2].Color = Color.Green;
                    alarm_chart.Series[1].Points[i + 2].Color = Color.Green;
                    alarm_chart.Series[2].Points[i + 2].Color = Color.Green;
                }
            }
        }
      
    }
}
