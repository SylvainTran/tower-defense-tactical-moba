using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEquatable<Enemy>
{
    public static int EnemyID = 0;
    public int m_EnemyID;
    PathObject m_PathObject;
    SphereCollider m_SphereCollider;

    public delegate void EnemyDied(Enemy enemy);

    public static event EnemyDied OnEnemyDied;

    private void Awake()
    {
        m_PathObject = GetComponent<PathObject>();
        m_SphereCollider = GetComponent<SphereCollider>();
        
        // TODO: To Test
        ++EnemyID;
        m_EnemyID = EnemyID;
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
        OnEnemyDied(this); // Notifies heroes in range to ignore
        Destroy(gameObject);
    }
    
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Enemy objAsEnemy = obj as Enemy;
        if (objAsEnemy == null) return false;
        else return Equals(objAsEnemy);
    }
    
    public override int GetHashCode()
    {
        return m_EnemyID;
    }
    
    public bool Equals(Enemy other)
    {
        if (other == null) return false;
        return (this.m_EnemyID.Equals(other.m_EnemyID));
    }
}
