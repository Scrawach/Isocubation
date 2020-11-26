using System;
using System.Collections.Generic;
using Environment;
using Environment.Cells;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Flame))]
    public class Torch : CellObject
    {
        private World _world;
        private Flame _flame;
        private TorchRouter _router;
        private Vector2Int _offset;
        [SerializeField] private List<Cell> _cellsAround;
        [SerializeField] private int _capturedCount;

        public event Action Fired;

        public void Initialization(World world, Cell cell, Vector2Int offset, TorchRouter router)
        {
            _world = world;
            _router = router;
            Current = cell;
            _world.Updated += OnWorldUpdated;
            _offset = offset;
            _cellsAround = _world.GetAround(cell.Position - offset);
        }

        private void Awake()
        {
            _flame = GetComponent<Flame>();
        }

        private void OnDestroy()
        {
            _world.Updated -= OnWorldUpdated;
            
            foreach (var cell in _cellsAround)
            {
                cell.Captured -= OnCellCaptured;
            }
        }

        private void OnWorldUpdated(Vector2Int index)
        {
            if (IsAround(index) == false)
                return;
            
            _cellsAround.Add(_world.Get(index));

            if (_cellsAround.Count < 8) 
                return;
            
            _world.Updated -= OnWorldUpdated;
            foreach (var cell in _cellsAround)
            {
                cell.Captured += OnCellCaptured;
            }
        }

        private void OnCellCaptured(Cell capturedCell)
        {
            _capturedCount++;
            capturedCell.Captured -= OnCellCaptured;
            
            if (_capturedCount < _cellsAround.Count) 
                return;
            
            FireOn();
        }

        private bool IsAround(Vector2Int index)
        {
            var position = Current.Position - _offset;
            return position.x <= index.x + 1 && position.x >= index.x - 1 &&
                   position.y <= index.y + 1 && position.y >= index.y - 1;
        }

        private void FireOn()
        {
            Fired?.Invoke();
            
            var nearest = _router.SelectNearest(Current.Position);
            var direction = nearest - Current.Position;
            _router.TryRemove(Current.Position);
            _flame.SetTarget(direction);
            _flame.Enable();
        }
    }
}