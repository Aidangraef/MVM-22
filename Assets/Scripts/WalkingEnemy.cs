using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    Transform player;
    [SerializeField] private float moveSpeed;
    public float dirX;
    private Rigidbody2D rb;
    private bool facingRight = true;

    [SerializeField] float kbTime = 30f;
    float kbTimer = 0f;
    [SerializeField] float kbAmount = 500f;
    int maxHP = 3;
    int hp = 3;

    // sound stuff
    float framesBetweenSound = 360;
    float frameTimer;

    private void Awake()
    {
        dirX = 1f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //dirX = 1f;
        frameTimer = framesBetweenSound + Random.Range(2, 60);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(kbTimer > 0)
        {
            kbTimer -= 1;
        }
        else
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }

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
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Walking Enemy" || collision.gameObject.tag == "Shield")
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

    public void Flip()
    {
        // change the direction of the sprite
        facingRight = !facingRight;

        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }

    public void TakeDamage(int amount)
    {
        //take damage
        hp -= amount;

        //take knockback
        Knockback();

        //if dead, die
        if (hp < 0)
        {
            AkSoundEngine.PostEvent("walkerDie", this.gameObject);
            Destroy(gameObject);
        }

        //else, flash red+hurt
        else
        {
            StartCoroutine(DoFlashRed());
            AkSoundEngine.PostEvent("walkerHurt", this.gameObject);
        }
        }

        void Knockback()
    {
        kbTimer = kbTime;
        //StartCoroutine(ToggleScript());
        Debug.Log("Taking KB");
        Vector3 direction = transform.position - player.transform.position;
        direction = new Vector3(direction.x, direction.y + 0.75f, direction.z); // pop 'em up a bit
        rb.velocity = Vector2.zero;
        rb.AddForce(direction.normalized * kbAmount, ForceMode2D.Impulse);
    }

    IEnumerator DoFlashRed()
    {
        // initial setup
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color originalColor = new Color(1, 1, 1, 1);
        Color redColor = new Color(1, 0, 0, 1);

        // flash and wait and turn back
        for(int i = 0; i < 3; i++)
        {
            sprite.color = redColor;
            yield return new WaitForSeconds(0.1f);
            sprite.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
