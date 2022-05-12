using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Mandelbrot.App
{
    class MandelbrotRenderer : IDisposable
    {
        private CpuIterator _cpuIterator;
        private CpuImageMapper _cpuImageMapper;
        private WriteableBitmap? _bitmap;
        private int[,]? _iterations;
        private byte[]? _pixels;
        private bool _disposed;

        private bool _useGpu;
        public bool UseGpu { get => _useGpu; set => _useGpu = value; }

        public IIterator Iterator => _cpuIterator;
        public IImageMapper ImageMapper => _cpuImageMapper;

        public MandelbrotRenderer()
        {
            _cpuIterator = new CpuIterator();
            _cpuImageMapper = new CpuImageMapper();
        }

        public ImageSource GenerateImage(int width, int height, ComplexDouble center, double step, int[] palette)
        {
            // Generate a new bitmap if necessary
            if (_bitmap == null || _bitmap.PixelWidth != width || _bitmap.PixelHeight == height)
            {
                _iterations = new int[width, height];
                _bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr24, null);
                _pixels = new byte[width * height * 3];
            }

            // Find the bottom left in the complex plane
            var start = new ComplexDouble(center.A - (width / 2) * step, center.B - (height / 2) * step);

            // Do the Mandelbrot thing
            Iterator.IterateRange(start, width, height, step, palette.Length - 1, _iterations!);

            // Map to an image using the palette
            ImageMapper.Map(_iterations!, palette, _pixels!);

            // Write the pixels to the bitmap
            _bitmap.WritePixels(new Int32Rect(0, 0, width, height), _pixels, width * 3, 0, 0);

            // Write a dot in the center
            //_bitmap.WritePixels(new Int32Rect(width / 2, height / 2, 1, 1), new byte[] { 255, 255, 255, 0 }, 4, 0);

            return _bitmap;
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
                }
                _disposed = true;
            }
        }
    }
}
