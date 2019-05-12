using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using OpenHardwareMonitor.Hardware;
using Microsoft.Win32;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            hScrollBar1.Minimum = 40;
            hScrollBar1.Maximum = 100;
            notifyIcon1.Visible = true;
            string[] portlar = SerialPort.GetPortNames(); // Bağlı seri portları diziye aktardık
            foreach (string portAdi in portlar)
            {
                comboBox1.Items.Add(portAdi);
            }
            try {comboBox1.SelectedIndex = 0;}
            catch { MessageBox.Show("Arduino bulunamadı"); }
        }


        private void seriPort(int tempcpu){
            SerialPort seriPort;
            seriPort = new SerialPort();
            seriPort.PortName = comboBox1.Text;
            seriPort.BaudRate = 9600;
            seriPort.Open();
            if (tempcpu > 79) seriPort.Write("w");
            else if (tempcpu > 78) seriPort.Write("e");
            else if (tempcpu > 77) seriPort.Write("r");
            else if (tempcpu > 76) seriPort.Write("t");
            else if (tempcpu > 75) seriPort.Write("y");
            else if (tempcpu > 74) seriPort.Write("u");
            else if (tempcpu > 73) seriPort.Write("o");
            else if (tempcpu > 72) seriPort.Write("p");
            else if (tempcpu > 71) seriPort.Write("a");
            else if (tempcpu > 70) seriPort.Write("s");
            else if (tempcpu > 69) seriPort.Write("d");
            else if (tempcpu > 68) seriPort.Write("f");
            else if (tempcpu > 67) seriPort.Write("g");
            else if (tempcpu > 66) seriPort.Write("h");
            else if (tempcpu > 65) seriPort.Write("j");
            else if (tempcpu > 64) seriPort.Write("k");
            else if (tempcpu > 63) seriPort.Write("l");
            else if (tempcpu > 62) seriPort.Write("i");
            else if (tempcpu > 61) seriPort.Write("z");
            else if (tempcpu > 60) seriPort.Write("x");
            else if (tempcpu > 59) seriPort.Write("c");
            else if (tempcpu > 58) seriPort.Write("v");
            else if (tempcpu > 57) seriPort.Write("b");
            else if (tempcpu > 56) seriPort.Write("n");
            else if (tempcpu > 55) seriPort.Write("m");
            else if (tempcpu > 54) seriPort.Write("0");
            else if (tempcpu > 53) seriPort.Write("1");
            else if (tempcpu > 52) seriPort.Write("2");
            else if (tempcpu > 51) seriPort.Write("3");
            else if (tempcpu > 50) seriPort.Write("4");
            else if (tempcpu > 48) seriPort.Write("5");
            else if (tempcpu > 46) seriPort.Write("6");
            else if (tempcpu > 44) seriPort.Write("7");
            else if (tempcpu > 42) seriPort.Write("8");
            else seriPort.Write("9");
            seriPort.Close();
}
        private int maxSicaklik = 0;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            Computer computer = new Computer() { CPUEnabled = true, GPUEnabled = false };
            Computer gpu = new Computer() { CPUEnabled = false, GPUEnabled = true };
            computer.Open();
            int tempcpu = 0;
            int tempgpu = 0;
            foreach (IHardware hardware in computer.Hardware)
            {
                hardware.Update();
                
                foreach (ISensor sensor in hardware.Sensors)
                {
                    // Celsius is default unit

                    if (sensor.SensorType == SensorType.Temperature)
                    {
                        tempcpu = (tempcpu + Convert.ToInt32(sensor.Value)) / 2;
                    }
                }
                label1.Text ="Cpu Sıcaklığı = " + Convert.ToString(tempcpu);

            }

            gpu.Open();
            foreach (IHardware hardware in gpu.Hardware)
            {
                hardware.Update();

                foreach (ISensor sensor in hardware.Sensors)
                {
                    // Celsius is default unit
                    if (sensor.SensorType == SensorType.Temperature)
                    {
                        tempgpu = Convert.ToInt32(sensor.Value);
                    }
                }
                label3.Text = "Gpu Sıcaklığı = " + Convert.ToString(tempgpu);
            }
            label4.Text = "  Ortalama sıcaklık " + ((tempcpu + tempgpu) / 2);

            if (tempcpu > tempgpu)
            {
                if (tempcpu > maxSicaklik) maxSicaklik = tempcpu;
            }
            else
            {
                if (tempgpu > maxSicaklik) maxSicaklik = tempgpu;
            }
            label4.Text = label4.Text + "\nMaxiumum Sıcaklık " + Convert.ToString(maxSicaklik);
            if (tempcpu > 75 || tempgpu > 75)
                {
                    NotifyBildirim("Yüksek sıcaklık değeri", "Lütfen sıcaklığı kontrol edin");
                }
            if (button2.Enabled == false)
            {
                if (tempcpu > tempgpu) seriPort(tempcpu);
                else seriPort(tempgpu);
            }
            if (FormWindowState.Minimized == this.WindowState) this.Hide();
            notifyIcon1.Text = "CPU Temp: " + Convert.ToString(tempcpu) + " GPU Temp : " + Convert.ToString(tempgpu);
        }

        private void NotifyBildirim(string baslik, string mesaj)
        {
            notifyIcon1.BalloonTipText = mesaj;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipTitle = baslik;
            notifyIcon1.ShowBalloonTip(3000);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SerialPort seriPort;
            seriPort = new SerialPort();
            seriPort.PortName = comboBox1.Text;
            seriPort.BaudRate = 9600;
            seriPort.Open();
            seriPort.Write("q");
            seriPort.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            hScrollBar1.Enabled = true;
        }

        private void HScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            seriPort(hScrollBar1.Value);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            timer1.Start();
            button1.Enabled = true;
            button2.Enabled = false;
            hScrollBar1.Enabled = false;
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

    }
}
