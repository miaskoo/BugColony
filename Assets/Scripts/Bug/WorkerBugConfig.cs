using UnityEngine;

namespace BugColony.Bugs
{
    [CreateAssetMenu(fileName = "WorkerBugConfig", menuName = "BugColony/WorkerBugConfig")]
    public class WorkerBugConfig : BugConfig
    {
        [field: SerializeField] public int MutationToPredatorChance { get; private set; } = 10;
        [field: SerializeField] public int MinBugsForMutation { get; private set; } = 10;
    }
}
