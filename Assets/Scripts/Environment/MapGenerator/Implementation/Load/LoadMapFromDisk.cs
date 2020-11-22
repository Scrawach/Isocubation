using System;
using System.Collections.Generic;
using System.IO;
using Environment.MapObjects;
using UnityEngine;

namespace Environment.Implementation.Load
{
    public class LoadMap : IMapCreator
    {
        private readonly MapObjectSymbol[] _objects;
        private readonly string _path;

        public LoadMap(string path, MapObjectsData mapObjectsData)
        {
            _path = path;
            _objects = mapObjectsData.Objects;
        }
        
        public Map Create()
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

        private Map ConvertToMap(IReadOnlyList<string> text)
        {
            var size = new Vector2Int(text[0].Length, text.Count);
            var map = new Map(size);

            for (var i = 0; i < size.x; i++)
            {
                var line = text[i];
                for (var j = 0; j < size.y; j++)
                {
                    var position = new Vector2Int(i, j);
                    var symbol = line[j];
                    
                    if (symbol != '\0')
                        map.ApplySymbol(position, symbol);
                    
                    if (symbol == (char) MapObjectSymbol.Start)
                      map.ApplyStartPosition(position);
                }
            }

            return map;
        }

        private MapObjectSymbol GetObjectBySymbol(char symbol)
        {
            foreach (var obj in _objects)
            {
                if (symbol == (char)obj)
                {
                    return obj;
                }
            }

            throw new Exception("Invalid map format!");
        }
    }
}