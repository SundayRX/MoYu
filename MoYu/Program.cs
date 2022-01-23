using MoYu.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
namespace MoYu
{
    public static class Program
    {
        public static string ProgramName = "MoYu";
        public static string LocalLanguage = "zh-hans";
        public static int? WindowsVersion = 10;
        public static System.Threading.Mutex ProcessLock=new Mutex();


        [STAThread]
        static void Main(string[] args)
        {
            Boolean CreatedNew = false;

            bool Temp = System.Threading.Mutex.TryOpenExisting(ProgramName, out ProcessLock);
            if (!Temp)
                ProcessLock = new System.Threading.Mutex(true, ProgramName, out CreatedNew);


            if (CreatedNew)
            {
                TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;
                Application app = new Application();
                LocalLanguage=GetLocalLanguage();
                WindowsVersion=GetWindowsMajorVersion();
                //WindowsVersion=7;
                ResourceDictionary LanguageDictionary = (ResourceDictionary)Application.LoadComponent(new System.Uri($"../Languages/{LocalLanguage}.xaml", System.UriKind.Relative));
                if (LanguageDictionary != null)
                {
                    app.Resources.MergedDictionaries.Add(LanguageDictionary);
                }
                app.StartupUri = new System.Uri("pack://application:,,,/MoYu;component/View/MainView.xaml", System.UriKind.Absolute);
                app.Run();
            }

        }
        static void TaskSchedulerUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
        }

        private static string GetLocalLanguage()
        {
            string TempLanguage=Thread.CurrentThread.CurrentCulture.Name.ToLower();
            string[] Temp = TempLanguage.Split('-');
            if (Temp.Length >= 1)
            {
                if (Temp[0] == "zh")
                {
                    if (Temp.Length >= 2)
                    {
                        if (Temp[1] == "hans" || Temp[1] == "cn" || Temp[1] == "sg" || Temp[1] == "my") TempLanguage = "zh-hans";
                        else if (Temp[1] == "hant" || Temp[1] == "tw" || Temp[1] == "hk" || Temp[1] == "mo") TempLanguage = "zh-hant";
                        else TempLanguage = "zh-hans";
                    }
                    else TempLanguage = "zh-hans";
                }
                else TempLanguage = "en";
            }
            else
                TempLanguage = "zh-hans";

            return TempLanguage;
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

    }

}
