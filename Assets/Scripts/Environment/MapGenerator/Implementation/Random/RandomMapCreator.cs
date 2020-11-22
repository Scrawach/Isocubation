using System.Threading;
using Environment.MapObjects;
using UnityEngine;

namespace Environment
{
    public class RandomMapCreator : IMapCreator
    {
        private readonly RandomMapData _data;
        private readonly MapObjectSymbol[] _objects;
        private MapObjectsFactory _mapObjectsFactory;
        private RectInt _walkersBorder;

        private int _createdObjects;

        public RandomMapCreator(RandomMapData data, MapObjectsData mapObjectsData)
        {
            _data = data;
            _createdObjects = 0;
            _objects = mapObjectsData.Objects;
        }
        
        public Map Create()
        {
          Random.InitState(10);
          
            var size = _data.GetMapSize();
            var map = new Map(size);
            var maxCreatedObjects = _data.GetMaxObjectsCount();
            
            var walkersBorders = new RectInt(Vector2Int.one, size - 2 * Vector2Int.one);
            var walkersRouter = new MapWalkersRouter(walkersBorders);
            _mapObjectsFactory = new MapObjectsFactory(_objects, map);

            CreateWalkers(walkersRouter, _data.WalkerData, _data.StartPosition, _data.InitWalkersCount);

            while (maxCreatedObjects > _createdObjects)
            {
                UpdateWalkers(map, walkersRouter);
            }

            return map;
        }

        private void CreateWalkers(MapWalkersRouter router, MapWalkerData data, Vector2Int position, int count)
        {
            for (var i = 0; i < count; i++)
            {
                router.CreateWalker(data, position);
            }
        }
        
        private void UpdateWalkers(Map map, MapWalkersRouter router)
        {
            foreach (var walker in router.Walkers)
            {
                TryCreateMapObject(map, walker.Position);
                walker.MoveRandom();
            }
            
            router.TryCreateWalker(_data.ChanceSpawnWalker, _data.WalkerData, _data.MaxWalkerCount);
            router.TryKillWalker(_data.ChanceKillWalker, _data.MinWalkerCount);
        }
        
        private void TryCreateMapObject(Map map, Vector2Int position)
        {
            if (map.Exist(position))
                return;

            var obj = _mapObjectsFactory.CreateMapObject(map, position);
            _createdObjects++;
        }
    }
}