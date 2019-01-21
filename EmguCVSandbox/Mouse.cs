using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmguCVSandbox
{
    class Mouse
    {
        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        //ustawienie czy do okna ma byc relative czy nie
        private const bool relativetowindow = true;

                
        Point winLoc = new Point(Windows.GameWindowRectangle().X, Windows.GameWindowRectangle().Y);

        

    private enum MouseButton
        {
            Left,
            Right,
        }

        public void MouseLeftClick(Point pt)
        {
            if (relativetowindow == true)
                {
                pt.X += winLoc.X;
                pt.Y += winLoc.Y;
            }


            Click(MouseButton.Left, pt.X, pt.Y);
        }

        public void MouseLeftClickRelative(int x, int y,int rx, int ry)
        {
            if (relativetowindow == true)
            {
                x = x + winLoc.X;
                y = y + winLoc.Y;
            }
            x = x + rx;
            y = y + ry;

            Click(MouseButton.Left, x, y);
        }

        public void MouseDragLeft(Point from, Point to)
        {
            if (relativetowindow == true)
            {
                from.X += winLoc.X;
                from.Y += winLoc.Y;
                to.X += winLoc.X;
                to.Y += winLoc.Y;

            }
            Drag(MouseButton.Left, from.X, from.Y, to.X, to.Y);
        }

        public void MouseDragLeftRelative(int x, int y, int xd, int yd,int rx,int ry)
        {
            if (relativetowindow == true)
            {
                x = x + winLoc.X;
                y = y + winLoc.Y;
                xd = xd + winLoc.X;
                yd = yd + winLoc.Y;

            }
            x = x+rx;
            y = y + ry;
            Drag(MouseButton.Left, x, y, xd, yd);
        }

        public void MouseRightClick(int x, int y)
        {
            if (relativetowindow == true)
            {
                x = x + winLoc.X;
                y = y + winLoc.Y;
            }
            Click(MouseButton.Right, x, y);
        }

        private int MbToVkDown(MouseButton mb)
        {
            switch (mb)
            {
                case MouseButton.Left:
                    return MOUSEEVENTF_LEFTDOWN;
                case MouseButton.Right:
                    return MOUSEEVENTF_RIGHTDOWN;
            }

            throw new Exception("wot.");
        }

        private int MbToVkUp(MouseButton mb)
        {
            switch (mb)
            {
                case MouseButton.Left:
                    return MOUSEEVENTF_LEFTUP;
                case MouseButton.Right:
                    return MOUSEEVENTF_RIGHTUP;
            }

            throw new Exception("wot.");
        }

        private int rand(int v)
        {
            var r = new Random();

            return v += r.Next(-3, 4);
        }

        private void Click(MouseButton mb, int x, int y)
        {
            if (relativetowindow == true)
            {
                x = x + winLoc.X;
                y = y + winLoc.Y;
            }
            x = rand(x);
            y = rand(y);

            SetCursorPos(x, y);
            Thread.Sleep(50);
            mouse_event(MbToVkDown(mb), 0, 0 , 0, 0);
            Thread.Sleep(50);
            mouse_event(MbToVkUp(mb), 0, 0, 0, 0);
            Thread.Sleep(50);
        }
        private void Drag(MouseButton mb, int x, int y, int xd, int yd)
        {
            if (relativetowindow == true)
            {
                x = x + winLoc.X;
                y = y + winLoc.Y;
                xd = xd + winLoc.X;
                yd = yd + winLoc.Y;

            }


            x = rand(x);
            y = rand(y);
            xd = rand(xd);
            yd = rand(yd);

            SetCursorPos(x, y);
            Thread.Sleep(50);
            mouse_event(MbToVkDown(mb), 0, 0, 0, 0);
            Thread.Sleep(50);
            SetCursorPos(xd, yd);
            Thread.Sleep(50);
            mouse_event(MbToVkUp(mb), 0, 0, 0, 0);
            Thread.Sleep(50);
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
