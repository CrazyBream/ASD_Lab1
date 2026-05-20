using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_Lab1.BLL.Components
{
    public class TouchScreen
    {
        public double SizeInches { get; set; }
        public bool IsMultiTouchSupported { get; set; }
        public TouchScreen(double size, bool multiTouch) { SizeInches = size; IsMultiTouchSupported = multiTouch; }
    }
}
