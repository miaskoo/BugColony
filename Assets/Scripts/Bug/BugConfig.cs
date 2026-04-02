using System.Collections.Generic;
using UnityEngine;

namespace BugColony.Bugs
{
    public class BugConfig : ScriptableObject
    {
        [field: SerializeField] public int PointsToSplit { get; private set; } = 2;
        [field: SerializeField] public bool CanEatFood { get; private set; } = true;
        [field: SerializeField] public List<BugType> EdibleBugTypes { get; private set; } = new();
        [field: SerializeField] public Material BugMaterial { get; private set; }
        [field: SerializeField] public float InvulnerabilityTime { get; private set; } = 0f;
        [field: SerializeField] public Material InvulnerabilityMaterial { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
    }
}
