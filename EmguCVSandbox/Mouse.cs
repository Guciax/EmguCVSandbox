using System;
using System.Collections.Generic;
using System.Linq;
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

        private enum MouseButton
        {
            Left,
            Right,
        }

        public void MouseLeftClick(int x, int y)
        {
            Click(MouseButton.Left, x, y);
        }

        public void MouseDragLeft(int x, int y, int xd, int yd)
        {
            Drag(MouseButton.Left, x, y, xd,yd);
        }

        public void MouseRightClick(int x, int y)
        {
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

    }
}
