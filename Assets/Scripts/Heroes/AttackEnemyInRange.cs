using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackEnemyInRange : MonoBehaviour
{
    public GameObject TurretProjectilePrefab;
    public float m_TurretSpeed = 5.0f;
    public float m_TurretDistance = 3.0f;
    
    public float m_AttackDelay = 0.1f; // Change this depending on the hero
    public GameObject m_CurrentTarget;

    // When a hero is in the ready state to attack
    // It checks its list of triggered targets
    // And selects the highest priority target to attack
    public GameObject m_PlayerBase;
    public List<Enemy> m_TargetCandidates = new List<Enemy>();

    private void OnEnable()
    {
        Enemy.OnEnemyDied += RemoveDeadTarget;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= RemoveDeadTarget;
    }

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    // Go through list of all collider hits,
    // Priority:
    // Sort hits on collider by priorty (nearest to player > farthest)
    // Challenger > anything else
    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.IsDestroyed() && collider.gameObject != null && collider.CompareTag("Enemy"))
        {
            m_TargetCandidates.Add(collider.gameObject.GetComponent<Enemy>());
        }
    }

    // Listens to dead event
    public void RemoveDeadTarget(Enemy deadEnemy)
    {
        foreach (Enemy target in m_TargetCandidates)
        {
            if (deadEnemy.Equals(target))
            {
                m_TargetCandidates.Remove(target);
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(m_AttackDelay);

        GameObject priorityTarget = new GameObject();
        
        if (m_TargetCandidates.Count > 0)
        {
            // Sort by priority
            float nearestToPlayerBase = Single.MaxValue;
            
            foreach (Enemy target in m_TargetCandidates)
            {
                if (target.gameObject == null)
                {
                    m_TargetCandidates.Remove(target);
                    continue;
                }
                float dist = (target.transform.position - m_PlayerBase.transform.position).magnitude;
                if (dist < nearestToPlayerBase)
                {
                    nearestToPlayerBase = dist;
                    priorityTarget = target.gameObject;
                }
            }
        }
        
        m_CurrentTarget = priorityTarget;
        m_TargetCandidates.Remove(priorityTarget.GetComponent<Enemy>());
        
        // print("N : " + m_TargetCandidates.Count + "Current hero's target: " + m_CurrentTarget.name);
        
        StartCoroutine(Fire());
        
        // Reset AI
        // m_TargetCandidates.Clear();
        
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(m_AttackDelay);
        GameObject projectile = Instantiate(TurretProjectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Track(m_CurrentTarget);
    }
}
