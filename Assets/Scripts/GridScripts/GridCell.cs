using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public static int ID;
    private int m_ID;
    private bool m_HasNode = false;

    private void Awake()
    {
        ID++;
        m_ID = ID;
    }

    private void OnMouseDown()
    {
        // TODO: Display cell texture / mesh renderer enabled instead of instantiating quads!

        Debug.Log("Clicked on grid cell ID: " + m_ID);
    }
}
