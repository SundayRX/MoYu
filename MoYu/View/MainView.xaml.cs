using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
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

namespace MoYu.View
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        private int Mode=1;
        public MainView()
        {
            InitializeComponent();
            this.Version.Text =Assembly.GetExecutingAssembly().GetName().Version.ToString();

        }

        private void Close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Environment.Exit(0);    
        }
        private void GitHub1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OpenUrl("https://github.com/SundayRX/MoYu");
        }
        private void GitHub2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OpenUrl("https://github.com/DinoChan/Loaf");
        }
        private void GitHub3_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OpenUrl("https://github.com/JiaFeiMiao-K-Cat");
        }


        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Control? Imitator = null;
            Model.UpdateProgress updateProgress= new Model.UpdateProgress();

            int? windowsVersion = GetWindowsMajorVersion();

            if (Mode == 1)
            {
                if (windowsVersion == 7)
                    Imitator = new Imitators.Win7.Update();
                else if (windowsVersion == 10)
                    Imitator = new Imitators.Win10.Update();
                else
                    Imitator = new Imitators.Win11.Update();
            }
            else if (Mode == 2)
            {
                Imitator = new Imitators.Win10.Upgrade();
            }
            else if (Mode == 3)
            {
                 if (windowsVersion == 10)
                    Imitator = new Imitators.Win10.Crash();
                else
                    Imitator = new Imitators.Win11.Crash();
            }
            if(Imitator!=null)
                Imitator.DataContext= updateProgress;

            bool IsShowCursor = false;
            if (windowsVersion == 7)
                IsShowCursor = true;


            List<PresenterView> presenterViews= new List<PresenterView>();
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                PresenterView? presenterView = null;
                
                if (screen.Primary)
                     presenterView = new PresenterView(screen,Imitator,IsShowCursor);
                else
                    presenterView = new PresenterView(screen,null,IsShowCursor);
                presenterViews.Add(presenterView);

            }
            foreach(PresenterView presenterView in presenterViews)
            {
                presenterView.Show();
                presenterView.WindowState = WindowState.Maximized;
            }
            this.Close();
        }

        private static void OpenUrl(string? o)
        {

            try
            {
                if (o != null && o.ToString() != "")
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = o.ToString(),
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
            }
            catch (Exception) { }


        }

        private static int? GetWindowsMajorVersionByWMI()
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
        private static int? GetWindowsMajorVersionByEnvironment()
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

        private static int? GetWindowsMajorVersion()
        {
            int? Major = GetWindowsMajorVersionByWMI();
            if (Major != null)
                return Major;
            else
                return GetWindowsMajorVersionByEnvironment();

        }

        private void ModeSwitch_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (++Mode >= 4) Mode = 1;

            if (Mode == 1)
                ModeText.Text = "系统更新";
            else  if (Mode == 2)
                ModeText.Text = "系统升级";
            else if (Mode == 3)
                ModeText.Text = "系统蓝屏";

        }
    }
}
