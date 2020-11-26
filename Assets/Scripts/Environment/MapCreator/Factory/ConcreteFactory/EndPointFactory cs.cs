using UnityEngine;

namespace Environment.MapObjects
{
    public class EndPointFactory : PointFactory
    {
        public EndPointFactory(Map2D map) : base(map)
        {
            NeedPoint = true;
        }
        
        public override MapObjectSymbol Create(Vector2Int position)
        {
            Map.ApplyStartPosition(position);
            NeedPoint = false;
            return MapObjectSymbol.End;
        }
    }
}