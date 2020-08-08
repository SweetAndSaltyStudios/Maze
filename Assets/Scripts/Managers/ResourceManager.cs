using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class ResourceManager : Singelton<ResourceManager>
    {
        [Header("Prefabs")]
        [Space]
        public Cell CellPrefab;
        public BallEngine BallPrefab;
        public Goal GoalPrefab;
        public ViewMask ViewMaskPrefab;
    }
}
