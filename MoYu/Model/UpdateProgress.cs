using Microsoft.Win32;
using MoYu.Common;
using MoYu.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using static MoYu.Tools.Kernel32;
using static MoYu.Tools.User32;
namespace MoYu.Model
{
    public class UpdateProgress:NotifyBase
    {
        private static IntPtr hHook=IntPtr.Zero;
        public static SDC DISPLAYCONFIG_SDC = SDC.SDC_APPLY;

        private Thread BackThread;
        private double Precent=0;



        private int progress;
        public int Progress
        {
            get { return progress; }
            set
            { 
                if(progress!=value)
                {
                    progress = value;
                    this.DoNotify();
                }

            }
        }


        private SolidColorBrush dWMColor=new SolidColorBrush(Color.FromRgb(0, 0, 0));
        public SolidColorBrush DWMColor
        {
            get { return dWMColor; }
            set { dWMColor = value;this.DoNotify(); }
        }

        public CommandBase MouseWheelCommand { get; set; }
        public CommandBase KeyDownCommand { get; set; }
        public UpdateProgress()
        {
            Kernel32.SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);

            IntPtr hModule = GetModuleHandle(IntPtr.Zero);
            hookProc = new LowLevelKeyboardProcDelegate(LowLevelKeyboardProc);
            hHook = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, hookProc, hModule, 0);

            int? Temp = GetDWMColor();
            if (Temp != null)
                DWMColor =new SolidColorBrush(Color.FromRgb((byte)(Temp & 0xFF), (byte)((Temp >> 8) & 0xFF), (byte)((Temp >> 16) & 0xFF)));

            this.MouseWheelCommand = new CommandBase();
            this.MouseWheelCommand.DoExecute = new Action<object>(DoMouseWheel);
            this.MouseWheelCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

            this.KeyDownCommand = new CommandBase();
            this.KeyDownCommand.DoExecute = new Action<object>(DoKeyDown);
            this.KeyDownCommand.DoCanExecute = new Func<object, bool>((o) => { return true; });

            BackThread = new Thread(BackFunction);
            BackThread.IsBackground = true;
            BackThread.Start();

        }

        ~UpdateProgress()
        {

        }
        private void DoMouseWheel(object o)
        {
            if(o !=null)
            {
                System.Windows.Input.MouseWheelEventArgs e = o as System.Windows.Input.MouseWheelEventArgs;
                double Temp = Precent - e.Delta * 0.01;
                if (Temp < 0) Temp = 0;
                if (Temp > 99) Temp = 99;
                Interlocked.CompareExchange(ref Precent, Temp, Precent);
                Progress = (int)Precent;
            }
        }
        private void DoKeyDown(object o)
        {
            if (o != null)
            {
                System.Windows.Input.KeyEventArgs e = o as System.Windows.Input.KeyEventArgs;
                if (e.Key == System.Windows.Input.Key.Escape)
                    Environment.Exit(0);
            }

        }
        private void BackFunction()
        {
            Thread.Sleep(3000);
            while (true)
            {
                double Temp = Precent + GetRandomValue(1, 5);
                //double Temp = Precent + (100.0 - Precent) / 100.0;
                if (Temp > 99) Temp = 99;
                Interlocked.CompareExchange(ref Precent, Temp, Precent);
                Progress = (int)Precent;
                int SleepTime = GetRandomValue(1000, 5000);
                Thread.Sleep(SleepTime);
            }
        }

        private static int? GetDWMColor()
        {
            int? Color = null;
            try
            {
                RegistryKey reg = Registry.CurrentUser;
                RegistryKey software = reg.OpenSubKey(@"SOFTWARE");
                RegistryKey run = reg.OpenSubKey(@"SOFTWARE\Microsoft\Windows\DWM");
                object key = run.GetValue("AccentColor");
                software.Close();
                run.Close();
                if (key != null && key is int)
                    Color = (int)key;
            }
            catch (Exception) { }

            return Color;
        }

        private static int GetRandomValue(int MinValue, int MaxValue)
        {
            if (MinValue == MaxValue) return MinValue;
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int iRoot = BitConverter.ToInt32(buffer, 0);
            Random rdmNum = new Random(iRoot);
            return rdmNum.Next(MinValue, MaxValue);
        }


        LowLevelKeyboardProcDelegate hookProc;



        private static int LowLevelKeyboardProc(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == 256 && lParam.vkCode == 27)
                {
                    if (DISPLAYCONFIG_SDC == SDC.SDC_TOPOLOGY_CLONE)
                        SetDisplayConfig(0, IntPtr.Zero, 0, IntPtr.Zero, SDC.SDC_APPLY | DISPLAYCONFIG_SDC);
                    if (hHook != IntPtr.Zero)
                        UnhookWindowsHookEx(hHook);

                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }
                    
            }
            return 1;
        }




    }
}
