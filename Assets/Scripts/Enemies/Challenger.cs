using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Challenger : Enemy
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            // Trigger event to make this encounter Slow down
            LevelScriptManager.Instance.m_CurrentEncounterAction = this;
            LevelScriptManager.Instance.StartNewCBTNode();
        }
    }
}
