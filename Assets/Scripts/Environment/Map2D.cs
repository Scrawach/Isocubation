using Environment.MapObjects;
using UnityEngine;

namespace Environment
{
    public class Map2D : Map<MapObjectSymbol>
    {
        private Vector2Int _start;

        public Map2D(Vector2Int size) : base(size)
        { }
        
        protected override MapObjectSymbol Empty => MapObjectSymbol.Empty;
        
        protected override bool EqualWithEmpty(MapObjectSymbol source)
        {
            return source != Empty;
        }
        
        public Vector2Int Start => _start;

        public void ApplyStartPosition(Vector2Int index)
        {
            if (InBorder(index))
            {
                _start = index;
            }
        }
    }
}