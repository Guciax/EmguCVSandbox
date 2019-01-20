using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EmguCVSandbox
{
    public class Windows
    {
        
        public static Tuple<int, int> GetTupleWindowxy()
        {


            var proc = Process.GetProcessesByName("Lord of the Rings - LCG")[0];
            var rect = new User32.Rect();
            User32.GetWindowRect(proc.MainWindowHandle, ref rect);
            int xw = rect.left-30; // nie wiem dlaczego :D
            int yw = rect.top;

            return new Tuple<int, int>(xw, yw);

        }
        public static Rectangle GameWindowRectangle()
        {
            var proc = Process.GetProcessesByName("Lord of the Rings - LCG")[0];
            var rect = new User32.Rect();
            User32.GetWindowRect(proc.MainWindowHandle, ref rect);

            int x = rect.left;
            int y = rect.top;
            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            return new Rectangle(x, y, width, height);
        }

        private class User32
        {

            [StructLayout(LayoutKind.Sequential)]
            public struct Rect
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
        }
    }
}
