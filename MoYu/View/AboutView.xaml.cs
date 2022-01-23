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

namespace MoYu.View
{
    /// <summary>
    /// AboutView.xaml 的交互逻辑
    /// </summary>
    public partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();

            if (Program.WindowsVersion == 7)
            {
                if (Program.LocalLanguage == "zh-hans")
                {
                    UpgradeCorrect.Visibility = Visibility.Hidden;
                    CrashCorrect.Visibility = Visibility.Hidden;
                    UpgradeError.Visibility = Visibility.Visible;
                    CrashError.Visibility = Visibility.Visible;

                }
                else if (Program.LocalLanguage == "zh-hant")
                {
                    UpgradeCorrect.Visibility = Visibility.Hidden;
                    CrashCorrect.Visibility = Visibility.Hidden;
                    UpgradeError.Visibility = Visibility.Visible;
                    CrashError.Visibility = Visibility.Visible;

                }
                else if (Program.LocalLanguage == "en")
                {
                    UpdateCorrect.Visibility = Visibility.Hidden;
                    UpgradeCorrect.Visibility = Visibility.Hidden;
                    CrashCorrect.Visibility = Visibility.Hidden;
                    UpdateError.Visibility = Visibility.Visible;
                    UpgradeError.Visibility = Visibility.Visible;
                    CrashError.Visibility = Visibility.Visible;

                }
            }
            else if (Program.WindowsVersion == 11)
            {
                if (Program.LocalLanguage == "zh-hans")
                {
                    UpgradeCorrect.Visibility = Visibility.Hidden;
                    UpgradeError.Visibility = Visibility.Visible;
                }
                else if (Program.LocalLanguage == "en")
                {
                    UpgradeCorrect.Visibility = Visibility.Hidden;
                    UpgradeError.Visibility = Visibility.Visible;

                }
            }

        }
        private void Close_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
