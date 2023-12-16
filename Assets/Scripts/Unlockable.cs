using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlockable : MonoBehaviour
{
    public string whatToUnlock;
    DoorToBoss doorToBoss;

    // Start is called before the first frame update
    void Start()
    {
        doorToBoss = GameObject.FindGameObjectWithTag("DoorToBoss").GetComponent<DoorToBoss>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerMovement pm = other.GetComponent<PlayerMovement>();
            if(whatToUnlock == "dash")
            {
                pm.dashUnlocked = true;
                //AkSoundEngine.PostEvent("firstUpgrade", gameObject);
            }
            else if(whatToUnlock == "double jump")
            {
                pm.doubleJumpUnlocked = true;
                //AkSoundEngine.PostEvent("secondUpgrade", gameObject);
            }
            else if (whatToUnlock == "shield")
            {
                pm.shieldUnlocked = true;
                //AkSoundEngine.PostEvent("thirdUpgrade", gameObject);
            }
            else if(whatToUnlock == "absorb bullets")
            {
                pm.absorbBulletsUnlocked = true;
                //AkSoundEngine.PostEvent("fourthUpgrade", gameObject);
            }
            doorToBoss.collectedCrystals += 1;

            // determine which sound to play
            if(doorToBoss.collectedCrystals == 1)
            {
                AkSoundEngine.PostEvent("firstUpgrade", gameObject);
            }
            else if (doorToBoss.collectedCrystals == 2)
            {
                AkSoundEngine.PostEvent("secondUpgrade", gameObject);
            }
            else if (doorToBoss.collectedCrystals == 3)
            {
                AkSoundEngine.PostEvent("thirdUpgrade", gameObject);
            }
            if (doorToBoss.collectedCrystals == 4)
            {
                AkSoundEngine.PostEvent("fourthUpgrade", gameObject);
            }

            Destroy(gameObject);
        }
    }
}
