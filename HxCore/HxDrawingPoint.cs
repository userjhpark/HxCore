using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    public struct HxDrawingPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public HxDrawingPoint(bool bInit = false)
        {
            X = 0;
            Y = 0;
            if (bInit == true)
            {
                X = int.MinValue;
                Y = int.MinValue;
            }
        }
        public HxDrawingPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
