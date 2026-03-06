using UnityEngine;
using System.Collections.Generic;

namespace GameCore.Objects
{
    public class GameplayObjectPool : MonoBehaviour
    {
        [SerializeField] private int _poolSize = 5;
        [SerializeField] private GameObject _objectPrefab;

        private Queue<GameObject> _objectPool;

        private void Awake()
        {
            _objectPool = new Queue<GameObject>();

            for (int i = 0; i < _poolSize; ++i)
            {
                GameObject obj = Instantiate(_objectPrefab);
                obj.SetActive(false);
                _objectPool.Enqueue(obj);
            }
        }

        public GameObject GetObject()
        {
            while (_objectPool.Count > 0)
            {
                var obj = _objectPool.Dequeue();
                if (obj != null)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }

            GameObject newObj = Instantiate(_objectPrefab);
            newObj.SetActive(true);
            return newObj;
        }

        public void ReturnObject(GameObject obj)
        {
            if (obj == null || _objectPool.Contains(obj))
                return;

            if (_objectPool.Count >= _poolSize)
                return;

            obj.SetActive(false);
            _objectPool.Enqueue(obj);
        }
    }
}