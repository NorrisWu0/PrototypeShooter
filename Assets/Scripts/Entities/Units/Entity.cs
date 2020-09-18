using UnityEngine;

namespace GeoShot
{
    public class Entity : MonoBehaviour
    {
        protected CameraController m_CameraController = null;

        protected virtual void Start()
        {
            m_CameraController = GameObject.FindGameObjectWithTag("MainRig").GetComponent<CameraController>();
        }
    }
}
