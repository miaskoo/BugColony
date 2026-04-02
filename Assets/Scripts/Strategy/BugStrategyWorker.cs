using BugColony.Bugs;
using BugColony.Environment;
using BugColony.Spawning;
using UnityEngine;

namespace BugColony.Strategies
{
    public class BugStrategyWorker : BugStrategyBase<WorkerBugConfig>
    {
        public override BugType SupportedType => BugType.Worker;

        public BugStrategyWorker(GroundController ground, BugSpawner spawn, FoodSpawner foodSpawner)
            : base(ground, spawn, foodSpawner, (WorkerBugConfig)spawn.GetConfig(BugType.Worker))
        {
        }

        public override void OnBugActivated(BugObject bug) { }

        public override bool TryMutate(BugObject bug)
        {
            if (bug.StomachPoints.Value < _config.PointsToSplit)
                return false;

            var spawnType = bug.Type;
            if (spawnType != BugType.Predator
                && _spawn.ActiveCount > _config.MinBugsForMutation
                && Random.Range(0, 100) < Mathf.Clamp(_config.MutationToPredatorChance, 0, 100))
            {
                spawnType = BugType.Predator;
            }

            return TrySplit(bug, spawnType);
        }
    }
}
