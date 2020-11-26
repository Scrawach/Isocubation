using System.Collections.Generic;
using UnityEngine;

namespace Environment.MapObjects
{
    public class PointerFactory : AbstractFactory
    {
        private readonly Map2D _map;
        private readonly List<Vector2Int> _pointers;
        private readonly int _distanceBetweenPointers;

        public PointerFactory(Map2D map, int minDistanceBetween)
        {
            _map = map;
            _pointers = new List<Vector2Int>();
            _distanceBetweenPointers = minDistanceBetween;
        }

        public override bool CanCreate(Vector2Int position)
        {
            return ValidDistanceBetweenPointers(position);
        }

        public override MapObjectSymbol Create(Vector2Int position)
        {
            _map.ApplyAround(position, MapObjectSymbol.Floor);
            _pointers.Add(position);
            return MapObjectSymbol.Pointer;
        }

        private bool ValidDistanceBetweenPointers(Vector2Int currentPointer)
        {
            var min = float.MaxValue;

            foreach (var position in _pointers)
            {
                var magnitude = (currentPointer - position).magnitude;
                if (magnitude < min)
                {
                    min = magnitude;
                }
            }

            return min > _distanceBetweenPointers;
        }
    }
}