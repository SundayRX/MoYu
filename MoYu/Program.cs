using MoYu.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace MoYu
{
    public static class Program
    {
        public static string ProgramName = "MoYu";
        public static System.Threading.Mutex ProcessLock = null;
        [STAThread]
        private static void Main(string[] args)
        {
            Boolean CreatedNew = false;

            bool Temp = System.Threading.Mutex.TryOpenExisting(ProgramName, out ProcessLock);
            if (!Temp)
                ProcessLock = new System.Threading.Mutex(true, ProgramName, out CreatedNew);
            if (CreatedNew)
            {
                TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;
                Application app = new Application();
                app.StartupUri = new System.Uri("pack://application:,,,/MoYu;component/View/MainView.xaml", System.UriKind.Absolute);
                app.Run();
            }

        }
        private static void TaskSchedulerUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
        }



    }

}
