using UnityEngine;

namespace BugColony.Core
{
    public abstract class SceneEntity : MonoBehaviour
    {
        public abstract Collider EntityCollider { get; }

        public virtual void Activate(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
