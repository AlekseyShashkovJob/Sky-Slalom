using UnityEngine;

namespace GameCore.Objects
{
    public class GameplayObjectSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private GameplayObjectPool[] _objectPools;
        [SerializeField] private float _spawnRate = 1.0f;
        [SerializeField] private float _boostedMultiplier = 0.25f;

        private float _timer;
        private float _currentSpawnRate;

        private void Awake() => _currentSpawnRate = _spawnRate;

        private void Start()
        {
            var manager = GameManager.Instance;
            if (manager != null)
                manager.OnBoostStateChanged += SetBoostedState;
        }

        private void OnDestroy()
        {
            var manager = GameManager.Instance;
            if (manager != null)
                manager.OnBoostStateChanged -= SetBoostedState;
        }

        private void Update()
        {
            if (_spawnPoints == null || _spawnPoints.Length == 0) return;
            if (_objectPools == null || _objectPools.Length == 0) return;

            _timer += Time.deltaTime;
            if (_timer >= _currentSpawnRate)
            {
                _timer = 0.0f;
                SpawnObject();
            }
        }

        private void SetBoostedState(bool isBoosted)
        {
            _currentSpawnRate = isBoosted ? _spawnRate * _boostedMultiplier : _spawnRate;
        }

        private void SpawnObject()
        {
            int track = UnityEngine.Random.Range(0, _spawnPoints.Length);
            int poolIndex = UnityEngine.Random.Range(0, _objectPools.Length);

            GameObject obj = _objectPools[poolIndex].GetObject();
            if (obj == null) return;

            obj.transform.SetParent(transform, false);
            obj.transform.position = _spawnPoints[track].position;

            if (obj.TryGetComponent<GameplayObjectMovement>(out var mover))
            {
                mover.Pool = _objectPools[poolIndex];
            }
        }
    }
}