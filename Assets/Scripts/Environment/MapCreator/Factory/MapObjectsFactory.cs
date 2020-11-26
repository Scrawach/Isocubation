using System;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.MapObjects
{
    public class MapObjectsFactory
    {
        private readonly AbstractFactory[] _objectFactories;

        public MapObjectsFactory(AbstractFactory[] objectFactories)
        {
            _objectFactories = objectFactories;
        }

        public MapObjectSymbol Create(Vector2Int position)
        {
            foreach (var factory in _objectFactories)
            {
                if (factory.CanCreate(position) == false) 
                    continue;
                
                return factory.Create(position);
            }
            
            throw new Exception("Invalid factories array!");
        }
    }
}