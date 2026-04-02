using BugColony.Bugs;
using DG.Tweening;
using UnityEngine;

namespace BugColony.Strategies
{
    public interface IBugStrategy
    {
        BugType SupportedType { get; }
        void OnBugActivated(BugObject bug);
        Tween CreateMoveAction(BugObject bug);
        void TryEatThis(BugObject bug, GameObject collision);
        bool TryMutate(BugObject bug);

        BugConfig GetConfig();
    }
}
