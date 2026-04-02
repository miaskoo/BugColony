using BugColony.Core;
using UnityEngine;
using Zenject;

namespace BugColony.Spawning
{
    public abstract class SpawnerBase<T> : MonoBehaviour where T : SceneEntity
    {
        protected ObjectPool<T> _pool;

        public int ActiveCount => _pool.ActiveCount;
        public Collider SpawnCollider { get; private set; }

        protected void InitPool(GameObject prefab, Transform parent, int initialSize, DiContainer container)
        {
            _pool = new ObjectPool<T>(prefab, parent, initialSize, container, obj => obj.GetComponent<T>());
            SpawnCollider = prefab.GetComponent<T>().EntityCollider;
        }

        public virtual void Return(T entity)
        {
            entity.Deactivate();
            _pool.Return(entity);
        }

        public bool IsActive(T entity) => _pool.IsActive(entity);

        public bool IsActiveObject(GameObject obj) => _pool.IsActiveObject(obj);

        public bool TryGetActive(GameObject obj, out T entity) => _pool.TryGetActive(obj, out entity);
    }
}
