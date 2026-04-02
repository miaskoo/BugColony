using BugColony.Core;
using UnityEngine;

namespace BugColony.Environment
{
    public class FoodObject : SceneEntity
    {
        [SerializeField] private SphereCollider _col;

        public override Collider EntityCollider => _col;
    }
}
