using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UpgradeStation : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private bool isHealthUpgrade;
    [SerializeField] private bool isWeaponUpgrade;
    private PlayerMovement playerMovement; 
    private PlayerCombat playerCombat; 
    [SerializeField] private int upgradeAmount; 
    private bool touchingPlayer; 

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerCombat = player.GetComponent<PlayerCombat>();
    }

    void Update(){
        if(touchingPlayer && Input.GetKey(KeyCode.E)){
            if(isHealthUpgrade){
                //maybe popup an upgrade textbox?
                playerMovement.maxHP += upgradeAmount;
                playerMovement.heal(1);
                Destroy(this.gameObject);
            }
            if(isWeaponUpgrade){
                playerCombat.attackDamage += upgradeAmount;
                //maybe make an animation for the upgrade?
                Destroy(this.gameObject); //just to make sure it only triggers once
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Player"){
            Debug.Log("Can Upgrade");
            touchingPlayer = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player"){
            touchingPlayer = false; 
        }
    }
}
