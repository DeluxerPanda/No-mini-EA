using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;

using System.Windows.Forms;

namespace No_Mini_EA
{
    public partial class Form1 : Form
    {
        const int SW_SHOWMINIMIZED = 2;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);
        private NotifyIcon notifyIcon;

        public Form1()
        {
            InitializeComponent();

            WindowState = FormWindowState.Minimized;
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Text = "No Mini EA";
            notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Exit", null, Exit_Click);
            notifyIcon.ContextMenuStrip.Items.Add("Auto start", null, Auto_Start_Click);


         
         
        }



        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Auto_Start_Click(object sender, EventArgs e)
        {
            string programName = "No_Mini_EA";
            if (IsProgramInCurrentUserStartup(programName))
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (key != null)
                    {
                        key.DeleteValue(programName, false);
                    }
                }
            }
            else
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (key != null)
                    {
                        key.SetValue(programName, "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"");
                    }
                }
            }

        }
        static bool IsProgramInCurrentUserStartup(string programName)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"))
            {
                if (key != null)
                {
                    string[] valueNames = key.GetValueNames();
                    foreach (string valueName in valueNames)
                    {
                        if (valueName.Equals(programName, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

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
            Console.WriteLine("TIMER WORKS 1 "+ randomNumber);
            foreach (Process process in processes)
            {
                Console.WriteLine("TIMER WORKS 2 " + randomNumber);
                if (string.IsNullOrEmpty(process.MainWindowTitle) && process.MainWindowHandle == IntPtr.Zero)
                  {
                    TimeSpan uptime = DateTime.Now - process.StartTime;
                    int desiredUptimeInSeconds = 20;
                    Console.WriteLine("TIMER WORKS 3 " + randomNumber);

                    if (uptime.TotalSeconds >= desiredUptimeInSeconds)
                    {
                        Console.WriteLine("60 SECUND AND PROGRAM CLOSES");
                        process.Kill();
                    }
                }
            }
        }
    }
}

