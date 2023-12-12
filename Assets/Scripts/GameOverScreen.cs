using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject player;
    public GameObject advisor;
    public Transform StartPosition;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void KeepPlaying()
    {
        player.GetComponent<PlayerMovement>().speed = 30f;
        player.GetComponent<PlayerMovement>().jumpForce = 650f;
        //player.GetComponent<PlayerMovement>().hp = player.GetComponent<PlayerMovement>().maxHP;
        player.GetComponent<PlayerMovement>().heal(player.GetComponent<PlayerMovement>().maxHP);

        //advisor.transform.position = StartPosition.position;
        player.transform.position = StartPosition.position;
        gameObject.SetActive(false);
    }
}
