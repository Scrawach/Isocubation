using UnityEngine;

namespace Gameplay
{
    public class Flame : MonoBehaviour
    {
        [SerializeField] private GameObject _pointer;

        public void SetTarget(Vector2Int direction)
        {
            var directionVector3 = new Vector3(direction.x, 0f, direction.y);
            var resultDirection = directionVector3.normalized;
            _pointer.transform.localRotation = Quaternion.LookRotation(resultDirection);
        }
        
        public void Enable()
        {
            _pointer.SetActive(true);
        }

        public void Disable()
        {
            _pointer.SetActive(false);
        }
    }
}