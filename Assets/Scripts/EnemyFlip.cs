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
    [SerializeField] float kbTime;
    [SerializeField] float kbAmount; 
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        aiPath = GetComponent<AIPath>();
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
        if (aiPath.desiredVelocity.x > 0.1){
            transform.localScale = new Vector3(1, -1, 1);
        }
        else{
            transform.localScale = new Vector3(1, 1, 1);
        }
        
    }

    // check for player collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // make player do their stuff
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(transform, 1);
            StartCoroutine(ToggleScript());
            // enemy do your stuff (recoil)
            Vector3 direction = transform.position - collision.transform.position;
            rb.velocity = Vector2.zero;
            rb.AddForce(direction.normalized * kbAmount, ForceMode2D.Impulse);
        }
    }

    IEnumerator ToggleScript()
    {
        aiPath.enabled = false;
        yield return new WaitForSeconds(kbTime);
        aiPath.enabled = true;          
    }
}
