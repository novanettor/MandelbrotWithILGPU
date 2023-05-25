namespace Mandelbrot
{
    public struct ComplexDouble : IEquatable<ComplexDouble>
    {
        public double A;
        public double B;

        public ComplexDouble(double a, double b)
        {
            A = a;
            B = b;
        }

        public static ComplexDouble operator +(ComplexDouble x, ComplexDouble y)
            => new ComplexDouble(x.A + y.A, x.B + y.B);

        public static ComplexDouble operator *(ComplexDouble x, ComplexDouble y)
            => new ComplexDouble(x.A * y.A - x.B * y.B, x.A * y.B + x.B * y.A);

        public double ModulusSquared => A * A + B * B;

        public double Modulus => Math.Sqrt(ModulusSquared);

        #region Tedious stuff

        public bool Equals(ComplexDouble other)
        {
            return Equals(A, other.A) && Equals(B, other.B);
        }

        public override bool Equals(object obj)
        {
            return (obj is ComplexDouble cd) ? Equals(cd) : false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (391 + A.GetHashCode()) * 23 + B.GetHashCode();
            }
        }

        public override string ToString() => $"({A},{B})";

        public static bool operator ==(ComplexDouble x, ComplexDouble y)
            => Equals(x, y);

        public static bool operator !=(ComplexDouble x, ComplexDouble y)
            => !Equals(x, y);

        #endregion
    }
}
