using System.IO;
using Environment.Implementation.Load;
using UnityEngine;

namespace Environment
{
    public class MapGenerator
    {
        public void WriteTo(string path, Map map)
        {
            using (var sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                for (var i = 0; i < map.Width; i++)
                {
                    for (var j = 0; j < map.Height; j++)
                    {
                        var position = new Vector2Int(i, j);
                        var symbol = map.GetSymbolAt(position);
                        sw.Write(symbol);
                    }
                    sw.WriteLine();
                }
            }
        }

        public Map Random(RandomMapData mapData, MapObjectsData mapObjectsData)
        {
            var creator = new RandomMapCreator(mapData, mapObjectsData);
            return Generate(creator);
        }
        
        public Map LoadFrom(string path, MapObjectsData mapObjectsData)
        {
            var creator = new LoadMap(path, mapObjectsData);
            return Generate(creator);
        }

        private Map Generate(IMapCreator creator)
        {
            return creator.Create();
        }
    }
}