using System;
using System.Collections.Generic;
using System.Linq;
using MLA;

namespace AI.Old
{
    class KmeansImageProcessing
    {
        static void ProcessImage()
        {
            Color[,] image = ImageViewer.LoadImage(@"C:\Users\theus\Desktop\kasteel.jpeg");
            MLA.KMeans.Vector[] data = FromColors(image);
            int width = image.GetLength(1), height = image.GetLength(0);
            int nClusters = 12;
            KMeansCalculator k = new KMeansCalculator();
            k.SetInputData(data);
            k.SetNumberOfClusters(nClusters);
            ClusterResult result = k.Cluster(10);

            List<Vector> borderData = new List<Vector>();

            data = data.Select((v, idx) =>
            {

                int centroidIndex = result.FinalCentroidAssignments[idx];
                if (idx - width > 0 && idx + width < data.Length && idx - 1 > 0 && idx + 1 < data.Length)
                {
                    if (centroidIndex != result.FinalCentroidAssignments[idx - 1]
                    || centroidIndex != result.FinalCentroidAssignments[idx + 1]
                    || centroidIndex != result.FinalCentroidAssignments[idx + width]
                    || centroidIndex != result.FinalCentroidAssignments[idx - width])
                    {
                        v.Values[0] = 0;
                        v.Values[1] = 0;
                        v.Values[2] = 0;

                        int y = idx / width;
                        int x = idx % width;
                        borderData.Add(new Vector(2) { Values = new double[] { x, y } });
                        return v;
                    }
                }

                return result.FinalCentroids[centroidIndex].Mean;//result.FinalCentroids[centroidIndex].Mean;

            }
            ).ToArray();


            Random r = new Random((int)(DateTime.UtcNow.Ticks % int.MaxValue));
            //int n = nClusters * 2;
            //k.SetInputData(borderData.ToArray());
            //k.SetNumberOfClusters(n);
            //ClusterResult borderResult = k.Cluster(1);

            //for (int i = 0; i < borderResult.FinalCentroids.Length; i++)
            //{

            //    Centroid c = borderResult.FinalCentroids[i];
            //    Color centroidColor = Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            //    for (int j = 0; j < c.Children.Count; j++)
            //    {

            //        Vector child = c.Children[j];
            //        int x = (int)child.Values[0];
            //        int y = (int)child.Values[1];
            //        Vector pixel = data[y * width + x];
            //        //pixel.Values[0] = centroidColor.R;
            //        //pixel.Values[1] = centroidColor.G;
            //        //pixel.Values[2] = centroidColor.B;
            //    }

            //}


            Color[,] newImage = FromVectors(data, width, height);
            Bitmap borderImage = ImageViewer.CreateBitmapFromColors(newImage);

            //foreach (Centroid c in borderResult.FinalCentroids)
            //{
            //    DrawCircleOnBitmap(borderImage, (int)c.Mean.Values[0], (int)c.Mean.Values[1], 5);
            //}
            ImageViewer.DrawImage(borderImage);
            Console.ReadLine();
        }

        private static Vector[] FromColors(Color[,] image)
        {
            int width = image.GetLength(1);
            int height = image.GetLength(0);

            Vector[] result = new Vector[width * height];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Vector v = new Vector(3);
                    Color currentColor = image[i, j];

                    v.Values[0] = currentColor.R;
                    v.Values[1] = currentColor.G;
                    v.Values[2] = currentColor.B;
                    result[i * width + j] = v;
                }
            }

            return result;
        }

        private static Color[,] FromVectors(Vector[] data, int width, int height)
        {
            Color[,] image = new Color[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Vector currentVector = data[j + i * width];
                    image[i, j] = Color.FromArgb((byte)currentVector.Values[0], (byte)currentVector.Values[1], (byte)currentVector.Values[2]);
                }
            }

            return image;
        }

        private static void DrawCircleOnBitmap(Bitmap bm, int x, int y, int radius, Color? circleColor = null)
        {
            Rectangle area = new Rectangle(x - radius, y - radius, radius * 2, radius * 2);
            int middleX = x, middleY = y;
            for (x = area.X; x < area.Right; x++)
            {
                for (y = area.Y; y < area.Bottom; y++)
                {
                    if (x < 0 || x >= bm.Width || y < 0 || y >= bm.Height)
                        continue;

                    double deltaX = middleX - x;
                    double deltaY = middleY - y;

                    if (Math.Sqrt(deltaX * deltaX + deltaY * deltaY) <= radius)
                    {
                        bm.SetPixel(x, y, circleColor.HasValue ? circleColor.Value : Color.Black);
                    }
                }
            }
        }
    }
}
