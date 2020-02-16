using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Projectile Setting
    [Header("Projectile Setting")]
    [SerializeField] bool m_IsAP;
    [SerializeField] TrailRenderer m_Trail;
    [SerializeField] string m_TargetTag;
    [SerializeField] private float m_Damage;
    [SerializeField] private float m_BulletSpeed;
    private Rigidbody2D m_RB2D;
    #endregion

    private void Awake()
    {
        m_RB2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        m_RB2D.velocity = transform.up * m_BulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(m_TargetTag))
        {
            collision.GetComponent<Enemy>().TakeDamage(m_Damage);
            if (!m_IsAP)
                gameObject.SetActive(false);
        }
    }

    private void OnBecameInvisible()
    {
        if (m_Trail != null)
            m_Trail.Clear();
        gameObject.SetActive(false);
    }
}
