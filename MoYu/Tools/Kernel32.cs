using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MoYu.Tools
{
    public class Kernel32
    {
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x40u,
            ES_CONTINUOUS = 0x80000000u,
            ES_DISPLAY_REQUIRED = 2u,
            ES_SYSTEM_REQUIRED = 1u,
            ES_USER_PRESENT = 4u
        }

        [DllImport("kernel32.dll")]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE flags);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(IntPtr path);
    }
}
