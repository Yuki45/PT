using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoInspection
{
    public class CRect
    {
        public int left { get; set; }
        public int top { get; set; }

        public int right { get; set; }
        public int bottom { get; set; }

        public CRect(int _left, int _right, int _top, int _bottom)
        {
            left = _left;
            right = _right;
            top = _top;
            bottom = _bottom;
        }
    }
}
