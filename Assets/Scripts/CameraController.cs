using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Camera Setting")]
    [SerializeField] private Transform m_Target;
    [SerializeField] private bool m_EnableLerpToMidPoint = false;
    [SerializeField] private float m_LerpSpeed;
    private Vector3 m_MidPoint;
    [SerializeField] private Vector3 m_Boundary;

    void FixedUpdate()
    {
        if (m_EnableLerpToMidPoint)
            LerpToMidPoint();
    }


    #region Camera Movement Function
    private void LerpToMidPoint()
    {
        if (m_Target != null)
        {
            Vector3 _MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_MidPoint = m_Target.position + (_MousePosition - m_Target.position) / 2;

            m_MidPoint = new Vector3(Mathf.Clamp(m_MidPoint.x, m_Target.position.x - m_Boundary.x, m_Target.position.x + m_Boundary.x),
                                    Mathf.Clamp(m_MidPoint.y, m_Target.position.y - m_Boundary.y, m_Target.position.y + m_Boundary.y),
                                    -100);

            transform.position = Vector3.Lerp(transform.position, m_MidPoint, m_LerpSpeed);
        }
    }
    #endregion

    #region Draw Debug Gizmos
    private void OnDrawGizmosSelected()
    {
        // Draw _midPoint Position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_MidPoint, 1f);

        // Draw Camera Boundary Box
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(m_Target.position, m_Boundary * 2);


    }

    #endregion
}
