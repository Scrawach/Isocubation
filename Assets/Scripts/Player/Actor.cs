using System;
using System.Text.RegularExpressions;
using Environment;
using Environment.Cells;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Mind))]
    public class Actor : MovableObject
    {
        [SerializeField] private Material _material;
        [SerializeField] private int _jumpStrength = 2;
        
        private World _world;
        private Mind _input;
        private Vector2Int _lastDirection;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _input.Moved += OnMove;
            _input.Jumped += OnJumped;
        }

        private void OnDisable()
        {
            _input.Moved -= OnMove;
            _input.Jumped -= OnJumped;
        }

        public void Initialization(World world, Cell cell)
        {
            _world = world;
            Current = cell;
        }
        
        protected override void AfterMoving(Cell target)
        {
            base.AfterMoving(target);
            target.DrawTo(_material);
            target.Capture(this);
        }

        private void OnJumped()
        {
            var position = GetPositionAfterJump(Current.Position, _lastDirection, _jumpStrength);
            MoveTo(_world.Get(position));
        }
        
        private void OnMove(Vector2Int direction)
        {
            if (IsMoving)
                return;

            _lastDirection = direction;
            var currentPosition = Current.Position;
            var targetPosition = currentPosition + direction;
            
            if (_world.TryGet(targetPosition, out var targetCell))
            {
                if (CanMoveTo(targetCell))
                {
                    MoveTo(targetCell);
                    return;
                }
            }

            MoveTo(Current);
        }

        private bool CanMoveTo(Cell cell)
        {
            return cell.Status == Status.Free;
        }

        private Vector2Int GetPositionAfterJump(Vector2Int startPosition, Vector2Int jumpDirection, int jumpStrength)
        {
            var position = startPosition;

            for (var i = 0; i < (jumpStrength + 1); i++)
            {
                var nextPosition = startPosition + jumpDirection * i;

                if (!_world.Exist(nextPosition)) 
                    continue;
                
                var cell = _world.Get(nextPosition);

                if (CanMoveTo(cell))
                {
                    position = nextPosition;
                }

                if (cell.Status == Status.Closed)
                {
                    break;
                }
            }

            return position;
        }
    }
}