using Environment.Cells;
using UnityEngine;

namespace Environment
{
    public class WorldCreator : WorldCreatorBase
    {
        public WorldCreator(Environment environment, MapObjectDecoderData decoderData) 
            : base(environment, decoderData)
        { }

        public World Create(Map2D map, Vector2Int offset)
        {
            World = new World(new Vector2Int(map.Width, map.Height));

            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    var position = new Vector2Int(x, y);
                    TryCreateObject(position, offset, map.Get(position));
                }
            }

            return World;
        }
    }
}