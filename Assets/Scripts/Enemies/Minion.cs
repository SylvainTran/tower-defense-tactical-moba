using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Enemy
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}
