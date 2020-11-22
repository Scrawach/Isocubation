using System;
using System.Collections.Generic;
using Environment.MapObjects.Data;
using UnityEngine;

namespace Environment.MapObjects
{
    public class MapObjectsFactory
    {
        private readonly IReadOnlyList<MapObjectSymbol> _mapObjects;
        private readonly PointerMapRouter _pointerMapRouter;
        private bool _needEntryPoint;
        private MapObject _current;
        
        public MapObjectsFactory(IReadOnlyList<MapObjectSymbol> mapObjects, Map map)
        {
            _mapObjects = mapObjects;
            _pointerMapRouter = new PointerMapRouter(map);
            _needEntryPoint = true;
        }

        public MapObject CreateMapObject(Map map, Vector2Int spawnPosition)
        {
            foreach (var mapObject in _mapObjects)
            {
                switch (mapObject)
                {
                    case MapObjectSymbol.Start when _needEntryPoint:
                        _needEntryPoint = false;
                        map.ApplyStartPosition(spawnPosition);
                        map.ApplySymbol(spawnPosition, (char)MapObjectSymbol.Start);
                        return new StartFloorMap(spawnPosition);
                    case MapObjectSymbol.Floor:
                        map.ApplySymbol(spawnPosition, (char)MapObjectSymbol.Floor);
                        return new FloorMap(spawnPosition);
                    case MapObjectSymbol.Pointer when _pointerMapRouter.CheckDistanceBetween(spawnPosition):
                        var createdPointer = CreatePointerMap(spawnPosition);
                        map.ApplySymbol(spawnPosition, (char)MapObjectSymbol.Pointer);
                        return createdPointer;
                }
            }
            
            throw new Exception("Invalid map objects!");
        }

        private PointerMap CreatePointerMap(Vector2Int position)
        {
            var createdPointer = new PointerMap(position);
            _pointerMapRouter.CreateFloorsAround(position);
            _pointerMapRouter.ApplyPointer(createdPointer);
            return createdPointer;
        }
    }
}