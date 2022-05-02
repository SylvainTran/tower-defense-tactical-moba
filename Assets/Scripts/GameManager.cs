using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
