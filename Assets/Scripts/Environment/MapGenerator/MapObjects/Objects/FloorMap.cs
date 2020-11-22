using UnityEngine;

namespace Environment.MapObjects
{
    public class FloorMap : MapObject
    {
        public FloorMap(Vector2Int position) : base(position)
        { }
        
        public override char Symbol => (char) MapObjectSymbol.Floor;
    }
}