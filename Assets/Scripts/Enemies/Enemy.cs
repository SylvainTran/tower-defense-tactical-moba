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

    public void SetSpeed(float speed)
    {
        m_PathObject.m_TravelSpeed = speed;
    }

    public void SetColliderState(bool state)
    {
        m_SphereCollider.enabled = state;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
