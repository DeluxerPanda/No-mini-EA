using Microsoft.Win32;
using System;

namespace No_Mini_EA
{
    public  class StartUp
    {
        public void Auto_Start_Click(object sender, EventArgs e)
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
    }
}
