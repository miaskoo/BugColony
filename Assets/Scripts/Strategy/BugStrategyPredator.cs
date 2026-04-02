using System;
using BugColony.Bugs;
using BugColony.Environment;
using BugColony.Spawning;
using UniRx;

namespace BugColony.Strategies
{
    public class BugStrategyPredator : BugStrategyBase<PredatorBugConfig>
    {
        public override BugType SupportedType => BugType.Predator;

        public BugStrategyPredator(GroundController ground, BugSpawner spawn, FoodSpawner foodSpawner)
            : base(ground, spawn, foodSpawner, (PredatorBugConfig)spawn.GetConfig(BugType.Predator))
        {
        }

        public override void OnBugActivated(BugObject bug)
        {
            StartLifeTimer(bug);
        }

        public override bool TryMutate(BugObject bug)
        {
            if (bug.StomachPoints.Value < _config.PointsToSplit)
                return false;

            var result = TrySplit(bug, BugType.Predator);
            if (result)
                StartLifeTimer(bug);
            return result;
        }

        private void StartLifeTimer(BugObject bug)
        {
            bug.ApplyInvulnerability();
            var subscription = Observable.Timer(TimeSpan.FromSeconds(_config.LifeTimeSeconds))
                .Subscribe(_ => _spawn.KillBug(bug));
            bug.SetLifeTimer(subscription, _config.LifeTimeSeconds);
        }
    }
}
