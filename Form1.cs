using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace No_Mini_EA
{
    public partial class Form1 : Form
    {   
        private readonly NotifyIcon notifyIcon;
        private readonly StartUp startup = new StartUp();
        private readonly String processName = "EADesktop";
        // private readonly Image image_Exit = Image.FromFile("C:\\Users\\DeluxerPanda\\source\\repos\\DeluxPanda\\No_Mini_EA\\Images\\close-button-png-30235-Windows.ico");
        //private readonly Image image_Auto_Start = Image.FromFile("C:\\Users\\DeluxerPanda\\source\\repos\\DeluxPanda\\No_Mini_EA\\Images\\windows-icon-42341-Windows.ico");
        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Minimized;
            notifyIcon = new NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath),
                Text = "No Mini EA",
                ContextMenuStrip = new ContextMenuStrip()
            };
            notifyIcon.ContextMenuStrip.Items.Add("Auto start", null, startup.Auto_Start_Click);
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, Exit_Click);
            notifyIcon.MouseClick += NotifyIcon_MouseClick;
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Middle)
            { notifyIcon.ContextMenuStrip.Show(Cursor.Position); }
        }


        private void Exit_Click(object sender, EventArgs e)
        { Close(); }

        private void Form1_Load(object sender, EventArgs e)
        {
            Hide();
            notifyIcon.Visible = true;
            Timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                if (string.IsNullOrEmpty(process.MainWindowTitle) && process.MainWindowHandle == IntPtr.Zero)
                {
                    TimeSpan uptime = DateTime.Now - process.StartTime;
                    int desiredUptimeInSeconds = 11;

                    if (uptime.TotalSeconds >= desiredUptimeInSeconds)
                    { ProcessKill(); }
                }
            }
        }
        private void ProcessKill()
        {
            try
            {

                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length == 1)
                {
                    processes[0].Kill();
                }
                else
                {
                   // MessageBox.Show("Process not found.");
                }
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode != 0)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }   
    }
}
