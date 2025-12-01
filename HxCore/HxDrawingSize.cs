using System;
using System.Collections.Generic;
using System.Text;

namespace HxCore
{
    public struct HxDrawingSize
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public HxDrawingSize(bool bInit = false)
        {
            Width = 0;
            Height = 0;
            if (bInit == true)
            {
                Width = int.MinValue;
                Height = int.MinValue;
            }
        }
        public HxDrawingSize(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
