using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoungeManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.HidePlayerCity(1, false);
        // Retrieve results of last TowerDefense level (Yarn InMemory storage vars)
        // Review them with player... node by node
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 300, 100), "Reset Dialogue State To Origin"))
        {
            print("Resetting dialogue state...");
        }
        if (GUI.Button(new Rect(10, 160, 150, 100), "Go To Tower Defense"))
        {
            print("Next Tower Defense level loading...");
            MetaManager.GoToTowerDefense();
        }
    }
}
