using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Management;
using System.Threading;

namespace MoYu
{
    /// <summary>
    /// Windows10.xaml 的交互逻辑
    /// </summary>
    public partial class WindowsUpdate : Window
    {
        Control View = null;
        public WindowsUpdate(System.Windows.Forms.Screen scr,Model.UpdateProgress updateProgress)
        {
            InitializeComponent();

            Left = scr.Bounds.Left;
            Top = scr.Bounds.Top;

            Topmost = true;

            if (scr.Primary)
            {
                int? windowsVersion = GetWindowsMajorVersion();


                if (windowsVersion == 7)
                {
                    View = new Pages.Update5();
                }
                else if (windowsVersion == 10)
                {
                    View = new Pages.Update2();
                }
                else
                {
                    View = new Pages.Update1();
                }


                this.BACK.Child = View;
                View.DataContext = updateProgress;

                if (windowsVersion != 7)
                    ShowCursor(0);
            }
            else
            { 
                this.BACK.Background=new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
 

        }



        private int? GetWindowsMajorVersionByWMI()
        {
            try
            {
                ManagementObjectSearcher os_searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject mobj in os_searcher.Get())
                {
                    string? Temp = mobj["Caption"].ToString();

                    if (Temp == null) return null;
                    string[] Sub = Temp.Split(' ');

                    if (int.TryParse(Sub[2], out int value))
                        return value;
                    else return null;
                }
            }
            catch (Exception) { }
            return null;
        }
        private int? GetWindowsMajorVersionByEnvironment()
        {
            OperatingSystem operatingSystem = Environment.OSVersion;
            int Major = operatingSystem.Version.Major;
            int Minor = operatingSystem.Version.Minor;
            int Build = operatingSystem.Version.Build;
            if (Major == 6)
            {
                if (Minor == 1)
                    return 7;
                else if (Minor == 2 || Minor == 3)
                    return 8;
            }
            else if (Major == 10)
            {
                return 10;
            }

            return null;
        }

        private int? GetWindowsMajorVersion()
        {
            int? Major = GetWindowsMajorVersionByWMI();
            if (Major != null)
                return Major;
            else
                return GetWindowsMajorVersionByEnvironment();

        }


        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        public extern static void ShowCursor(int status);

    }
}