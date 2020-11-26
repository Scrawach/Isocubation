using System;
using System.Collections;
using Gameplay;
using Player;
using UnityEngine;

namespace Environment.Cells
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Drawer _drawer;

        public bool IsCaptured { get; private set; }
        public bool IsShaking { get; private set; }
        public Vector2Int Position { get; private set; }
        public Status Status { get; private set; }

        public event Action<Cell> Captured;

        public void Initialization(Vector2Int position, Status status)
        {
            Status = status;
            Position = position;
            StartMove();
        }

        public void DrawTo(Material material)
        {
            _drawer.DrawTo(material);
        }
        
        public void Capture(Actor actor)
        {
            Captured?.Invoke(this);
            IsCaptured = true;
            StartShake(.1f, actor.Speed * 2);
        }

        private void StartShake(float change, float speed)
        {
            if (IsShaking)
                return;

            IsShaking = true;
            StartCoroutine(Shaking(change, speed));
        }
        
        private IEnumerator Shaking(float change, float speed)
        {
            var start = transform.position;
            var result = start + change * Vector3.down;

            yield return StartCoroutine(Functions.SmoothChange(start, result, speed, PositionChanging));
            yield return StartCoroutine(Functions.SmoothChange(result, start, speed, PositionChanging));
            IsShaking = false;
        }

        private void StartMove()
        {
            if (IsShaking)
                return;

            IsShaking = true;
            StartCoroutine(Moving(5, 2));
        }
        
        private IEnumerator Moving(float change, float speed)
        {
            var start = transform.position;
            var result = start + change * Vector3.up;

            yield return StartCoroutine(Functions.SmoothChange(start, result, speed, PositionChanging));
            IsShaking = false;
        }
        
        private void PositionChanging(Vector3 start, Vector3 end, float t)
        {
            transform.position = Vector3.Lerp(start, end, t);
        }
    }
}