using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlockable : MonoBehaviour
{
    public string whatToUnlock;

    // Start is called before the first frame update
    void Start()
    {
        
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
            }
            else if(whatToUnlock == "double jump")
            {
                pm.doubleJumpUnlocked = true;
            }
            else if (whatToUnlock == "shield")
            {
                pm.shieldUnlocked = true;
            }
            else if(whatToUnlock == "absorb bullets")
            {
                pm.absorbBulletsUnlocked = true;
            }

            Destroy(gameObject);
        }
    }
}
