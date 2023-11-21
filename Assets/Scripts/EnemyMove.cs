using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Vector2 point1;
    [SerializeField] private Vector2 point2;
    [SerializeField] private float waitTime;

    private Vector2 currentTarget;
    [SerializeField] private float movementSpeed = 5f;
    private bool isMoving = true;

    Rigidbody2D rb;

    void Start()
    {
        // Start by moving towards point1
        currentTarget = point1;

        // set rb
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //We can set isMoving = false if we don't want it to move. Could be useful if its unloaded for instance idk
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        // Calculate the direction to move towards the current target
        Vector2 direction = (currentTarget - new Vector2(transform.position.x, transform.position.y).normalized);

        // Move the GameObject towards the target
        transform.Translate(direction * movementSpeed * Time.deltaTime);

        // Check if the GameObject has reached the current target
        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), currentTarget);
        if (distance < 0.1f)
        {
            // Stop moving when reached the target
            isMoving = false;
            Invoke("SwitchTarget", waitTime);  // Wait for waitTime before switching targets and moving again
        }
    }

    void SwitchTarget()
    {
        // Switch to the other target
        if (currentTarget == point1)
        {
            currentTarget = point2;
        }
        else
        {
            currentTarget = point1;
        }

        // Resume moving
        isMoving = true;
    }

    // check for player collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("collided with player");
            // make player do their stuff
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(transform, 1);

            // enemy do your stuff (recoil)
            Vector3 direction = transform.position - collision.transform.position;
            rb.AddForce(direction * 3, ForceMode2D.Impulse);
        }
    }
}
