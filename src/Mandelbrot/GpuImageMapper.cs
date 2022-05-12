using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;

namespace Mandelbrot
{
    public class GpuImageMapper : IImageMapper, IDisposable
    {
        private static Context _context;
        private static Accelerator _accelerator;
        private static Action<Index2, ArrayView2D<int>, ArrayView<int>, ArrayView<byte>> _mapKernel;
        private bool _disposed;

        public GpuImageMapper()
        {
            _context = new Context();
            _accelerator = new CudaAccelerator(_context);

            _mapKernel = _accelerator.LoadAutoGroupedStreamKernel<Index2, ArrayView2D<int>, ArrayView<int>, ArrayView<byte>>(MapKernel);
        }

        public void Map(int[,] iterationsMap, int[] palette, byte[] output)
        {
            var height = iterationsMap.GetLength(1);
            var width = iterationsMap.GetLength(0);

            var iterationsBuffer = _accelerator.Allocate(iterationsMap);
            var paletteBuffer = _accelerator.Allocate(palette);
            var pixelBuffer = _accelerator.Allocate<byte>(width * height * 3);

            // Launch kernel
            _mapKernel(new Index2(width, height), iterationsBuffer.View, paletteBuffer.View, pixelBuffer.View);

            // Wait for it to finish
            _accelerator.Synchronize();

            // Get the results
            pixelBuffer.CopyTo(output, 0, 0, pixelBuffer.Length);

            pixelBuffer.Dispose();
        }

        private static void MapKernel(
            Index2 index,
            ArrayView2D<int> iterationsMap,
            ArrayView<int> palette,
            ArrayView<byte> output)
        {
            var width = iterationsMap.Width;
            var height = iterationsMap.Height;

            var iterations = iterationsMap[index.X, index.Y];
            var color = palette[iterations];

            // Screen space is inverted along y, so b -> (height - 1 - b)
            var pixelIndex = ((height - 1 - index.Y) * width + index.X) * 3;
            unsafe
            {
                var colorBytes = (byte*)&color;
                output[pixelIndex] = colorBytes[0];
                output[pixelIndex + 1] = colorBytes[1];
                output[pixelIndex + 2] = colorBytes[2];
            }
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
