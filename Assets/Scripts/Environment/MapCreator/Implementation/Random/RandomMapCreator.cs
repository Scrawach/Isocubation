using Environment.MapObjects;
using UnityEngine;

namespace Environment
{
    public class RandomMapCreator : IMapCreator
    {
        private readonly RandomMapData _data;
        private MapObjectsFactory _mapObjectsFactory;
        private RectInt _walkersBorder;

        private int _createdObjects;

        public RandomMapCreator(RandomMapData data, int seed)
        {
            _data = data;
            _createdObjects = 0;
            Random.InitState(seed);
        }
        
        public Map2D Create()
        {
            var map = CreateEmpty();
            var walkersRouter = SetupWalkersRouter(_data.InitWalkersCount);
            var maxCreatedObjects = _data.GetMaxObjectsCount();
            SetupFactory(map);

            while (maxCreatedObjects > _createdObjects)
            {
                UpdateWalkers(map, walkersRouter);
            }

            return map;
        }

        private Map2D CreateEmpty()
        {
            var size = _data.GetMapSize();
            return new Map2D(size);
        }

        private MapWalkersRouter SetupWalkersRouter(int walkersCount)
        {
            var size = _data.GetMapSize();
            var walkersBorders = new RectInt(Vector2Int.one, size - 2 * Vector2Int.one);
            var walkersRouter = new MapWalkersRouter(walkersBorders);

            for (var i = 0; i < walkersCount; i++)
            {
                walkersRouter.CreateWalker(_data.WalkerData, _data.StartPosition);
            }
            
            return walkersRouter;
        }

        private void SetupFactory(Map2D map)
        {
            var objectFactories = new AbstractFactory[]
            {
                new StartPointFactory(map),
                new HeroFactory(map), 
                new PointerFactory(map, _data.MinDistanceBetweenPointers),
                new FloorFactory(),
            };
            
            _mapObjectsFactory = new MapObjectsFactory(objectFactories);
        }

        private void UpdateWalkers(Map2D map, MapWalkersRouter router)
        {
            foreach (var walker in router.Walkers)
            {
                TryCreateMapObject(map, walker.Position);
                walker.MoveRandom();
            }
            
            router.TryCreateWalker(_data.ChanceSpawnWalker, _data.WalkerData, _data.MaxWalkerCount);
            router.TryKillWalker(_data.ChanceKillWalker, _data.MinWalkerCount);
        }
        
        private void TryCreateMapObject(Map2D map, Vector2Int position)
        {
            if (map.Exist(position))
                return;

            _createdObjects++;
            var symbol = _mapObjectsFactory.Create(position);
            map.Apply(position, symbol);
        }
    }
}