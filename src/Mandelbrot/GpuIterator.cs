using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;
using System.Numerics;

namespace Mandelbrot
{
    public class GpuIterator : IIterator, IDisposable
    {
        private readonly Context _context;
        private readonly Accelerator _accelerator;
        private readonly Action<Index2, Complex, double, int, ArrayView2D<int>> _iterateKernel;
        private bool _disposed;

        public GpuIterator()
        {
            _context = new Context();
            _accelerator = new CudaAccelerator(_context);

            _iterateKernel = _accelerator.LoadAutoGroupedStreamKernel<Index2, Complex, double, int, ArrayView2D<int>>(IterateKernel);
        }

        public void IterateRange(Complex start, int width, int height, double step, int limit, int[,] output)
        {
            var iterationsBuffer = _accelerator.Allocate<int>(width, height);

            // Launch kernel
            _iterateKernel(new Index2(width, height), start, step, limit, iterationsBuffer.View);

            // Wait for it to finish
            _accelerator.Synchronize();

            // Get the results
            iterationsBuffer.CopyTo(output, new LongIndex2(0, 0), new LongIndex2(0, 0), new LongIndex2(width, height));

            iterationsBuffer.Dispose();
        }

        private static void IterateKernel(
            Index2 index,
            Complex start,
            double step,
            int limit,
            ArrayView2D<int> output)
        {
            var c = new Complex(start.Real + index.X * step, start.Imaginary + index.Y * step);
            var z = new Complex(0, 0);

            var iterations = 0;
            for (; iterations < limit; iterations++)
            {
                z = z * z + c;

                if (z.MagnitudeSquared() >= 4)
                    break;
            }

            output[index.X, index.Y] = iterations;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _accelerator?.Dispose();
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
