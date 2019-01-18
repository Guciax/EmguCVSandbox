using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;


namespace EmguCVSandbox
{
    class ScreenShot
    {
        public static Bitmap RectangleScreenshot(Rectangle rect)
        {
            //int width = rect.right - rect.left;
            //int height = rect.bottom - rect.top;

            int width = rect.Width;
            int height = rect.Height;

            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(rect.Location.X, rect.Location.Y, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);

            return bmp;
        }

        public static Bitmap GetScreenShop(Rectangle gameWindowRectangle)
        {
            var bmp = new Bitmap(gameWindowRectangle.Width, gameWindowRectangle.Height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(gameWindowRectangle.X, gameWindowRectangle.Y, 0, 0, new Size(gameWindowRectangle.Width, gameWindowRectangle.Height), CopyPixelOperation.SourceCopy);
            return bmp;
        }

        //Używana do zapisywania screenu o podanej nazwie pliku
        public static Bitmap ScreenShopSaver(string filename, Rectangle gameWindowRectangle)
        {
            var bmp = new Bitmap(gameWindowRectangle.Width, gameWindowRectangle.Height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.CopyFromScreen(gameWindowRectangle.X, gameWindowRectangle.Y, 0, 0, new Size(gameWindowRectangle.Width, gameWindowRectangle.Height), CopyPixelOperation.SourceCopy);

            bmp.Save(@"Images\"+filename+".png");
            return bmp;
        }

        
    }
}
