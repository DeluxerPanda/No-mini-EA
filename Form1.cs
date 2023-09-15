using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private Timer processCheckTimer;
        private NotifyIcon notifyIcon;

        public Form1()
        {
            InitializeComponent();
            InitializeProcessCheckTimer();

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
            processCheckTimer.Start();
        }

        private void InitializeProcessCheckTimer()
        {
            processCheckTimer = new Timer();
            processCheckTimer.Interval = 11000; // Set the interval in milliseconds (e.g., 60 seconds).
            processCheckTimer.Tick += ProcessCheckTimer_Tick;
        }

        private void ProcessCheckTimer_Tick(object sender, EventArgs e)
        {
            CheckAndKillEADesktop();
        }

        private void CheckAndKillEADesktop()
        {

            Process[] processes = Process.GetProcessesByName("EADesktop");

            foreach (Process process in processes)
            {
                if (string.IsNullOrEmpty(process.MainWindowTitle) && process.MainWindowHandle == IntPtr.Zero)
                {
                    process.Kill();
                }
                System.Threading.Thread.Sleep(11000);
            }
        }
    }
}

