using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BugColony.Spawning
{
    public class ObjectPool<T> where T : class
    {
        private const int GrowBatchSize = 10;

        private readonly Stack<T> _free;
        private readonly HashSet<T> _active;
        private readonly Dictionary<GameObject, T> _objectMap;
        private readonly GameObject _prefab;
        private readonly Transform _parent;
        private readonly DiContainer _container;
        private readonly Func<GameObject, T> _extractor;
        private int _totalCreated;

        public int ActiveCount => _active.Count;
        public IReadOnlyCollection<T> ActiveItems => _active;

        public ObjectPool(GameObject prefab, Transform parent, int initialCount, DiContainer container, Func<GameObject, T> extractor)
        {
            _prefab = prefab;
            _parent = parent;
            _container = container;
            _extractor = extractor;
            _free = new Stack<T>(initialCount);
            _active = new HashSet<T>(initialCount);
            _objectMap = new Dictionary<GameObject, T>(initialCount);

            Grow(initialCount);
        }

        public T Get()
        {
            if (_free.Count == 0)
                Grow(GrowBatchSize);

            var item = _free.Pop();
            _active.Add(item);
            return item;
        }

        public void Return(T item)
        {
            if (!_active.Remove(item))
            {
                Debug.LogWarning($"ObjectPool: returning item that is not active");
                return;
            }

            _free.Push(item);
        }

        public bool IsActive(T item) => _active.Contains(item);

        public bool IsActiveObject(GameObject obj)
        {
            return _objectMap.TryGetValue(obj, out var item) && _active.Contains(item);
        }

        public bool TryGetActive(GameObject obj, out T component)
        {
            if (_objectMap.TryGetValue(obj, out component) && _active.Contains(component))
                return true;
            component = default;
            return false;
        }

        private void Grow(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = UnityEngine.Object.Instantiate(_prefab, _parent);
                obj.name = _totalCreated.ToString();
                obj.SetActive(false);
                _container.InjectGameObject(obj);

                var item = _extractor(obj);
                _objectMap[obj] = item;
                _free.Push(item);
                _totalCreated++;
            }
        }
    }
}
