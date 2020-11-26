using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class Environment : MonoBehaviour
    {
        [Header("Данные для случайной генерации карты.")]
        [SerializeField] private RandomMapData _mapData;
        [SerializeField] private MapObjectDecoderData _mapObjects;
        [SerializeField] private int _seed;

        [Header("Путь для записи / чтения карты.")]
        [SerializeField] private bool _write;
        [SerializeField] private string _writePath = @"/home/user/Documents/map.txt";
        [SerializeField] private bool _load;
        [SerializeField] private string _loadPath = @"/home/user/Documents/map.txt";
        [SerializeField] private string _loadPath2 = @"/home/user/Documents/map.txt";
        
        [SerializeField] private List<string> _loadPaths;
        private int _currentMap;
        
        [Header("Пошаговое создание уровня.")] 
        [SerializeField] private bool _stepByStep;
        [SerializeField] private float _pauseBetweenSpawns;

        private WorldCreator _creator;
        
        public World World { get; private set; }

        private SerialPicker _mapSerialPickerCreator;
        private Vector2Int _offset;

        private void Start()
        {
            var maps = new List<Map2D>(_loadPaths.Count);
            foreach (var path in _loadPaths)
            {
                var map = CreateMap(path);
                maps.Add(map);
            }
            
            _mapSerialPickerCreator = new SerialPicker(maps);
            World = CreateWorld(Vector2Int.zero);
        }

        private World CreateWorld(Vector2Int offset)
        {
            _creator = new WorldCreator(this, _mapObjects);
            var map = _mapSerialPickerCreator.Next();
            var world = _creator.Create(map, offset);
            _creator.TorchesDone += OnTorchDone;
            return world;
        }
        
        private void OnTorchDone()
        {
            _creator.EventDeregister();

            if (_mapSerialPickerCreator.CanTakeNext() == false)
            {
                return;
            }

            var world = CreateWorld(new Vector2Int(World.Width, 0));
            World.Extend(world);
        }

        private Map2D CreateMap(string loadPath)
        {
            var generator = new MapGenerator();
            var map = _load ? generator.LoadFrom(loadPath) : generator.Random(_mapData, _seed);
            
            if (_write) generator.WriteTo(_writePath, map);
            return map;
        }
        
        private World SimpleCreate(Map2D map, Vector2Int offset)
        {
            var worldCreator = new WorldCreator(this, _mapObjects);
            worldCreator.Create(map, offset);
            return worldCreator.World;
        }

        private World CreateStepByStep(Map2D map)
        {
            var worldIteration = new WorldIterator(this, _mapObjects, _pauseBetweenSpawns);
            StartCoroutine(worldIteration.Create(map));
            return worldIteration.World;
        }
        
        public GameObject Spawn(GameObject prefab, Vector3 position, Transform parent = null)
        {
            if (parent == null)
                parent = transform;
            
            return Instantiate(prefab, position, Quaternion.identity, parent);
        }
    }
}