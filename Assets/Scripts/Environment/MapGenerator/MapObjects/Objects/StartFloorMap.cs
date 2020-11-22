using Environment.MapObjects;
using UnityEngine;

namespace Environment.MapObjects
{
    public class StartFloorMap : MapObject
    {
        public StartFloorMap(Vector2Int position) : base(position)
        { }

        public override char Symbol => (char) MapObjectSymbol.Start;
    }
}