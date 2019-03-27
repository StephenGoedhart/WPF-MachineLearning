using System;

namespace MLA.KMeans
{
    public class Vector
    {
        public double[] Values;

        public int Dimensions;

        public Vector(int dimensions)
        {
            this.Dimensions = dimensions;
            this.Values = new double[dimensions];
        }

        public double Length()
        {
            double sum = 0;
            for(int i = 0; i < Dimensions; i++)
            {
                double v = Values[i];
                sum += v * v;
            }

            return Math.Sqrt(sum);
        }

        public double Distance(Vector other)
        {
            return (this - other).Length();
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            VerifyEqualDimensions(v1, v2);

            Vector result = new Vector(v1.Dimensions);
            for(int i = 0; i < v1.Dimensions; i++)
            {
                result.Values[i] = v1.Values[i] - v2.Values[i];
            }

            return result;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            VerifyEqualDimensions(v1, v2);
            Vector result = new Vector(v1.Dimensions);

            for(int i = 0; i < v1.Dimensions; i++)
            {
                result.Values[i] = v1.Values[i] + v2.Values[i];
            }

            return result;
        }

        public static Vector operator *(Vector v1, double x)
        {
            Vector result = new Vector(v1.Dimensions);
            for(int i = 0; i < v1.Dimensions; i++)
            {
                result.Values[i] = v1.Values[i] * x;
            }

            return result;
        }

        public static Vector operator /(Vector v1, double x)
        {
            Vector result = new Vector(v1.Dimensions);
            for (int i = 0; i < v1.Dimensions; i++)
            {
                result.Values[i] = v1.Values[i] / x;
            }

            return result;
        }

        private static void VerifyEqualDimensions(Vector v1, Vector v2)
        {
            if(v1.Dimensions != v2.Dimensions)
            {
                throw new ArgumentException("Dimensions of the given vectors aren't the same.");
            }
        }

        public override bool Equals(object obj)
        {
            Vector other = obj as Vector;
            if(other == null)
            {
                return false;
            }

            VerifyEqualDimensions(this, other);

            for(int i = 0; i < Dimensions; i++)
            {
                if(this.Values[i] != other.Values[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
