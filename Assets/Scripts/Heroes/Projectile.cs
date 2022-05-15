using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private GameObject m_TrackedTarget;
    public float m_Speed = 1.0f;
    private float m_ProjectileForce = 3;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (m_TrackedTarget != null)
        {
            // transform.Translate(m_TrackedTarget.transform.position - transform.position * m_Speed * Time.deltaTime);
        }

        if (transform.position.x > 100 || transform.position.y > 100 || transform.position.x < -100 || transform.position.y < -100)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Destroy(other.gameObject);
            if (gameObject != null)
            {
                Minion minion = gameObject.GetComponent<Minion>();
                Challenger challenger = gameObject.GetComponent<Challenger>();

                if (minion)
                {
                    minion.DestroyMe();
                } else if (challenger)
                {
                    challenger.DestroyMe();
                }   
            }
        }
    }

    public void Track(GameObject target)
    {
        m_TrackedTarget = target;
        if (target == null)
        {
            return;
        }
        Vector3 targetPos = m_TrackedTarget.transform.position;

        rb.AddForce((targetPos - transform.position).normalized * m_ProjectileForce, ForceMode.Impulse);
    }
}
