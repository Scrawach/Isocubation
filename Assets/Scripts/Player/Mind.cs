using System;
using UnityEngine;

namespace Player
{
    public class Mind : MonoBehaviour
    {
        public event Action<Vector2Int> Moved;
        public event Action Jumped;
        
        protected void MoveLeft() => Moved?.Invoke(Vector2Int.left);
        protected void MoveRight() => Moved?.Invoke(Vector2Int.right);
        protected void MoveUp() => Moved?.Invoke(Vector2Int.up);
        protected void MoveDown() => Moved?.Invoke(Vector2Int.down);
        protected void Jump() => Jumped?.Invoke();
    }
}