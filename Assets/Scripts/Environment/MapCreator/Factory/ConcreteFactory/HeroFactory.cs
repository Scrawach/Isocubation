using UnityEngine;

namespace Environment.MapObjects
{
    public class HeroFactory : PointFactory
    {
        public HeroFactory(Map2D map) : base(map)
        {
            NeedPoint = true;
        }
        
        public override MapObjectSymbol Create(Vector2Int position)
        {
            Map.ApplyStartPosition(position);
            NeedPoint = false;
            return MapObjectSymbol.Hero;
        }
    }
}