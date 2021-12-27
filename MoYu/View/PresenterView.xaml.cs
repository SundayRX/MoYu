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

namespace MoYu.View
{
    /// <summary>
    /// Windows10.xaml 的交互逻辑
    /// </summary>
    public partial class PresenterView : Window
    {
        public PresenterView(System.Windows.Forms.Screen screen,Control? Imitator,bool IsShowCursor)
        {
            InitializeComponent();

            Left = screen.Bounds.Left;
            Top = screen.Bounds.Top;

            Topmost = true;


            if (Imitator!=null)
                this.BACK.Child = Imitator;

            if (!IsShowCursor)
                ShowCursor(0);

        }


        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        public extern static void ShowCursor(int status);

    }
}