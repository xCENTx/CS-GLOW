using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace helpers.CS
{
    class func
    {
        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, int dwExtraInfo);
        public const uint MOUSEEVENTF_MOVE = 0x0001;
        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Int32 vKey);
        public static int VK_LBUTTON = 0x01;
        public static int VK_RBUTTON = 0x02;
        public static int VK_MENU = 0x12;
        public static int VK_END = 0x23;
        public static int VK_HOME = 0x24;
        public static int VK_INSERT = 0x2D;
        public static int VK_NUMPAD1 = 0x61;
        public static int VK_NUMPAD2 = 0x62;
        public static int VK_NUMPAD3 = 0x63;
        public static int VK_NUMPAD4 = 0x64;
        public static int VK_NUMPAD9 = 0x69;
        public static int VK_LSHIFT = 0xA0;

        /// <summary>
        /// Console UI
        /// </summary>
        /// <param name="PRESET1">String 1</param>
        /// <param name="PRESET2">String 2</param>
        /// <param name="PRESET3">String 3</param>
        /// <param name="RAPIDFIRE">String 4</param>
        /// <param name="FLAG">String 5</param>
        public static void _menu(string PRESET1)
        {
            Console.Clear();
            Console.WriteLine(" _______________________ \n" +
            "|------- CS:GLOW -------|\n" +
            $"| [1] GLOW:      => [{PRESET1}] |\n" +
            "| [END] QUIT            |\n" +
            "|v1.0-------------------|");
        }

    }
}