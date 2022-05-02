using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void OnEnable()
    {
        PlayerManager.OnPlayerHealthIsZero += LevelGameOver;
    }
    private void OnDisable()
    {
        PlayerManager.OnPlayerHealthIsZero -= LevelGameOver;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void LevelGameOver()
    {
        Debug.Log("Level game over!");

        // When the level is over, we simply go to the Lounge and go over the player's results
        // and choices to promote education about CBT!
        MetaManager.GoToLounge();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(1400, 10, 150, 100), "Return to Main Menu"))
        {
            print("Returning to main menu level!");
            MetaManager.GoToMainMenu();
        }
        if (GUI.Button(new Rect(1400, 160, 150, 100), "Reset Level"))
        {
            print("Reset level!");
            MetaManager.ResetLevel();
        }
        if (GUI.Button(new Rect(1400, 310, 150, 100), "Simulate End Level\nAnd Return To Lounge"))
        {
            print("Simulated end of level score and returning to lounge...");
            MetaManager.GoToLounge();
        }
        if (GUI.Button(new Rect(1400, 460, 150, 100), "Quit Game"))
        {
            MetaManager.QuitGame();
        }
    }
}
