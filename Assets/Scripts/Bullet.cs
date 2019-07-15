using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Bullet Setting
    [Header("Bullet Setting", order = 0)]
    [Space(5, order = 1)]
    [SerializeField] private float m_Damage;
    [SerializeField] private float m_BulletSpeed;
    private Rigidbody2D m_RB2D;
    #endregion

    private void Start()
    {
        m_RB2D = GetComponent<Rigidbody2D>();
        m_RB2D.velocity = transform.up * m_BulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(m_Damage);
            Destroy(gameObject);
        }
    }

}
