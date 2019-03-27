using System;
using System.Collections.Generic;

namespace MLA.KMeans
{
    public class Centroid
    {
        public Vector Mean => mean;

        public List<Vector> Children => children;

        private Vector mean;

        private Vector oldMean;

        private List<Vector> children;

        public Centroid(Vector initialMean)
        {
            mean = initialMean;
            children = new List<Vector>();
        }

        public double Distance(Vector v)
        {
            return v.Distance(this.mean);
        }


        public bool HasMoved()
        {
            return oldMean.Distance(mean) > 0d;
        }

        public void UpdateMean()
        {
            Vector sum = new Vector(mean.Dimensions);

            for (int i = 0; i < children.Count; i++)
            {
                sum = sum + children[i];
            }

            oldMean = mean;
            mean = sum / (double)children.Count;
        }

        public void ClearChildren()
        {
            children = new List<Vector>();
        }

        public Centroid Clone()
        {
            Centroid c = new Centroid(this.mean);
            c.Children.AddRange(this.children);
            return c;
        }
    }
}
