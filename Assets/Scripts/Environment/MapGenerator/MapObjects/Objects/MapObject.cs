using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Environment
{
    public class MapObject
    {
        public MapObject(Vector2Int position)
        {
            Coords = position;
        }

        public virtual char Symbol { get; }
        public Vector2Int Coords { get; }

    }
}