using UnityEngine;

namespace BugColony.Environment
{
    public class GroundController : MonoBehaviour
    {
        [SerializeField] private BoxCollider _groundZone;
        [SerializeField] private float _groundHeight = 1;
        [SerializeField] private LayerMask _obstacleMask;

        public Vector3 GetFreePosOnGround(Collider prefabCollider = null)
        {
            Bounds bounds = _groundZone.bounds;

            for (int i = 0; i < 30; i++)
            {
                float x = Random.Range(bounds.min.x, bounds.max.x);
                float z = Random.Range(bounds.min.z, bounds.max.z);

                Vector3 point = new Vector3(x, _groundHeight, z);

                if (prefabCollider != null)
                {
                    bool isBusy = false;

                    switch (prefabCollider)
                    {
                        case BoxCollider box:
                            Vector3 center = point + box.center;
                            Vector3 halfExtents = box.size * 0.5f;
                            isBusy = Physics.CheckBox(center, halfExtents, Quaternion.identity, _obstacleMask);
                            break;

                        case SphereCollider sphere:
                            Vector3 centerS = point + sphere.center;
                            isBusy = Physics.CheckSphere(centerS, sphere.radius, _obstacleMask);
                            break;

                        default:
                            Debug.LogWarning($"Unsupported collider type: {prefabCollider.GetType()}");
                            break;
                    }

                    if (!isBusy)
                        return point;
                }
                else
                {
                    return point;
                }
            }

            Debug.LogWarning("Can't find a free position on the ground!");
            return Vector3.zero;
        }
    }
}