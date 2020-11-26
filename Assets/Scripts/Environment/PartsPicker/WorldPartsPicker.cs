using System.Collections.Generic;

namespace Environment
{
    public abstract class WorldPartsPicker
    {
        private List<Map2D> _mapParts;
        public int Position { get; protected set; }
        
        public WorldPartsPicker(List<Map2D> maps)
        {
            _mapParts = maps;
            Count = _mapParts.Count;
        }
        
        public int Count { get; }

        public Map2D Current => _mapParts[Position];

        public abstract Map2D Next();

        public void Reset()
        {
            Position = 0;
        }
    }
}