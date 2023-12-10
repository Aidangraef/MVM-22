using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;
public class EnemyFlip : MonoBehaviour
{
    private Rigidbody2D rb; 
    private AIPath aiPath;
    private WalkingEnemy WalkingEnemy;
    [SerializeField] float kbTime = 0.2f;
    [SerializeField] float kbAmount = 50f; 
    [SerializeField] private int maxHP;
    [SerializeField] private int hp;
    private GameObject player; 


    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
        WalkingEnemy = GetComponent<WalkingEnemy>();
        player = GameObject.FindGameObjectWithTag("Player");
        hp = maxHP;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update(){

    }

    void FixedUpdate()
    {
        Switch();
    }

    void Switch(){
        if(aiPath != null){
            if (aiPath.desiredVelocity.x > 0.1){
            transform.localScale = new Vector3(1, -1, 1);
            }
            else{
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        
        
    }

    // check for player collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // make player do their stuff
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(transform, 1);
            // enemy do your stuff (recoil)
            Knockback();
        }
    }

    IEnumerator ToggleScript()
    {
        if(aiPath != null){
            aiPath.enabled = false;
        }
        if(WalkingEnemy != null){
            WalkingEnemy.enabled = false;
        }
        
        yield return new WaitForSeconds(kbTime);
        if(aiPath != null){
            aiPath.enabled = true;
        }
        if(WalkingEnemy != null){
            WalkingEnemy.enabled = true;
        }      
    }

    public void TakeDamage(int amount){
        //take damage
        hp -= amount;
        //take knockback
        Knockback();
        if(hp <= 0){
            Die();
        }
    }

    void Knockback(){
        //StartCoroutine(ToggleScript());
        Debug.Log("Taking KB");
        Vector3 direction = transform.position - player.transform.position;
        rb.velocity = Vector2.zero;
        rb.AddForce(direction.normalized * kbAmount, ForceMode2D.Impulse);
    }

    void Die(){
        //if we want animations heres where to put it
        Destroy(this.gameObject);
    }
}
