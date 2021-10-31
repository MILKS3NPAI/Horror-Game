using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void RestartGame()
    {
        // change this scene to actual game later
        SceneManager.LoadScene(sceneName: "Kevin's Scene");
    }
    public void ExitGame()
    {
        AudioManager.StopSound("Music1");
        AudioManager.StopSound("Music2");
        SceneManager.LoadScene(sceneName: "Main Menu");
    }
}
