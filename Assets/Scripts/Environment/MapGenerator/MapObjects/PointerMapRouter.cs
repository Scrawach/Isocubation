using System.Collections.Generic;
using UnityEngine;

namespace Environment.MapObjects.Data
{
    public class PointerMapRouter
    {
        private Map _map;
        private List<Vector2Int> _pointersCoords;

        public PointerMapRouter(Map map)
        {
            _map = map;
            _pointersCoords = new List<Vector2Int>();
        }

        public void ApplyPointer(PointerMap pointer)
        {
            _pointersCoords.Add(pointer.Coords);
        }

        public bool CheckDistanceBetween(Vector2Int position)
        {
            var minMagnitude = float.MaxValue;

            foreach (var coords in _pointersCoords)
            {
                var magnitude = (position - coords).magnitude;

                if (magnitude < minMagnitude)
                {
                    minMagnitude = magnitude;
                }
            }

            return (minMagnitude > 10);
        }

        public void CreateFloorsAround(Vector2Int position)
        {
            _map.ApplySymbolAround(position, (char)MapObjectSymbol.Floor);
        }
    }
}