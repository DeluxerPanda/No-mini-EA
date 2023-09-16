using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace No_Mini_EA
{
    public partial class Form1 : Form
    {
        private NotifyIcon notifyIcon;
        private StartUp startup = new StartUp();

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
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random random = new Random();
            int randomNumber = random.Next(1, 101);
            Process[] processes = Process.GetProcessesByName("EADesktop");
           // Console.WriteLine("TIMER WORKS 1 "+ randomNumber);
            foreach (Process process in processes)
            {
              //  Console.WriteLine("TIMER WORKS 2 " + randomNumber);
                if (string.IsNullOrEmpty(process.MainWindowTitle) && process.MainWindowHandle == IntPtr.Zero)
                  {
                    TimeSpan uptime = DateTime.Now - process.StartTime;
                    int desiredUptimeInSeconds = 20;
                 //   Console.WriteLine("TIMER WORKS 3 " + randomNumber);

                    if (uptime.TotalSeconds >= desiredUptimeInSeconds)
                    {
                     //   Console.WriteLine("60 SECUND AND PROGRAM CLOSES");
                        ProcessKill();
                    }
                }
            }
        }
        private void ProcessKill()
        {
            foreach (var processkill in Process.GetProcessesByName("EADesktop"))
            {
                // MessageBox.Show("ehj");
               var desc = FileVersionInfo.GetVersionInfo(processkill.MainModule.FileName);
                Console.WriteLine(desc.FileDescription);
             
                  processkill.Kill();
            }
        }
    }
}

