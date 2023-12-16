using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealPool : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private int healAmount = 2;
    [SerializeField] private float timer;
    private bool canHeal; 

    void Awake(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        canHeal = true; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && canHeal)
        {
            Debug.Log("HEAL");
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.heal(healAmount);

            // Start cooldown coroutine
            StartCoroutine(StartCooldown());
        }
    }

    IEnumerator StartCooldown()
    {
        canHeal = false;
        yield return new WaitForSeconds(timer);
        canHeal = true;
    }
}
