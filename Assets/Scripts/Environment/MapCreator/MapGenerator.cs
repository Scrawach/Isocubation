using System.IO;
using UnityEngine;

namespace Environment
{
    public class MapGenerator
    {
        public void WriteTo(string path, Map2D map)
        {
            using (var sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                for (var i = 0; i < map.Width; i++)
                {
                    for (var j = 0; j < map.Height; j++)
                    {
                        var position = new Vector2Int(i, j);
                        var symbol = map.Get(position);
                        sw.Write((char)symbol);
                    }
                    sw.WriteLine();
                }
            }
        }

        public Map2D Random(RandomMapData mapData, int seed)
        {
            var creator = new RandomMapCreator(mapData, seed);
            return Generate(creator);
        }
        
        public Map2D LoadFrom(string path)
        {
            var creator = new MapLoader(path);
            return Generate(creator);
        }

        private Map2D Generate(IMapCreator creator)
        {
            return creator.Create();
        }
    }
}