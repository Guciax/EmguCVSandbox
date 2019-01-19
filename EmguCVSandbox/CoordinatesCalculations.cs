using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmguCVSandbox
{
    class CoordinatesCalculations
    { 
        public static Point GoBackToWindowsOrigins(Point point, Point originOfPoint)
        {
            return new Point(originOfPoint.X + point.X, originOfPoint.Y + point.Y);
        }
    }
}
