using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SCENES
{
    MAIN_MENU = 0,
    LOUNGE = 1,
    TOWER_DEFENSE = 2
}

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button m_StartButton;
    [SerializeField] Button m_QuitButton;

    // Start is called before the first frame update
    void Start()
    {
        m_StartButton.onClick.AddListener(StartGame);
        m_QuitButton.onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        MetaManager.GoToLounge();
    }

    void QuitGame()
    {
        MetaManager.QuitGame();
    }
}
