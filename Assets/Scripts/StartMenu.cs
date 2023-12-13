using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    float playDelay = 2.5f;

    public GameObject menu;
    public GameObject credits;
    public GameObject settings;
    //public GameObject howToPlay;

    [SerializeField]
    ImageFadeEffect fadeEffect;

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(true);
        credits.SetActive(false);
        settings.SetActive(false);
        //howToPlay.SetActive(false);
    }

    public void Play()
    {
        // Disappear buttons
        menu.SetActive(false);

        // for music change with scenes
        AkSoundEngine.PostEvent("gameStart", this.gameObject);

        // Start fade effect
        fadeEffect.FadeSpeed = 1f / playDelay;
        fadeEffect.TargetAlpha = 1f;

        StartCoroutine(WaitAndStartGame());
    }

    IEnumerator WaitAndStartGame()
    {

        yield return new WaitForSeconds(playDelay);
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        menu.SetActive(false);
        AkSoundEngine.PostEvent("buttonClick", this.gameObject);
        credits.SetActive(true);
        AkSoundEngine.PostEvent("toCredits", this.gameObject);
    }

    public void Settings()
    {
        menu.SetActive(false);
        AkSoundEngine.PostEvent("buttonClick", this.gameObject);
        settings.SetActive(true);
    }

    /*public void HowToPlay()
    {
        menu.SetActive(false);
        AkSoundEngine.PostEvent("buttonClick", this.gameObject);
        howToPlay.SetActive(true);
    }*/

    public void Back()
    {
        menu.SetActive(true);
        AkSoundEngine.PostEvent("buttonClick", this.gameObject);
        credits.SetActive(false);
        settings.SetActive(false);
        AkSoundEngine.PostEvent("toMenu", this.gameObject);
        //howToPlay.SetActive(false);
    }

    public void Quit()
    {
        AkSoundEngine.PostEvent("buttonClick", this.gameObject);
        Application.Quit();
    }
}