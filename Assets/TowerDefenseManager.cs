using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefenseManager : MonoBehaviour
{
    void Start()
    {
        // Show the player's city in this scene
        GameManager.HidePlayerCity(2, true);
    }
}
