using System;
using System.Collections.Generic;
using System.IO;
using Environment.MapObjects;
using UnityEngine;

namespace Environment
{
    public class MapLoader : IMapCreator
    {
        private readonly string _path;

        public MapLoader(string path)
        {
            _path = path;
        }
        
        public Map2D Create()
        {
            var text = ReadFrom(_path);
            return ConvertToMap(text);
        }

        private List<string> ReadFrom(string path)
        {
            var text = new List<string>();
            using (var sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    text.Add(line);
                }
            }

            return text;
        }

        private Map2D ConvertToMap(IReadOnlyList<string> text)
        {
            var size = new Vector2Int(text[0].Length, text.Count);
            var map = new Map2D(size);
            

            for (var i = 0; i < size.y; i++)
            {
                var line = text[size.y - 1 - i];
                for (var j = 0; j < size.x; j++)
                {
                    var position = new Vector2Int(j, i);
                    var symbol = line[j];
                    var ch = ((int) symbol).ToString();

                    if (!Enum.TryParse(ch, out MapObjectSymbol result)) 
                        continue;
                    
                    map.Apply(position, result);
                    if (symbol == (char) MapObjectSymbol.Start)
                        map.ApplyStartPosition(position);
                }
            }

            return map;
        }
    }
}