using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class TorchRouter
    {
        private readonly List<Vector2Int> _torches;
        
        public TorchRouter()
        {
            _torches = new List<Vector2Int>();
        }

        public event Action Done;

        public Vector2Int SelectNearest(Vector2Int position)
        {
            var min = float.MaxValue;
            var point = Vector2Int.zero;

            foreach (var torchPosition in _torches)
            {
                if (torchPosition == position)
                    continue;

                var magnitude = (torchPosition - position).magnitude;

                if (min > magnitude)
                {
                    min = magnitude;
                    point = torchPosition;
                }
            }

            return point;
        }

        public void Add(Vector2Int position)
        {
            _torches.Add(position);
        }

        public bool TryRemove(Vector2Int position)
        {
            var trying = _torches.Remove(position);
            
            if (_torches.Count == 0)
                Done?.Invoke();

            return trying;
        }
    }
}