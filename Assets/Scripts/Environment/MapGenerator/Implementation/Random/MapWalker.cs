using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Environment
{
    public class MapWalker
    {
        private readonly MapWalkerData _data;
        private readonly RectInt _borders;

        public MapWalker(MapWalkerData data, Vector2Int position, RectInt borders)
        {
            _data = data ? data : throw new ArgumentException("Map Walker Data is invalid!");
            _borders = borders;
            Position = position;
        }
        
        public Vector2Int Position { get; private set; }
        public Vector2Int Direction { get; set; }

        public void MoveRandom()
        {
            Direction = GetNextDirection(_data.ChanceChangeDirection);
            Position = GetNextPosition(_data.StepsForUpdate, _borders);
        }
        
        private Vector2Int GetNextPosition(int steps, RectInt borders)
        {
            var nextPosition = DoSteps(steps);
            nextPosition.Clamp(borders.min, borders.max - Vector2Int.one);
            
            return nextPosition;
        }
        
        private Vector2Int DoSteps(int count)
        {
            return Position + count * Direction;
        }
        
        private Vector2Int GetNextDirection(float changeDirectionChance)
        {
            return CheckChance(changeDirectionChance) ? _data.GetRandomDirection() : Direction;
        }
        
        private bool CheckChance(float chance)
        {
            return Random.value < chance;
        }
    }
}
