using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MoYu.Tools
{
    public class User32
    {

        public enum HookType
        {
            
            WH_MSGFILTER = -1,

            WH_JOURNALRECORD,

            WH_JOURNALPLAYBACK,

            WH_KEYBOARD,

            WH_GETMESSAGE,

            WH_CALLWNDPROC,

            WH_CBT,

            WH_SYSMSGFILTER,

            WH_MOUSE,
            WH_HARDWARE,

            WH_DEBUG,

            WH_SHELL,

            WH_FOREGROUNDIDLE,

            WH_CALLWNDPROCRET,

            WH_KEYBOARD_LL,

            WH_MOUSE_LL
        }

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            int scanCode;
            public int flags;
            int time;
            int dwExtraInfo;
        }
         [Flags]
        public enum QDC
        {
            QDC_ALL_PATHS = 0x1,
            QDC_ONLY_ACTIVE_PATHS = 0x2,
            QDC_DATABASE_CURRENT = 0x4,
            QDC_VIRTUAL_MODE_AWARE = 0x10,
            QDC_INCLUDE_HMD = 0x20
        }

        [Flags]
        public enum SDC
        {
            SDC_TOPOLOGY_INTERNAL = 0x1,
            SDC_TOPOLOGY_CLONE = 0x2,
            SDC_TOPOLOGY_EXTEND = 0x4,
            SDC_TOPOLOGY_EXTERNAL = 0x8,
            SDC_TOPOLOGY_SUPPLIED = 0x10,
            SDC_USE_DATABASE_CURRENT = 0xF,
            SDC_USE_SUPPLIED_DISPLAY_CONFIG = 0x20,
            SDC_VALIDATE = 0x40,
            SDC_APPLY = 0x80,
            SDC_NO_OPTIMIZATION = 0x100,
            SDC_SAVE_TO_DATABASE = 0x200,
            SDC_ALLOW_CHANGES = 0x400,
            SDC_PATH_PERSIST_IF_REQUIRED = 0x800,
            SDC_FORCE_MODE_ENUMERATION = 0x1000,
            SDC_ALLOW_PATH_ORDER_CHANGES = 0x2000,
            SDC_VIRTUAL_MODE_AWARE = 0x8000
        }
        [Flags]
        public enum DISPLAYCONFIG_TOPOLOGY_ID : uint
        {
            DISPLAYCONFIG_TOPOLOGY_INTERNAL = 0x1u,
            DISPLAYCONFIG_TOPOLOGY_CLONE = 0x2u,
            DISPLAYCONFIG_TOPOLOGY_EXTEND = 0x4u,
            DISPLAYCONFIG_TOPOLOGY_EXTERNAL = 0x8u
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto,Size =72)]
        public struct DISPLAYCONFIG_PATH_INFO
        {
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4,Size =64)]
        public struct DISPLAYCONFIG_MODE_INFO
        {
        }

        [DllImport("user32.dll")]
        public static extern uint GetDisplayConfigBufferSizes(QDC Flags,out uint NumPathArrayElements,out uint NumModeInfoArrayElements);
        [DllImport("user32.dll")]
        public static extern uint QueryDisplayConfig(QDC flags, ref uint numPathArrayElements, [In][Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DISPLAYCONFIG_PATH_INFO[] pathArray, ref uint numModeInfoArrayElements, [In][Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] DISPLAYCONFIG_MODE_INFO[] modeInfoArray, IntPtr currentTopologyId = default(IntPtr));
       [DllImport("user32.dll")]
         public static extern uint QueryDisplayConfig(QDC flags, ref uint numPathArrayElements, [In][Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] DISPLAYCONFIG_PATH_INFO[] pathArray, ref uint numModeInfoArrayElements, [In][Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] DISPLAYCONFIG_MODE_INFO[] modeInfoArray, out DISPLAYCONFIG_TOPOLOGY_ID currentTopologyId);
        
        public static uint QueryDisplayConfig(QDC flags, out DISPLAYCONFIG_PATH_INFO[] pathArray, out DISPLAYCONFIG_MODE_INFO[] modeInfoArray, out DISPLAYCONFIG_TOPOLOGY_ID currentTopologyId)
        {
            uint displayConfigBufferSizes;
            do
            {
                displayConfigBufferSizes = GetDisplayConfigBufferSizes(flags, out uint numPathArrayElements, out uint numModeInfoArrayElements);
                pathArray = new DISPLAYCONFIG_PATH_INFO[(displayConfigBufferSizes==0) ? numPathArrayElements : 0];
                modeInfoArray = new DISPLAYCONFIG_MODE_INFO[(displayConfigBufferSizes==0) ? numModeInfoArrayElements : 0];
                currentTopologyId =0u;
                if (displayConfigBufferSizes!=0)
                {
                    return displayConfigBufferSizes;
                }

                displayConfigBufferSizes = (flags!=QDC.QDC_DATABASE_CURRENT) ? QueryDisplayConfig(flags, ref numPathArrayElements, pathArray, ref numModeInfoArrayElements, modeInfoArray, (IntPtr)0) : QueryDisplayConfig(flags, ref numPathArrayElements, pathArray, ref numModeInfoArrayElements, modeInfoArray, out currentTopologyId);
                if (displayConfigBufferSizes==0 || displayConfigBufferSizes != 122u)
                {
                    return displayConfigBufferSizes;
                }
            }
            while (displayConfigBufferSizes == 122u);
            return displayConfigBufferSizes;
        }



        public delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(HookType idHook, LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll")]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("user32.dll")]
        public static extern long SetDisplayConfig(uint numPathArrayElements, IntPtr pathArray, uint numModeArrayElements,IntPtr modeArray, SDC flags);
    }
}
