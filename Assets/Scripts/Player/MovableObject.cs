using System.Collections;
using Environment.Cells;
using UnityEngine;

namespace Player
{
    public class MovableObject : CellObject
    {
        [SerializeField] private Transform _body;
        
        private float _tempSpeed = 5f;
        private float _tempHeight = 1f;
        
        public bool IsMoving { get; private set; }
        public float Speed => _tempSpeed;

        protected virtual void AfterMoving(Cell target)
        {
            Current = target;
            transform.parent = target.transform;
        }

        protected void MoveTo(Cell target)
        {
            StartMove(target, _tempSpeed);
        }

        private void StartMove(Cell target, float speed)
        {
            if (IsMoving)
                return;

            IsMoving = true;
            transform.parent = null;
            StartCoroutine(Moving(target, speed));
        }

        private IEnumerator Moving(Cell target, float speed)
        {
            var start = transform.position;
            var end = target.transform.position;

            var startRot = _body.localRotation;
            var endRot = Functions.GetRotationAfterFlip(startRot, end - start);
            
            StartCoroutine(Functions.SmoothChange(startRot, endRot, speed, RotationChanging));
            yield return StartCoroutine(Functions.SmoothChange(start, end, speed, PositionChanging));
            IsMoving = false;
            
            transform.parent = target.transform;
            transform.position = end;
            AfterMoving(target);
        }
        
        private void PositionChanging(Vector3 start, Vector3 end, float t)
        {
            var startPosition = start;
            var endPosition = end;
            
            var x = Mathf.Lerp(startPosition.x, endPosition.x, t);
            var y = Functions.Bezier(startPosition.y, _tempHeight, endPosition.y, t);
            var z = Mathf.Lerp(startPosition.z, endPosition.z, t);
            transform.position = new Vector3(x, y, z);
        }

        private void RotationChanging(Quaternion start, Quaternion end, float t)
        {
            _body.rotation = Quaternion.Slerp(start, end, t);
        }
    }
}