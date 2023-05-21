using System;
using System.Drawing;

namespace Mandelbrot.App.Palettes
{
    class BinaryPaletteGenerator : IPaletteGenerator
    {
        public string DisplayName => "Binary";

        public int[] GeneratePalette(int size)
        {
            var palette = new int[size];
            Array.Fill(palette, Color.White.ToArgb());
            palette[size - 1] = Color.Black.ToArgb();
            return palette;
        }
    }
}
