using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaManager : MonoBehaviour
{
    public static int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public static void GoToMainMenu()
    {
        SceneManager.LoadScene((int)SCENES.MAIN_MENU);
    }

    public static void GoToLounge()
    {
        SceneManager.LoadScene((int)SCENES.LOUNGE);
    }

    public static void GoToTowerDefense()
    {
        SceneManager.LoadScene((int)SCENES.TOWER_DEFENSE);
    }

    public static void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
