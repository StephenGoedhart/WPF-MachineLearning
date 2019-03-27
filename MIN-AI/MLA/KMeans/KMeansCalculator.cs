using System;
using System.Collections.Generic;
using System.Linq;

namespace MLA.KMeans
{
    public class KMeansCalculator
    {
        public Centroid[] GetCurrentCentroids() => centroids;

        private Centroid[] centroids;
        private int[] assignedCentroids;
        private int nClusters;

        private Vector[] inputData;
        private int dimensions;

        public void SetNumberOfClusters(int n, bool keepOld = false)
        {
            if (centroids == null)
            {
                centroids = new Centroid[n];
            }

            if (n != nClusters)
            {
                Array.Resize(ref centroids, n);
            }

            nClusters = n;
        }

        public void SetInputData(Vector[] inputData)
        {
            this.inputData = inputData;
        }

        public ClusterResult Cluster(int maxIterations)
        {
            centroids = GenerateRandomCentroids(nClusters, inputData);
            int lenData = inputData.Length;
            assignedCentroids = new int[lenData];

            for (int x = 0; x < maxIterations; x++)
            {
                Iterate();

                if (centroids.All(c => !c.HasMoved()))
                {
                    break;
                }
            }

            return new ClusterResult(centroids, assignedCentroids);
        }

        public Centroid[] Iterate()
        {
            for(int i = 0; i < centroids.Length; i++)
                centroids[i].ClearChildren();

            for (int i = 0; i < inputData.Length; i++)
            {
                Vector currentDataPoint = inputData[i];
                int centroidIndex = GetClosestCentroidIndex(currentDataPoint, centroids);
                assignedCentroids[i] = centroidIndex;
                centroids[centroidIndex].Children.Add(currentDataPoint);
            }

            for (int i = 0; i < nClusters; i++)
            {
                Centroid centroid = centroids[i];
                centroid.UpdateMean();
            }

            return centroids;
        }

        private int GetClosestCentroidIndex(Vector dataPoint, Centroid[] centroids)
        {
            double closestDistance = double.MaxValue;
            int closestIdx = -1;
            for(int i = 0; i < centroids.Length; i++)
            {
                double distance = centroids[i].Distance(dataPoint);
                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIdx = i;
                }
            }

            return closestIdx;
        }

        private Centroid[] GenerateRandomCentroids(int nClusters, Vector[] dataPoints)
        {
            Random r = new Random((int)(DateTime.UtcNow.Ticks % int.MaxValue));

            Centroid[] centroids = new Centroid[nClusters];

            HashSet<int> usedIndexes = new HashSet<int>();
            for(int i = 0; i < nClusters; i++)
            {
                int idx;
                do
                {
                    idx = r.Next(0, dataPoints.Length);
                } while (!usedIndexes.Add(idx));

                centroids[i] = new Centroid(dataPoints[idx]);
            }

            return centroids;
        }
    }
}
