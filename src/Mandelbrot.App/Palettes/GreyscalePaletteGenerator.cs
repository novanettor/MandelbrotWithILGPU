using System.Drawing;

namespace Mandelbrot.App.Palettes
{
    class GreyscalePaletteGenerator : IPaletteGenerator
    {
        public string DisplayName => "Greyscale";

        public int[] GeneratePalette(int size)
        {
            var palette = new int[size];
            for (int i = 0; i < size; i++)
            {
                var component = (int)((float)(size - i - 1) / size * 255);
                palette[i] = Color.FromArgb(component, component, component).ToArgb();
            }
            return palette;
        }
    }
}
