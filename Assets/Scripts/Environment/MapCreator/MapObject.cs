using System;
using Environment.MapObjects;
using UnityEngine;

namespace Environment
{
    [Serializable]
    public class MapObject
    {
        [SerializeField] private MapObjectSymbol _symbol;
        [SerializeField] private GameObject _prefab;

        public MapObjectSymbol Symbol => _symbol;
        public GameObject Prefab => _prefab;
    }
}