using UnityEngine;

namespace Environment
{
    [CreateAssetMenu(fileName = "Map Walker Data", menuName = "Isocubation/Environment/Map Walker Data", order = 1)]
    public class MapWalkerData : ScriptableObject
    {
        [SerializeField] private int _stepsForUpdate;
        [SerializeField] private Vector2Int[] _availableDirections;
        [SerializeField] private float _chanceChangeDirection;
        
        public int StepsForUpdate => _stepsForUpdate;
        public float ChanceChangeDirection => _chanceChangeDirection;

        public Vector2Int[] AvailableDirections => _availableDirections;

        public Vector2Int GetRandomDirection()
        {
            var index = Random.Range(0, _availableDirections.Length);
            return _availableDirections[index];
        }
    }
}