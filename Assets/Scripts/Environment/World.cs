using System;
using Environment.Cells;
using UnityEngine;

namespace Environment
{
    public class World : Map<Cell>
    {
        public World(Vector2Int size) : base(size)
        { }
        
        protected override Cell Empty => null;
        protected override bool EqualWithEmpty(Cell source)
        {
            return source != Empty;
        }
    }
}