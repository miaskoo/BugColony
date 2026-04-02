using BugColony.Bugs;
using BugColony.Environment;
using BugColony.Spawning;
using DG.Tweening;
using UnityEngine;

namespace BugColony.Strategies
{
    public abstract class BugStrategyBase<TConfig> : IBugStrategy where TConfig : BugConfig
    {
        protected readonly GroundController _ground;
        protected readonly BugSpawner _spawn;
        protected readonly FoodSpawner _foodSpawner;
        protected readonly TConfig _config;

        protected BugStrategyBase(GroundController ground, BugSpawner spawn, FoodSpawner foodSpawner,
            TConfig config)
        {
            _ground = ground;
            _spawn = spawn;
            _foodSpawner = foodSpawner;
            _config = config;
        }

        public abstract BugType SupportedType { get; }
        public abstract void OnBugActivated(BugObject bug);
        public abstract bool TryMutate(BugObject bug);

        public BugConfig GetConfig()
        {
            return _config;
        }

        public void TryEatThis(BugObject bug, GameObject target)
        {
            if (_config.CanEatFood && _foodSpawner.IsActiveFood(target))
            {
                _foodSpawner.ReturnFood(target);
                bug.IncrementPoint();
                return;
            }

            if (_spawn.TryGetActive(target, out var targetBug)
                && !targetBug.IsInvulnerable
                && _config.EdibleBugTypes.Contains(targetBug.Type))
            {
                _spawn.KillBug(targetBug);
                bug.IncrementPoint();
                return;
            }
        }

        public virtual Tween CreateMoveAction(BugObject bug)
        {
            Vector3 endPos = _ground.GetFreePosOnGround();
            var action = bug.transform.DOMove(endPos, Vector3.Distance(bug.transform.position, endPos) / _config.MoveSpeed);
            action.SetEase(Ease.Linear);
            action.OnComplete(() => bug.GoToNextPoint());
            return action;
        }

        protected bool TrySplit(BugObject bug, BugType spawnType)
        {
            var curPos = bug.transform.position;
            bug.ReplaceMoveAction(CreateMoveAction(bug));

            _spawn.SpawnOne(spawnType, curPos);
            return true;
        }
    }
}
