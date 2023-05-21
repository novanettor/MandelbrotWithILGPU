namespace Mandelbrot.App.Palettes
{
    interface IPaletteGenerator
    {
        string DisplayName { get; }
        int[] GeneratePalette(int size);
    }
}
