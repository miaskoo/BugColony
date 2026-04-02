using BugColony.Bugs;
using BugColony.Environment;
using BugColony.Spawning;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace BugColony.Infrastructure
{
    public class ColonyController : MonoBehaviour
    {
        [Header("Colony Settings")]
        [SerializeField] private float _foodSpawnInterval = 10f;
        [SerializeField] private int _maxFoodOnScene = 10;

        private BugSpawner _bugSpawner;
        private FoodSpawner _foodSpawner;
        private GroundController _ground;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(BugSpawner bugSpawner, FoodSpawner foodSpawner, GroundController ground, SignalBus signalBus)
        {
            _bugSpawner = bugSpawner;
            _foodSpawner = foodSpawner;
            _ground = ground;
            _signalBus = signalBus;

            _signalBus.GetStream<BugKilledSignal>()
                .Subscribe(OnBugKilled)
                .AddTo(this);

            Observable.Interval(TimeSpan.FromSeconds(_foodSpawnInterval))
                .Where(_ => _foodSpawner.ActiveCount < _maxFoodOnScene)
                .Subscribe(_ => SpawnFood())
                .AddTo(this);
        }

        private void Start()
        {
            SpawnWorker();
            SpawnFood();
        }

        private void OnBugKilled(BugKilledSignal signal)
        {
            if (_bugSpawner.ActiveCount == 0)
                SpawnWorker();
        }

        private void SpawnWorker()
        {
            _bugSpawner.SpawnOne(BugType.Worker, _ground.GetFreePosOnGround(_bugSpawner.SpawnCollider));
        }

        private void SpawnFood()
        {
            _foodSpawner.SpawnOne(_ground.GetFreePosOnGround(_foodSpawner.SpawnCollider));
        }
    }
}
