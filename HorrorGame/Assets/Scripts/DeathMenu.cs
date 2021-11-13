using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void RestartGame()
    {
        AudioManager.PlaySound("Hide1");
        StartCoroutine(Wait());
    }
    public void ExitGame()
    {
        AudioManager.StopSound("Music1");
        AudioManager.StopSound("Music2");
        SceneManager.LoadScene(sceneName: "Main Menu");
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName: "Demo Scene");
    }
}
