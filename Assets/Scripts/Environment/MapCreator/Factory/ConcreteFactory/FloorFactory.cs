using UnityEngine;

namespace Environment.MapObjects
{
    public class FloorFactory : AbstractFactory
    {
        public override bool CanCreate(Vector2Int position) => true;

        public override MapObjectSymbol Create(Vector2Int position)
        {
            return MapObjectSymbol.Floor;
        }
    }
}