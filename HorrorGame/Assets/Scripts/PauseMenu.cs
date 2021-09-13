using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] GameObject pauseMenu, soundMenu;
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1;
    }
    void PauseGame()
    {
        pauseMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0;
    }
    public void SoundMenu()
    {
        soundMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }
    public void MenuReturn()
    {
        soundMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
    public void ExitGame()
    {
        Debug.Log("You quit");
    }
}
