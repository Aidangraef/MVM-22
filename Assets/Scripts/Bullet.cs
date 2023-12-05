using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float direction; // will be 1 for right or -1 for left
    public float shootingForce = 30f;

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.parent.parent.localScale.x;
        GetComponent<Rigidbody2D>().AddForce(new Vector3(direction * shootingForce, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // make player do their stuff
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(transform, 1);

            // enemy do your stuff (die)
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag != "Black Hole")
        {
            Destroy(gameObject); // die if you collide with anything but the black hole
        }
    }
}
