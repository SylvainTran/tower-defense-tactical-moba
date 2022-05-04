using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerDefenseManager : MonoBehaviour
{
    /// <summary>
    /// Used to validate turret placement on Raycast hitting a grid cell
    /// </summary>
    public static GameObject m_CurrentGameObjectClicked = null;

    private void OnEnable()
    {
        Timer.OnTenSecondsReached += ResetPlayerHand;
        Timer.OnZeroSecondsReached += EndLevel;
    }
    private void OnDisable()
    {
        Timer.OnTenSecondsReached -= ResetPlayerHand;
        Timer.OnZeroSecondsReached -= EndLevel;
    }

    void Awake()
    {
        // Show the player's city in this scene
        GameManager.HidePlayerCity(2, true);

        // Add the health text object on the hud if needed for this scene
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        Hud hud = playerManager.GetComponent<Hud>();

        if (hud.m_HealthTextObject == null)
        {
            hud.m_HealthTextObject = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<TMP_Text>();
            if (hud.m_HealthTextObject != null)
            {
                print("Health Bar added");
            } else
            {
                print("Health bar not found!");
            }
        }
    }

    public void ResetPlayerHand()
    {
        print("Resetting player's abilities hand!");
    }

    public void EndLevel()
    {
        print("Level ended.");
        MetaManager.GoToLounge();
    }
}
