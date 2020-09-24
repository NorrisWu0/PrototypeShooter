using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeShooter
{
    public class Collectable : MonoBehaviour
    {
        public enum Collectable_Type
        {
            Medkit, Powerup
        }

        [SerializeField] Collectable_Type m_Type;
        [SerializeField] float m_Timer = 0;

        void OnEnable()
        {
            StartCoroutine(CR_Timer());
        }

        /// <summary>
        /// Disable this collectable after m_Timer
        /// </summary>
        IEnumerator CR_Timer()
        {
            yield return new WaitForSeconds(m_Timer);
            gameObject.SetActive(false);
        }

        void OnCollisionEnter2D(Collision2D _collision)
        {
            if (_collision.gameObject.tag == "Player")
            {
                if (m_Type == Collectable_Type.Medkit)
                    _collision.gameObject.GetComponent<Player>().RegenHealth();
                if (m_Type == Collectable_Type.Powerup)
                    _collision.gameObject.GetComponent<Player>().WeaponPowerup();

                gameObject.SetActive(false);
            }
        }
    }
}
