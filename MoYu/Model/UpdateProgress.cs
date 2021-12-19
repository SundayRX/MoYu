using Microsoft.Win32;
using MoYu.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MoYu.Model
{
    public class UpdateProgress:NotifyBase
    {
        private Thread BackThread;
        private double Precent=0;
        private IntPtr hHook;


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
            IntPtr hModule = GetModuleHandle(IntPtr.Zero);
            hookProc = new LowLevelKeyboardProcDelegate(LowLevelKeyboardProc);
            hHook = SetWindowsHookEx(WH_KEYBOARD_LL, hookProc, hModule, 0);

            int? Temp = GetDWMColor();
            if(Temp!=null)
                DWMColor=new SolidColorBrush(Color.FromRgb((byte)(Temp & 0xFF), (byte)((Temp >> 8) & 0xFF), (byte)((Temp >> 16) & 0xFF)));

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
            if (hHook != IntPtr.Zero)
                UnhookWindowsHookEx(hHook);
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
            while (true)
            {
                double Temp = Precent + (100.0 - Precent) / 100.0;
                if (Temp > 99) Temp = 99;
                Interlocked.CompareExchange(ref Precent, Temp, Precent);
                Progress = (int)Precent;
                byte[] buffer = Guid.NewGuid().ToByteArray();
                int iRoot = BitConverter.ToInt32(buffer, 0);
                Random rdmNum = new Random(iRoot);
                int SleepTime = rdmNum.Next(100, 3000);
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
        private struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            int scanCode;
            public int flags;
            int time;
            int dwExtraInfo;
        }

        private delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hHook);


        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(IntPtr path);

        LowLevelKeyboardProcDelegate hookProc; // prevent gc

        const int WH_KEYBOARD_LL = 13;

        private static int LowLevelKeyboardProc(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == 256 && lParam.vkCode == 27)
                    Environment.Exit(0);
            }
            return 1;
        }




    }
}
