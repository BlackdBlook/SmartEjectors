namespace SmartEjectors
{
    public static class Util
    {
        public static bool isSphereFilled(DysonSphere sphere)
        {
            return sphere.totalConstructedCellPoint + sphere.swarm.sailCount >= sphere.totalCellPoint;
        }
    }
}
