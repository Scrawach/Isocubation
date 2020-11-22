using System.Collections.Generic;
using Environment.MapObjects;
using UnityEngine;

namespace Environment
{
    [CreateAssetMenu(fileName = "Map Objects Data", menuName = "Isocubation/Environment/Map Objects Data", order = 2)]
    public class MapObjectsData : ScriptableObject
    {
        [SerializeField] private MapObjectSymbol[] _objects;
        
        public MapObjectSymbol[] Objects => _objects;
    }
}