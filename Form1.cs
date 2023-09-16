using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace No_Mini_EA
{
    public partial class Form1 : Form
    {
        private readonly NotifyIcon notifyIcon;
        private readonly StartUp startup = new StartUp();
        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Minimized;
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Text = "No Mini EA";
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, Exit_Click);
            notifyIcon.ContextMenuStrip.Items.Add("Auto start", null, startup.Auto_Start_Click);
        }


        private void Exit_Click(object sender, EventArgs e)
        {Close();}

        private void Form1_Load(object sender, EventArgs e)
        {
             Hide();
             notifyIcon.Visible = true;
            Timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("EADesktop");
            foreach (Process process in processes)
            {
                if (string.IsNullOrEmpty(process.MainWindowTitle) && process.MainWindowHandle == IntPtr.Zero)
                  {
                    TimeSpan uptime = DateTime.Now - process.StartTime;
                    int desiredUptimeInSeconds = 11;

                    if (uptime.TotalSeconds >= desiredUptimeInSeconds)
                    {ProcessKill();}
                }
            }
        }
        private void ProcessKill()
        {
            Process[] processes = Process.GetProcessesByName("EADesktop");
            foreach (Process process in processes)
            {
                try
                {
                    process.Kill();
                 //   Console.WriteLine($"Terminated process: {process.ProcessName} (ID: {process.Id})");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error terminating process: {ex.Message}");
                }
            }
}
}
}

