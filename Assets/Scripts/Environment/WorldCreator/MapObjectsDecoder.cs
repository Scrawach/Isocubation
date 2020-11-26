using System;
using System.Collections.Generic;
using Environment.Cells;
using Environment.MapObjects;
using UnityEngine;

namespace Environment
{
    public class MapObjectsDecoder
    {
        private Dictionary<MapObjectSymbol, GameObject> _dictionary;
        
        public MapObjectsDecoder(MapObjectDecoderData data)
        {
            SetupDictionary(data);
        }

        public bool TryGetObject(MapObjectSymbol key, out GameObject obj)
        {
            return _dictionary.TryGetValue(key, out obj);
        }

        private void SetupDictionary(MapObjectDecoderData data)
        {
            var objects = data.Objects;
            _dictionary = new Dictionary<MapObjectSymbol, GameObject>(objects.Length);
            foreach (var obj in objects)
            {
                _dictionary.Add(obj.Symbol, obj.Prefab);
            }
        }
    }
}