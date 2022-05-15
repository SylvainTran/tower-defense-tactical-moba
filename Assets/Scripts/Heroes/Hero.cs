using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private AttackEnemyInRange m_AttackEnemyInRange;
    // Start is called before the first frame update
    void Start()
    {
        m_AttackEnemyInRange = GetComponentInChildren<AttackEnemyInRange>();
        // StartCoroutine(ResetTargetAI());
    }

    public IEnumerator ResetTargetAI()
    {
        yield return new WaitForSeconds(3.0f);
        // Reset AI
        m_AttackEnemyInRange.m_TargetCandidates.Clear();
        StartCoroutine(ResetTargetAI());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
