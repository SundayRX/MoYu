using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static MoYu.Tools.User32;

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
        private void GitHub4_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OpenUrl("https://github.com/chiyi4488");
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            

            DISPLAYCONFIG_TOPOLOGY_ID topId=DISPLAYCONFIG_TOPOLOGY_ID.DISPLAYCONFIG_TOPOLOGY_INTERNAL;
            try
            {
                QueryDisplayConfig(QDC.QDC_DATABASE_CURRENT, out var paths, out var modes, out topId);
            }
            catch { }
            
            if (topId == DISPLAYCONFIG_TOPOLOGY_ID.DISPLAYCONFIG_TOPOLOGY_CLONE && Mode == 3)
            {
                 Model.UpdateProgress.DISPLAYCONFIG_SDC = SDC.SDC_TOPOLOGY_CLONE;
                 SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, SDC.SDC_APPLY | SDC.SDC_TOPOLOGY_EXTEND);
            }
                

            Control? Imitator = null;
            Model.UpdateProgress updateProgress= new Model.UpdateProgress();

            int? windowsVersion = Program.WindowsVersion;


            string ImitatorXAMLRoot = "../Imitators";
            string PlatformXAMLPath = "";
            string ModeXAMLPath = "";
            string LanguageXAMLPath = "";

            if (windowsVersion == 7) PlatformXAMLPath = "Win7";
            else if (windowsVersion == 10) PlatformXAMLPath = "Win10";
            else if (windowsVersion == 11) PlatformXAMLPath = "Win11";
            else PlatformXAMLPath = "Win10";


            if (Mode == 1) ModeXAMLPath = "Update";
            else if(Mode == 2) ModeXAMLPath = "Upgrade";
            else if(Mode == 3) ModeXAMLPath = "Crash";
            else ModeXAMLPath = "Update";

             LanguageXAMLPath = Program.LocalLanguage + ".xaml";


            string ImitatorXAMLFullPath = System.IO.Path.Combine(ImitatorXAMLRoot,PlatformXAMLPath,ModeXAMLPath,LanguageXAMLPath);

            try
            {
                Imitator = (UserControl)Application.LoadComponent(new System.Uri(ImitatorXAMLFullPath, System.UriKind.Relative));
            }
            catch (Exception)
            {
                Imitator = null;
            }

            if (Imitator == null)
            {
                Imitator = (UserControl)Application.LoadComponent(new System.Uri(System.IO.Path.Combine(ImitatorXAMLRoot,"Win10",ModeXAMLPath,LanguageXAMLPath), System.UriKind.Relative));
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
                
                if (screen.Primary )
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



        private void ModeSwitch_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (++Mode >= 4) Mode = 1;
            if (Mode == 1)
                ModeText.Text = (string) FindResource("Str_ModeUpdate");
            else  if (Mode == 2)
                ModeText.Text =  (string)FindResource("Str_ModeUpgrade");
            else if (Mode == 3)
                ModeText.Text =  (string)FindResource("Str_ModeCrash");

        }

        private void Help_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AboutView aboutView = new AboutView();
            aboutView.Owner = this;
            aboutView.ShowDialog();
        }
    }
}
