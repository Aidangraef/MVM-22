using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advisor : MonoBehaviour
{
    [SerializeField] private float speed = 25f;
    [SerializeField] private Transform target;
    Rigidbody2D rb;
    Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        moveDirection = direction;
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * speed;
        //Debug.Log("move direction: " + moveDirection.x + ", " + moveDirection.y);
        //Debug.Log("velocity: " + rb.velocity);
    }
}
