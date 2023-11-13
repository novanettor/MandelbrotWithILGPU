using System.Numerics;

namespace Mandelbrot
{
    public class CpuIterator : IIterator
    {
        public int Iterate(Complex c, int limit)
        {
            Complex z = new Complex(0, 0);

            for (var i = 0; i < limit; i++)
            {
                z = z * z + c;

                if (z.MagnitudeSquared() >= 4)
                    return i;
            }

            return limit;
        }

        public void IterateRange(Complex start, int width, int height, double step, int limit, int[,] output)
        {
            Parallel.For(0, height, b =>
            {
                Parallel.For(0, width, a =>
                {
                    var c = new Complex(start.Real + a * step, start.Imaginary + b * step);
                    output[a, b] = Iterate(c, limit);
                });
            });
        }
    }
}
