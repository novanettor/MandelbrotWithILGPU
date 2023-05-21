using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot.App.Palettes
{
    interface IPaletteGenerator
    {
        int[] GeneratePalette(int size);
    }
}
