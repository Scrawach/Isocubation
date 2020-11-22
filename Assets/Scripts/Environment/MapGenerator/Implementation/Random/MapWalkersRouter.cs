using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Environment
{
    public class MapWalkersRouter
    {
        private readonly List<MapWalker> _walkers;
        private readonly RectInt _borders;

        public MapWalkersRouter(RectInt borders)
        {
            _borders = borders;
            _walkers = new List<MapWalker>();
        }

        public IEnumerable<MapWalker> Walkers => _walkers;
        
        public void CreateWalker(MapWalkerData data, Vector2Int position)
        {
            var walker = new MapWalker(data, position, _borders);
            _walkers.Add(walker);
        }

        public void TryCreateWalker(float chance, MapWalkerData data, int maxCount)
        {
            if (CheckChance(chance) == false)
                return;

            if (_walkers.Count >= maxCount)
                return;

            var index = Random.Range(0, _walkers.Count);
            CreateWalker(data, _walkers[index].Position);
        }

        public void TryKillWalker(float chance, int minCount)
        {
            if (CheckChance(chance) == false)
                return;

            if (_walkers.Count <= minCount)
                return;

            var index = Random.Range(0, _walkers.Count);
            _walkers.RemoveAt(index);
        }
        
        private bool CheckChance(float chance)
        {
            return Random.value < chance;
        }
    }
}