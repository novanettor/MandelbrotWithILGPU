using System.Numerics;

namespace Mandelbrot
{
    public static class ComplexExtensions
    {
        public static double MagnitudeSquared(this Complex z) => z.Real * z.Real + z.Imaginary * z.Imaginary;
    }
}
