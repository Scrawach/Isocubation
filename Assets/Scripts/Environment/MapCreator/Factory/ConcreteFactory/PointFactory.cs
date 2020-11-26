using UnityEngine;

namespace Environment.MapObjects
{
    public class PointFactory : AbstractFactory
    {
        protected Map2D Map;
        protected bool NeedPoint;

        public PointFactory(Map2D map)
        {
            Map = map;
        }

        public override bool CanCreate(Vector2Int position)
        {
            return NeedPoint;
        }

        public override MapObjectSymbol Create(Vector2Int position)
        {
            return MapObjectSymbol.Floor;
        }
    }
}