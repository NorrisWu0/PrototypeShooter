using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPointer : MonoBehaviour
{
    [SerializeField] Transform m_Destination;
    [SerializeField] RectTransform m_PointerRectTransform;

    void Awake()
    {
        m_PointerRectTransform = transform.Find("WaypointPointer").GetComponent<RectTransform>();
    }

    void Update()
    {
        
    }
}
