namespace Mandelbrot
{
    public class CpuIterator : IIterator
    {
        public int Iterate(ComplexDouble c, int limit)
        {
            ComplexDouble z = new ComplexDouble(0, 0);

            for (var i = 0; i < limit; i++)
            {
                z = z * z + c;

                if (z.ModulusSquared >= 4)
                    return i;
            }

            return limit;
        }

        public void IterateRange(ComplexDouble start, int width, int height, double step, int limit, int[,] output)
        {
            Parallel.For(0, height, b =>
            {
                Parallel.For(0, width, a =>
                {
                    var c = new ComplexDouble(start.A + a * step, start.B + b * step);
                    output[a, b] = Iterate(c, limit);
                });
            });
        }
    }
}
