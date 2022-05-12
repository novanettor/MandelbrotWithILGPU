namespace Mandelbrot
{
    public interface IImageMapper
    {
        /// <summary>
        /// Generates a pixel bitmap from an iterations map by indexing each iterations value into a palette.
        /// </summary>
        /// <param name="iterationsMap">The iterations map, a 2D array of <see cref="int"/>s</param>
        /// <param name="palette">The palette to apply, representing an array of Bgr24 color values</param>
        /// <param name="output">The resulting bitmap</param>
        void Map(int[,] iterationsMap, int[] palette, byte[] output);
    }
}
