using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // change this scene to actual game later
        SceneManager.LoadScene(sceneName: "Kevin's Scene");
    }
    public void ExitGame()
    {
        Debug.Log("You quit");
        Application.Quit();
    }
}
