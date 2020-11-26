using UnityEngine;

namespace Environment.MapObjects
{
    public class StartPointFactory : PointFactory
    {
        public StartPointFactory(Map2D map) : base(map)
        {
            NeedPoint = true;
        }
        
        public override MapObjectSymbol Create(Vector2Int position)
        {
            Map.ApplyStartPosition(position);
            NeedPoint = false;
            return MapObjectSymbol.Start;
        }
    }
}