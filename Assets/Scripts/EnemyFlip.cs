using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;
public class EnemyFlip : MonoBehaviour
{
    private Rigidbody2D rb; 
    private AIPath aiPath;

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
}
