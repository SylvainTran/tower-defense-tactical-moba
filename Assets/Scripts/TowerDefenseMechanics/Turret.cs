using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject TurretProjectilePrefab;
    public float m_TurretSpeed = 5.0f;
    public float m_TurretDistance = 3.0f;
    public float m_FireDelay = 0.1f;
    public GameObject m_CurrentTarget;

    private void Start()
    {
        print("New turret cell created!");
    }
    protected void OnMouseDown()
    {
        print("Turret info log UI activated!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_CurrentTarget = other.gameObject;
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(m_FireDelay);
        GameObject projectile = Instantiate(TurretProjectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Track(m_CurrentTarget);
    }
}
