using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerDefenseManager : MonoBehaviour
{
    public static TowerDefenseManager Instance;
    
    public List<GameObject> m_EnemiesAlive = new List<GameObject>();

    private void OnEnable()
    {
        Timer.OnTenSecondsReached += SpawnMinionWave;
        Timer.OnTenSecondsReached += ResetPlayerHand;
        Timer.OnZeroSecondsReached += EndLevel;
    }
    private void OnDisable()
    {
        Timer.OnTenSecondsReached -= SpawnMinionWave;
        Timer.OnTenSecondsReached -= ResetPlayerHand;
        Timer.OnZeroSecondsReached -= EndLevel;
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        // Show the player's city in this scene
        GameManager.HidePlayerCity(2, true);
        
        // Add the health text object on the hud if needed for this scene
        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
        Hud hud = playerManager.gameObject.GetComponent<Hud>();

        if (hud != null)
        {
            hud.m_HealthTextObject = GameObject.FindGameObjectWithTag("Health Bar").GetComponent<TMP_Text>();
            hud.m_TurretTextObject = GameObject.FindGameObjectWithTag("TurretTextObject").GetComponent<TMP_Text>();
        }
    }

    private void Start()
    {
        GridManager.Instance.SnapObjectsToGridEditorTime();
        GridManager.Instance.m_CurrentlyPlacedActors = 0; // Reset
    }

    public void SpawnMinionWave()
    {
        
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
