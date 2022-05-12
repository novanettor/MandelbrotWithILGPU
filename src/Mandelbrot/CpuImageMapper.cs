namespace Mandelbrot
{
    public class CpuImageMapper : IImageMapper
    {
        public void Map(int[,] iterations, int[] palette, byte[] output)
        {
            var height = iterations.GetLength(1);
            var width = iterations.GetLength(0);
            Parallel.For(0, height, b =>
            {
                Parallel.For(0, width, a =>
                {
                    var color = palette[iterations[a, b]];

                    // Screen space is inverted along y, so b -> (height - 1 - b)
                    var pixelIndex = ((height - 1 - b) * width + a) * 3;
                    unsafe
                    {
                        var colorBytes = (byte*)&color;
                        output[pixelIndex] = colorBytes[0];
                        output[pixelIndex + 1] = colorBytes[1];
                        output[pixelIndex + 2] = colorBytes[2];
                    }
                });
            });
        }
    }
}
