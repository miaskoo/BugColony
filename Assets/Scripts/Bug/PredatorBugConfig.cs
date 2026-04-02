using UnityEngine;

namespace BugColony.Bugs
{
    [CreateAssetMenu(fileName = "PredatorBugConfig", menuName = "BugColony/PredatorBugConfig")]
    public class PredatorBugConfig : BugConfig
    {
        [field: SerializeField] public int LifeTimeSeconds { get; private set; } = 10;
    }
}
