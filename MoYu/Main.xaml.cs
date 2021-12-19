using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace MoYu
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Main : Window
    {
        public Main()
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
            catch(Exception) { }


        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Model.UpdateProgress updateProgress= new Model.UpdateProgress();

            foreach (System.Windows.Forms.Screen scr in System.Windows.Forms.Screen.AllScreens)
            {

                WindowsUpdate windowsUpdate = new WindowsUpdate(scr,updateProgress);
                windowsUpdate.Show();



            }

            this.Close();
        }
       
    }
}
