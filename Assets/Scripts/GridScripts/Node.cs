using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    private void OnMouseDown()
    {
        TowerDefenseManager.m_CurrentGameObjectClicked = this.gameObject;        
    }
}
