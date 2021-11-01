using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public static float volume;
    public static bool notFirstLoad;
    [SerializeField] Camera camera;
    [SerializeField] GameObject pauseMenu, soundMenu;
    [SerializeField] Slider slider;
    //[SerializeField] Vector3 positionOffset;
    private GameObject player;
    PlayerControls playerControls;

    //Initialize the PlayerControls Map to use 
    protected void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
        playerControls = new PlayerControls();
    }
    //This just detects when Pause is pressed in the map(PlayerControls.UI) (Currently:P)
    private void Start()
    {
        if (!notFirstLoad)
        {
            notFirstLoad = true;
            volume = 0.5f;
        }
        slider.value = volume;
        // += _ means blank, some inputs can return a value, you would do   += var => Function(var);
        playerControls.UI.Pause.performed += _ => Pause();
    }
    //OnEnable and disbale are required in any script using an InputMap
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        //Don't need to check in update because the PlayerControls map auto does that
        volume = slider.value;
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
        pauseMenu.transform.position = player.transform.position + new Vector3(0, 1.68f, 0);
    }
    public void SoundMenu()
    {
        soundMenu.SetActive(true);
        pauseMenu.SetActive(false);
        soundMenu.transform.position = player.transform.position + new Vector3(0, 1.68f, 0);
    }
    public void MenuReturn()
    {
        soundMenu.SetActive(false);
        pauseMenu.SetActive(true);
        AudioManager.SetVolume(volume);
    }
    public void ExitGame()
    {
        ResumeGame();
        AudioManager.StopSound("Music1");
        AudioManager.StopSound("Music2");
        SceneManager.LoadScene(sceneName: "Main Menu");
    }

    //Rename however you want
    public void Pause()
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
