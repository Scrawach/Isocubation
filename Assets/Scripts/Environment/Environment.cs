using System;
using System.Collections;
using Environment.MapObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Environment
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] private RandomMapData _mapData;
        [SerializeField] private MapObjectsData _mapObjects;
        [SerializeField] private MapWalkerData _readerData;
        [SerializeField] private MapWalkersRouter _readerRouter;
        [SerializeField] private bool _random;
        [SerializeField] private bool _saveMap;
        [SerializeField] private string _loadPath = @"/home/user/Documents/map.txt";
        [SerializeField] private string _writePath = @"/home/user/Documents/map.txt";
        
        [SerializeField] private GameObject[,] _world;
        [SerializeField] private GameObject _floor;
        [SerializeField] private GameObject _pointer;
        [SerializeField] private GameObject _startPoint;
        
        private void Start()
        {
            var generator = new MapGenerator();
            var map = CreateMap(generator);
            if (_saveMap) generator.WriteTo(_writePath, map);
            //_world = SpawnWorld(map);
            _world = new GameObject[map.Width, map.Height];
            Debug.Log(map.StartPosition);
            //StartCoroutine(SpawnSimpleIteration(map));
            StartCoroutine(NotSoSimple(map));
            //CreateStepByStep(map);
        }

        private void Update()
        {

        }

        private void CreateWalkers(MapWalkersRouter router, MapWalkerData data, Vector2Int position, int count)
        {
            for (var i = 0; i < count; i++)
            {
                router.CreateWalker(data, position);
            }
        }
        
        private Map CreateMap(MapGenerator generator)
        {
            return _random ? generator.Random(_mapData, _mapObjects) : 
                generator.LoadFrom(_loadPath, _mapObjects);
        }

        private void CreateStepByStep(Map map)
        {
            _readerRouter = new MapWalkersRouter(map.Borders);
            CreateWalkers(_readerRouter, _readerData, _mapData.StartPosition, 4);
            var index = 0;
            foreach (var walker in _readerRouter.Walkers)
            {
                walker.Direction = _readerData.AvailableDirections[index];
                index++;
            }

            StartCoroutine(Create(map));
        }

        private IEnumerator Create(Map map)
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                foreach (var walker in _readerRouter.Walkers)
                {
                    if (_world != null && _world[walker.Position.x, walker.Position.y] == null)
                    {
                        SpawnObject(walker.Position, map.GetSymbolAt(walker.Position));
                    }
                    walker.MoveRandom();
                }
            }
        }

        private void SpawnObject(Vector2Int index, char symbol)
        {
            if (symbol == (char)MapObjectSymbol.Floor)
            {
                _world[index.x, index.y] = Instantiate(_floor, new Vector3(index.x, 0, index.y), Quaternion.identity, transform);
            }
            else if (symbol == (char) MapObjectSymbol.Pointer)
            {
                _world[index.x, index.y] = Instantiate(_pointer, new Vector3(index.x, 0, index.y), Quaternion.identity, transform);
            }
            else if (symbol == (char) MapObjectSymbol.Start)
            {
                _world[index.x, index.y] = Instantiate(_startPoint, new Vector3(index.x, 0, index.y), Quaternion.identity, transform);
            }
        }

        private GameObject[,] SpawnSimple(Map map)
        {
          var grid = new GameObject[map.Width, map.Height];

          for (var i = 0; i < map.Width; i++)
          {
            for (var j = 0; j < map.Height; j++)
            {
              var pos = new Vector2Int(i, j);
              grid[i, j] = SpawnConcrete(pos, map.GetSymbolAt(pos));
            }
          }

          return grid;
        }
        
        private IEnumerator SpawnSimpleIteration(Map map)
        {
          _world = new GameObject[map.Width, map.Height];

          for (var i = 0; i < map.Width; i++)
          {
            for (var j = 0; j < map.Height; j++)
            {
              var pos = new Vector2Int(i, j);
              _world[i, j] = SpawnConcrete(pos, map.GetSymbolAt(pos));
              yield return new WaitForSeconds(0.001f);
            }
          }
        }

        private IEnumerator NotSoSimple(Map map)
        {
          var startSpawnIndex = map.StartPosition;
          _world = new GameObject[map.Width, map.Height];

          var point = startSpawnIndex - Vector2Int.one;
          var size = Vector2Int.zero;
          while (true)
          {
            var borders = new RectInt(point, size);
            SquareFunction(map, borders);
            yield return new WaitForSeconds(0.2f);
            
            point -= Vector2Int.one;

            if (point.x < 0)
              size += Vector2Int.one;
            else
              size += 2 * Vector2Int.one;
            
            point.Clamp(Vector2Int.zero, new Vector2Int(map.Width - 1, map.Height - 1));
            size.Clamp(Vector2Int.zero, new Vector2Int(map.Width - 1, map.Height - 1));
          }
        }

        private void SquareFunction(Map map, RectInt borders)
        {
          for (var i = borders.xMin; i <= borders.xMax; i++)
          {
            Spawn(i, borders.yMin, map);
            Spawn(i, borders.yMax, map);
          }
          
          for (var i = borders.yMin; i <= borders.yMax; i++)
          {
            Spawn(borders.xMin, i, map);
            Spawn(borders.xMax, i, map);
          }
        }

        private void Spawn(int x, int y, Map map)
        {
          if (_world[x, y] != null)
            return;
          
          var pos = new Vector2Int(x, y);
          _world[x, y] = SpawnConcrete(pos, map.GetSymbolAt(pos));
        }

        private GameObject SpawnConcrete(Vector2Int position, char symbol)
        {
          var worldPosition = new Vector3(position.x, 0f, position.y);
          
          if (symbol == (char)MapObjectSymbol.Floor)
          {
            return Instantiate(_floor, worldPosition, Quaternion.identity, transform);
          }
          
          if (symbol == (char) MapObjectSymbol.Pointer)
          {
            return Instantiate(_pointer, worldPosition, Quaternion.identity, transform);
          }
          
          if (symbol == (char) MapObjectSymbol.Start)
          {
            return Instantiate(_startPoint, worldPosition, Quaternion.identity, transform);
          }

          return null;
        }
        
        private GameObject[,] SpawnWorld(Map map)
        {
            var grid = new GameObject[map.Width, map.Height];

            for (var i = 0; i < map.Width; i++)
            {
                for (var j = 0; j < map.Height; j++)
                {
                    var symbol = map.GetSymbolAt(new Vector2Int(i, j));

                    if (symbol == (char)MapObjectSymbol.Floor)
                    {
                        grid[i, j] = Instantiate(_floor, new Vector3(i, 0, j), Quaternion.identity, transform);
                    }
                    else if (symbol == (char) MapObjectSymbol.Pointer)
                    {
                        grid[i, j] = Instantiate(_pointer, new Vector3(i, 0, j), Quaternion.identity, transform);
                    }
                    else if (symbol == (char) MapObjectSymbol.Start)
                    {
                        grid[i, j] = Instantiate(_startPoint, new Vector3(i, 0, j), Quaternion.identity, transform);
                    }
                }
            }

            return grid;
        }
    }
}