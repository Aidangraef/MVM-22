using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DoorToBoss : MonoBehaviour
{
    GameObject Player;
    GameObject dialogueParent;
    private bool touchingPlayer;
    public int collectedCrystals = 0;
    GameObject EPrompt;
    int currentText = 0;

    float oldSpeed;
    float OldJumpForce;

    private void Awake()
    {
        dialogueParent = GameObject.FindGameObjectsWithTag("Dialogue")[0]; // should be the only object with this tag
        Player = GameObject.FindGameObjectWithTag("Player");
        EPrompt = GameObject.FindGameObjectWithTag("EPrompt");
    }

    // Start is called before the first frame update
    void Start()
    {

        oldSpeed = Player.GetComponent<PlayerMovement>().speed;
        OldJumpForce = Player.GetComponent<PlayerMovement>().jumpForce;
    }

    // Update is called once per frame
    void Update()
    {
        if (touchingPlayer && Input.GetKeyDown(KeyCode.E))
        {
            if (currentText == 0)
            {
                Player.GetComponent<PlayerMovement>().speed = 0;
                Player.GetComponent<PlayerMovement>().jumpForce = 0;
                Debug.Log(dialogueParent.tag);
                Debug.Log(dialogueParent.transform.GetChild(1).gameObject);
                Debug.Log(dialogueParent.transform.GetChild(1).GetChild(0).gameObject);
                Debug.Log(dialogueParent.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>());
                dialogueParent.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "This is the door to the heavens.";
                dialogueParent.GetComponent<Animator>().SetBool("dialogueOn", true);
                currentText += 1;
            }
            else if (currentText == 1 && collectedCrystals < 4)
            {
                dialogueParent.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "We can't open this until we have all four crystals";
                currentText += 1;
            }
            else if (currentText > 0 && collectedCrystals == 4)
            {
                dialogueParent.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "We have all the crystals! Let's go!";
                currentText += 1;
                AkSoundEngine.PostEvent("bossDoorOpen", gameObject);
            }
            else if (currentText == 2 && collectedCrystals < 4)
            {
                dialogueParent.GetComponent<Animator>().SetBool("dialogueOn", false);
                Player.GetComponent<PlayerMovement>().speed = oldSpeed;
                Player.GetComponent<PlayerMovement>().jumpForce = OldJumpForce;
                currentText = 0;
            }
            else if (currentText == 2 && collectedCrystals == 4)
            {
                SceneManager.LoadScene(2);
            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Can Upgrade");
            touchingPlayer = true;
            EPrompt.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            touchingPlayer = false;
            EPrompt.SetActive(false);
        }
    }
}
