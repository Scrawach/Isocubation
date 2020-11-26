using System.Collections;
using UnityEngine;

namespace Environment
{
    public class WorldIterator : WorldCreatorBase
    {
        private readonly float _pauseBetweenCreating;

        public WorldIterator(Environment environment, MapObjectDecoderData decoderData, float pause)
            : base(environment, decoderData)
        {
            _pauseBetweenCreating = pause;
        }

        public IEnumerator Create(Map2D map)
        {
            var start = map.Start;
            World = new World(new Vector2Int(map.Width, map.Height));

            var point = start;
            var size = Vector2Int.zero;

            var min = Vector2Int.zero;
            var max = new Vector2Int(map.Width - 1, map.Height - 1);

            while (true)
            {
                var rect = new RectInt(point, size);
                PerimeterCreate(map, rect);
                yield return new WaitForSeconds(_pauseBetweenCreating);

                point -= Vector2Int.one;

                var x = point.x < 0 ? 1 : 2;
                var y = point.y < 0 ? 1 : 2;
                
                size += new Vector2Int(x, y);
                
                point.Clamp(min, max);
                size.Clamp(min, max);

                if (point == min && size == max)
                {
                    break;
                }
            }
        }
        
        private void PerimeterCreate(Map2D map, RectInt borders)
        {
            for (var i = borders.xMin; i <= borders.xMax; i++)
            {
                PreparationCreate(new Vector2Int(i, borders.yMin), map);
                PreparationCreate(new Vector2Int(i, borders.yMax), map);
            }

            for (var i = borders.yMin + 1; i <= borders.yMax - 1; i++)
            {
                PreparationCreate(new Vector2Int(borders.xMin, i), map);
                PreparationCreate(new Vector2Int(borders.xMax, i), map);
            }
        }

        private void PreparationCreate(Vector2Int position, Map2D map)
        {
            TryCreateObject(position, Vector2Int.zero, map.Get(position));
        }
    }
}