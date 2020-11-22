using System.Collections.Generic;
using Environment.MapObjects.Data;
using UnityEngine;

namespace Environment.MapObjects
{
    public class PointerMap : MapObject
    {
        public PointerMap(Vector2Int position) : base(position)
        { }

        public override char Symbol => (char) MapObjectSymbol.Pointer;
    }
}