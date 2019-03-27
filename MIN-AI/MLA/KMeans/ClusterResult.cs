namespace MLA.KMeans
{
    public class ClusterResult
    {
        public Centroid[] FinalCentroids { get; }

        public int[] FinalCentroidAssignments { get; }

        public ClusterResult(Centroid[] finalCentroids, int[] finalCentroidAssignments)
        {
            FinalCentroids = finalCentroids;
            FinalCentroidAssignments = finalCentroidAssignments;
        }
    }
}
