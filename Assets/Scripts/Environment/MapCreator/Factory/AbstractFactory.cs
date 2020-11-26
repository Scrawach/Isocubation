using UnityEngine;

namespace Environment.MapObjects
{
    public abstract class AbstractFactory
    {
        public abstract bool CanCreate(Vector2Int position);
        public abstract MapObjectSymbol Create(Vector2Int position);
    }
}