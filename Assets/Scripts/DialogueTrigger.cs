using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    private GameObject dialogueParent; // this is the game object that has the animation to switch between dialogue box and inventory.
    private float dialogueTimer = 0f;
    [SerializeField] private string text;
    [SerializeField] private float framesOnScreen;
    private bool shownDialogue = false;

    [SerializeField] bool isLock = false;
    private GameObject ePrompt;
    [SerializeField] List<string> moreTexts;

    GameObject Player;
    float oldSpeed;
    float OldJumpForce;

    // Start is called before the first frame update
    void Awake()
    {
        dialogueParent = GameObject.FindGameObjectsWithTag("Dialogue")[0]; // should be the only object with this tag
        ePrompt = GameObject.FindGameObjectWithTag("EPrompt");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("e"))
        {

            if(moreTexts.Count == 0 && isLock && dialogueTimer > 0f)
            {
                dialogueTimer = 0f;
                shownDialogue = true;

            }
            if(dialogueTimer > 0f && isLock)
            {
                text = moreTexts[0];
                moreTexts.RemoveAt(0);
                dialogueParent.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
            }
        }
    }

    private void FixedUpdate()
    {
        
        if(dialogueTimer > 0f) // dialogue is open
        {
            if(isLock)
            {
                
            }
            else
            {
                dialogueTimer -= 1f;
            }
        }
        else if (dialogueTimer <= 0 && shownDialogue)
        {
            dialogueParent.GetComponent<Animator>().SetBool("dialogueOn", false); // hide the dialogue box
            ePrompt.SetActive(false);
            if(isLock)
            {
                Player.GetComponent<PlayerMovement>().speed = oldSpeed;
                Player.GetComponent<PlayerMovement>().jumpForce = OldJumpForce;
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" || shownDialogue) return; // only care about the player, nothing else, and only show dialogue once

        Player = collision.gameObject;

        // check if dialogue is locking controls
        if(isLock)
        {
            // store the old speed and jump force, then set it to zero so the player cannot move while in dialogue lock
            oldSpeed = collision.gameObject.GetComponent<PlayerMovement>().speed;
            OldJumpForce = collision.gameObject.GetComponent<PlayerMovement>().jumpForce;
            collision.gameObject.GetComponent<PlayerMovement>().speed = 0;
            collision.gameObject.GetComponent<PlayerMovement>().jumpForce = 0;
            ePrompt.SetActive(true);
        }

        // show the dialogue for a short period of time
        dialogueTimer = framesOnScreen;
        dialogueParent.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        dialogueParent.GetComponent<Animator>().SetBool("dialogueOn", true);
        shownDialogue = true;
    }
}
