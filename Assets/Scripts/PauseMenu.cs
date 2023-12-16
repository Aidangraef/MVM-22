using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButtons;
    public bool isPaused = false;
    public GameObject settings;
    public bool settingsActive = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*if (pauseMenu.activeSelf)
        {
            isPaused = true;
        }
        else isPaused = false; */
        if (Input.GetKeyDown(KeyCode.Escape) & (isPaused == false))
        {
            pauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
    }
    public void Settings()
    {
        if (!settingsActive)
        {
            isPaused = true;
            settingsActive = true;
            pauseButtons.SetActive(false);
            AkSoundEngine.PostEvent("buttonClick", this.gameObject);
            settings.SetActive(true);
        }
    }
    public void Back()
    {
        pauseButtons.SetActive(true);
        AkSoundEngine.PostEvent("buttonClick", this.gameObject);
        settings.SetActive(false);
        settingsActive = false;
    }
    public void Continue()
    {
        isPaused = true;
        AkSoundEngine.PostEvent("buttonClick", this.gameObject);
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
}
