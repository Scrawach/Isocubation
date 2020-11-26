using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    [CreateAssetMenu(fileName = "Map Data", menuName = "Isocubation/Environment/Map Data", order = 3)]
    public class RandomMapData : ScriptableObject
    {
        public MapWalkerData WalkerData;
        public RectInt Borders;
        public Vector2Int StartPosition;
        public int InitWalkersCount;
        public int MinWalkerCount;
        public int MaxWalkerCount;
        public float ChanceKillWalker;
        public float ChanceSpawnWalker;
        public int MinDistanceBetweenPointers;
        public float PercentToFill;

        public Vector2Int GetMapSize()
        {
            return Borders.max - Borders.min;
        }

        public int GetMaxObjectsCount()
        {
            var size = GetMapSize();
            return (int)(size.x * size.y * PercentToFill);
        }
    }
}