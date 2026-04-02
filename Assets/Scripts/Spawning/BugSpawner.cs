using System;
using System.Collections.Generic;
using System.Linq;
using BugColony.Bugs;
using BugColony.Infrastructure;
using UnityEngine;
using Zenject;

namespace BugColony.Spawning
{
    public class BugSpawner : SpawnerBase<BugObject>
    {
        [Serializable]
        public struct BugTypeConfig
        {
            public BugType Type;
            public BugConfig Config;
        }

        [SerializeField] private GameObject _bugPrefab;
        [SerializeField] private Transform _objectsLayer;
        [SerializeField] private List<BugTypeConfig> _bugConfigs = new();

        private const int InitialBugPoolSize = 30;
        private SignalBus _signalBus;

        private void OnValidate()
        {
            var existing = new HashSet<BugType>(_bugConfigs.Select(c => c.Type));
            foreach (BugType type in Enum.GetValues(typeof(BugType)))
            {
                if (!existing.Contains(type))
                    _bugConfigs.Add(new BugTypeConfig { Type = type });
            }
        }

        [Inject]
        private void Construct(DiContainer container, SignalBus signalBus)
        {
            _signalBus = signalBus;
            InitPool(_bugPrefab, _objectsLayer, InitialBugPoolSize, container);
        }

        public BugConfig GetConfig(BugType type)
        {
            return _bugConfigs.First(c => c.Type == type).Config;
        }

        public void SpawnOne(BugType type, Vector3 spawnPos)
        {
            var bug = _pool.Get();
            bug.ActivateLife(spawnPos);
            bug.StartLife(type);
        }

        public void KillBug(BugObject bug)
        {
            var killedType = bug.Type;
            Return(bug);
            _signalBus.Fire(new BugKilledSignal { Type = killedType });
        }
    }
}
