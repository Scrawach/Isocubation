using System;
using System.Collections.Generic;
using Environment.MapObjects;
using UnityEngine;

namespace Environment
{
    [CreateAssetMenu(fileName = "Map Objects Data", menuName = "Isocubation/Environment/Map Objects Data", order = 2)]
    public class MapObjectDecoderData : ScriptableObject
    {
        [SerializeField] private MapObject[] _objects;

        public MapObject[] Objects => _objects;
    }
}