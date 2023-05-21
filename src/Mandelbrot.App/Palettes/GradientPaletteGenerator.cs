using System.Collections.Generic;
using System.Drawing;

namespace Mandelbrot.App.Palettes
{
    class GradientPaletteGenerator : IPaletteGenerator
    {
        private readonly IList<GradientStop> _stops;

        public GradientPaletteGenerator() : this(null) { }

        public GradientPaletteGenerator(IList<GradientStop>? stops)
        {
            _stops = stops ?? GetDefaultStops();
        }

        public int[] GeneratePalette(int size)
        {
            var palette = new int[size];
            for (int i = 0; i < size; i++)
            {
                palette[i] = GetColor(i / (float)size);
            }
            return palette;
        }

        private int GetColor(float p)
        {
            if (p > 1) p = 1;
            for (int i = 0; i < _stops.Count - 1; i++)
            {
                if (_stops[i].offset <= p && _stops[i + 1].offset >= p)
                {
                    var stop1 = _stops[i];
                    var stop2 = _stops[i + 1];

                    var pos = (p - stop1.offset) / (stop2.offset - stop1.offset);
                    var rpos = 1 - pos;

                    return
                        255 << 24 |
                        (byte)(rpos * stop1.r + pos * stop2.r) << 16 |
                        (byte)(rpos * stop1.g + pos * stop2.g) << 8 |
                        (byte)(rpos * stop1.b + pos * stop2.b);
                }
            }

            return 0;
        }

        public IList<GradientStop> GetDefaultStops() => new List<GradientStop>
        {
            new GradientStop(0.000f, 0, 0, 0),
            new GradientStop(0.160f, 32, 107, 202),
            new GradientStop(0.420f, 237, 255, 255),
            new GradientStop(0.642f, 255, 169, 0),
            new GradientStop(1.000f, 0, 0, 0)
        };


        public struct GradientStop
        {
            public float offset;
            public byte r;
            public byte g;
            public byte b;

            public GradientStop(float offset, byte r, byte g, byte b)
            {
                this.offset = offset;
                this.r = r;
                this.g = g;
                this.b = b;
            }

            public GradientStop(float offset, Color color)
            {
                this.offset = offset;
                r = color.R;
                g = color.G;
                b = color.B;
            }
        }
    }
}
