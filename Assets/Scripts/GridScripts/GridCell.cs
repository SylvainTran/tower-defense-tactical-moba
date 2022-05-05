using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public static int ID;
    public int m_ID;
    public bool m_HasNode = false;     // Has a turret or something else placed in it

    private void Awake()
    {
        ID++;
        m_ID = ID;
    }

    protected void OnMouseDown()
    {
        // TODO: Display cell texture / mesh renderer enabled instead of instantiating quads!
        // Debug.Log("Clicked on grid cell ID: " + m_ID);
    }
}
