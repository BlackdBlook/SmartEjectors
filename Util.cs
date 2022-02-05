namespace SmartEjectors
{
    public static class Util
    {
        public static bool IsSphereFilled(DysonSphere sphere)
        {
            if (!Config.enableLockEjector.ActiveValue()) return false;

            return sphere.totalConstructedCellPoint + sphere.swarm.sailCount >= sphere.totalCellPoint;
        }

        public static bool IsNodeLimitReached(DysonSphere sphere)
        {
            if (!Config.enableLockEjector.ActiveValue()) return false;
            if (Config.nodeToSailRatio.ActiveValue() == 0) return false;

            int avaliableNodeCount = 0;
            for (int i = 1; i < sphere.layersIdBased.Length; i++)
            {
                if (sphere.layersIdBased[i] == null || sphere.layersIdBased[i].id != i) continue;

                DysonNode[] nodePool = sphere.layersIdBased[i].nodePool;
                for (int j = 1; j < nodePool.Length; j++)
                {
                    if (nodePool[j] == null || nodePool[j].id != j) continue;

                    if (nodePool[j].totalSp > 30 && nodePool[j].totalCp < nodePool[j].totalCpMax) avaliableNodeCount++;
                }
            }

            return sphere.swarm.sailCount >= avaliableNodeCount * Config.nodeToSailRatio.ActiveValue();
        }
    }
}
