using System;
using System.Collections.Generic;
using Environment.Cells;
using Environment.MapObjects;
using Gameplay;
using Player;
using UnityEngine;

namespace Environment
{
    public abstract class WorldCreatorBase
    {
        private readonly Environment _environment;
        private readonly MapObjectsDecoder _decoder;
        private readonly TorchRouter _torchRouter;
        
        protected WorldCreatorBase(Environment environment, MapObjectDecoderData decoderData)
        {
            _environment = environment;
            _decoder = new MapObjectsDecoder(decoderData);
            _torchRouter = new TorchRouter();
            _torchRouter.Done += OnTorchDone;
        }
        
        public event Action TorchesDone; 
        
        public World World { get; protected set; }

        public void EventDeregister()
        {
            _torchRouter.Done -= OnTorchDone;
        }
        
        protected bool TryCreateObject(Vector2Int position, Vector2Int offset, MapObjectSymbol id)
        {
            if (_decoder.TryGetObject(id, out var prefab) == false)
                return false;
            
            CreateObject(prefab, position, offset, id);
            return true;
        }

        private void CreateObject(GameObject prefab, Vector2Int position, Vector2Int offset, MapObjectSymbol id)
        {
            var worldPosition = new Vector3(position.x + offset.x, -5f, position.y + offset.y);
            var cell = _environment.Spawn(prefab, worldPosition).GetComponent<Cell>();
            var initCellPosition = position + offset;
            World.Apply(position, cell);
            
            switch (id)
            {
                case MapObjectSymbol.Hero:
                    var hero = cell.GetComponentInChildren<Actor>();
                    cell.Initialization(initCellPosition, Status.Free);
                    hero.Initialization(World, cell);
                    break;
                case MapObjectSymbol.Pointer:
                    var pointer = cell.GetComponentInChildren<Torch>();
                    cell.Initialization(initCellPosition, Status.Closed);
                    pointer.Initialization(World, cell, offset, _torchRouter);
                    _torchRouter.Add(initCellPosition);
                    break;
                case MapObjectSymbol.Dead:
                    cell.Initialization(initCellPosition, Status.Busy);
                    break;
                case MapObjectSymbol.BreakFloor:
                    cell.Initialization(initCellPosition, Status.Break);
                    break;
                default:
                    cell.Initialization(initCellPosition, Status.Free);
                    break;
            }
        }

        private void OnTorchDone()
        {
            TorchesDone?.Invoke();
        }
        
    }
}