using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public string m_ActorName;
    public static int TurretCount = 0;

    private void Awake()
    {
        TurretCount++;
    }

    private void Start()
    {
        m_ActorName = "Hero Character " + TurretCount;
    }
}
