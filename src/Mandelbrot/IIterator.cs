using System.Numerics;

namespace Mandelbrot
{
    public interface IIterator
    {
        /// <summary>
        /// Creates an iterations map by performing the Mandelbrot iteration Z -> Z^2 + C for all
        /// complex values in the given range in the complex plane.
        /// </summary>
        /// <param name="start">The complex number at which to start (the lower left corner in the complex plane)</param>
        /// <param name="width">The width (in number of samples) of the map</param>
        /// <param name="height">The height (in number of samples) of the map</param>
        /// <param name="step">The interval between samples in the complex plane</param>
        /// <param name="limit">The maximum of number of iterations to perform</param>
        /// <param name="output">The resulting iterations map</param>
        void IterateRange(Complex start, int width, int height, double step, int limit, int[,] output);
    }
}