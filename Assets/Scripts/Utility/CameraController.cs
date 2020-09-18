using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform m_Reticle = null;
    [SerializeField] Transform m_Player = null;
    [SerializeField] Transform m_CameraDolly = null;

    [Header("MidPoint Setting")]
    public bool enableLerpToMidPoint = false;
    [SerializeField] float m_LerpSpeed;
    [SerializeField] Vector2 m_Boundary;

    private Vector3 m_MidPoint;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void FixedUpdate()
    {
        UpdateCameraPosition();
    }

    /// <summary>
    /// Lock camera rig to player and attach reticle to mouse position.
    /// </summary>
    private void UpdateCameraPosition()
    {
        // Find mouse position in world
        Vector3 _MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (m_Player != null)
            transform.position = m_Player.position;

        // Move reticle to mouse position
        if (m_Reticle != null)
            m_Reticle.position = new Vector3(_MousePosition.x, _MousePosition.y, 0);


        if (enableLerpToMidPoint)
        {
            // Find mid point between mouse and player position.
            m_MidPoint = (_MousePosition - m_Player.position) / 2;
            
            // Clamp mid point position in the boundary box.
            m_MidPoint = new Vector3(Mathf.Clamp(m_MidPoint.x, -m_Boundary.x, m_Boundary.x), Mathf.Clamp(m_MidPoint.y, -m_Boundary.y, m_Boundary.y), -10);

            // Lerp camera position to mid point
            m_CameraDolly.localPosition = Vector3.Lerp(m_CameraDolly.localPosition, m_MidPoint, m_LerpSpeed);
        }
        else
            m_CameraDolly.localPosition = Vector3.Lerp(m_CameraDolly.localPosition, new Vector3(0, 0, -10), m_LerpSpeed);
    }

    /// <summary>
    /// Change camera orthographic size
    /// </summary>
    public void StartCameraZoon(float _value)
    {
        StartCoroutine(CR_ZoomCamera(_value, Camera.main.orthographicSize));
    }

    private IEnumerator CR_ZoomCamera(float _value, float _original)
    {
        float _t = .7f;

        while (_t > 0)
        {
            Camera.main.orthographicSize = Mathf.SmoothStep(_value, _original, _t);

            _t -= Time.deltaTime;
            yield return null;
        }

        Camera.main.orthographicSize = _value;
    }

    /// <summary>
    /// Start the camera shake from other script
    /// </summary>
    public void StartCameraShake(float _duration, float _magnitute)
    {
        StartCoroutine(CR_ShakeCamera(_duration, _magnitute));
    }

    /// <summary>
    /// Shake camera based on the duration and the magnitute given.
    /// </summary>
    private IEnumerator CR_ShakeCamera(float _duration, float _magnitute)
    {
        float _t = _duration;

        while (_t > 0)
        {
            // Get random position inside circle
            Vector2 _randomPos = Random.insideUnitCircle * _magnitute * _t;

            // Move camara to random position
            Camera.main.transform.localPosition = new Vector3(_randomPos.x, _randomPos.y, -10);

            _t -= Time.deltaTime;
            yield return null;
        }

        // Reset camera position
        Camera.main.transform.localPosition = new Vector3(0, 0, -10);
    }

    /// <summary>
    /// Draw Gizmo for midPoint and midPoint boundary box.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // Draw _midPoint Position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_MidPoint, 1f);

        // Draw Camera Boundary Box
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(m_Player.position, m_Boundary * 2);
    }
}
