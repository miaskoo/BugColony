using BugColony.Environment;
using UnityEngine;
using Zenject;

namespace BugColony.Spawning
{
    public class FoodSpawner : SpawnerBase<FoodObject>
    {
        [SerializeField] private GameObject _foodPrefab;
        [SerializeField] private Transform _objectsLayer;

        private const int InitialPoolSize = 10;

        [Inject]
        private void Construct(DiContainer container)
        {
            InitPool(_foodPrefab, _objectsLayer, InitialPoolSize, container);
        }

        public void SpawnOne(Vector3 posValue)
        {
            var food = _pool.Get();
            food.Activate(posValue);
        }

        public void ReturnFood(GameObject food)
        {
            if (_pool.TryGetActive(food, out var entity))
                Return(entity);
        }

        public bool IsActiveFood(GameObject obj)
        {
            return IsActiveObject(obj);
        }
    }
}
