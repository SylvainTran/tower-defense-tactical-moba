using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PathObject m_PathObject;
    SphereCollider m_SphereCollider;

    private void Awake()
    {
        m_PathObject = GetComponent<PathObject>();
        m_SphereCollider = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            // Trigger event to make this encounter Slow down
            LevelScriptManager.Instance.m_CurrentEncounterAction = this;
            LevelScriptManager.Instance.StartNewCBTNode();
        }
    }

    public void SlowDown()
    {
        m_PathObject.m_TravelSpeed = 0.0f;
    }

    public void DisableCollider()
    {
        m_SphereCollider.enabled = false;
    }

    public void RestoreSpeed()
    {
        m_PathObject.m_TravelSpeed = 1.0f;
    }

    public void EnableCollider()
    {
        m_SphereCollider.enabled = true;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
