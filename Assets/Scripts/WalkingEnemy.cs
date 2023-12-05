using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private float dirX;
    private Rigidbody2D rb;
    private bool facingRight = true;

    // sound stuff
    float framesBetweenSound = 360;
    float frameTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dirX = 1f;

        frameTimer = framesBetweenSound + Random.Range(2, 60);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if(frameTimer <=0 )
        {
            //AkSoundEngine.PostEvent("")
            frameTimer = framesBetweenSound;
        }
        else
        {
            frameTimer -= 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            dirX *= -1;
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // make player do their stuff
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(transform, 1);

            // enemy do your stuff (recoil)
            Vector3 direction = transform.position - collision.transform.position;
            rb.AddForce(direction * 100, ForceMode2D.Impulse);
        }
    }

    private void Flip()
    {
        // change the direction of the sprite
        facingRight = !facingRight;

        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
