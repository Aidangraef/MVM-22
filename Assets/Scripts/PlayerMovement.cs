using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float speed; // how fast the player moves
    [SerializeField] public int maxHP = 5; 
    [SerializeField] public int hp = 5;
    [SerializeField] private float jumpForce = 400f; // force added when the player jumps
    [SerializeField] private float jumpDecay = 0.33f;
    [SerializeField] private float movementSmoothing = 0.05f; // how much to smooth the movement
    [SerializeField] private float coyoteTime = 0.2f; // how much time after running off a ledge the player will float before falling
    [SerializeField] private float jumpBufferTime = 0.2f; // how much time the player can buffer a jump
    [SerializeField] private LayerMask whatIsGround; // used to determine when the player touches the ground so they can jump again
    [SerializeField] private Transform groundCheck; // place this gameobject as a child of the player at the bottom of the sprite
    [SerializeField] private GameObject HPUIBar;
    [SerializeField] private GameObject heartObject; 
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private MapSwitcher mapSwitcher;
    const float groundedRadius = 0.2f; // used for checking if the player is on the ground
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    public bool grounded;
    Vector3 lastGroundedPosition;
    private Rigidbody2D rb;
    public bool facingRight = true; // for sprite flipping
    private Vector3 velocity = Vector3.zero;

    public UnityEvent OnLandEvent; // pretty straightforward

    private float damageTimer = 0f;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    Animator animator;
    //GameObject camera;

    bool invincible; // if the player is temporarily invincible due to damage or something
    float invincibleTimer = -1f;
    SpriteRenderer spriteRenderer;
    float input; // player keyboard input
    bool jump; // becomes true when the player tries to jump
    bool releaseJump; // becomes true when the player lets go of the jump button

    private void Awake()
    {
        // All normal set up stuff
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hp = maxHP;
        //camera = camera.main

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }

        invincible = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // cannot move when in map view
        if (mapSwitcher.mapIsActive) return;

        // get player input in Update
        input = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown("space"))
        {
            jump = true;
            jumpBufferCounter = jumpBufferTime;
        }

        if(Input.GetKeyUp("space"))
        {
            releaseJump = true;
        }
    }

    private void FixedUpdate()
    {
        // and actually do the movement in FixedUpdate

        // animation
        if(input >= -0.0001 || input <= 0.0001)
        {
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
        }

        // invincibility
        if(invincibleTimer > 0)
        {
            invincibleTimer -= 1;

            // blink
            if (invincibleTimer % 4 == 0)
            {
                spriteRenderer.enabled = false;
            }
            else
            {
                spriteRenderer.enabled = true;
            }
        }
        else if (invincibleTimer == 0)
        {
            invincibleTimer = -1f; // keep at -1 when not invincible
            invincible = false;
            spriteRenderer.enabled = true;
        }

        // damage recoil
        if (damageTimer > 0)
        {
            damageTimer -= 1;
            return;
        }

        // subtract from jumpBufferCounter
        jumpBufferCounter -= Time.fixedDeltaTime;

        // save last grounded position
        if (grounded) lastGroundedPosition = transform.position;

        // determine if the player is grounded
        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround); // check the groundCheck for nearby ground
        for (int i = 0; i < colliders.Length; i++) // cycle through all found colliders
        {
            if(colliders[i].gameObject != gameObject)
            {
                grounded = true; // found nearby ground
                if(!wasGrounded)
                {
                    OnLandEvent.Invoke(); // handle results
                }
            }
        }

        // handle coyote time
        if(grounded)
        {
            coyoteTimeCounter = coyoteTime; // only count down if not on the ground
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        Move(input * speed * Time.fixedDeltaTime, jump); // make your move!
        jump = false; //handled in move, so set to false here
        releaseJump = false; // handled in move, so set to false here
    }

    public void Move(float move, bool jump)
    {
        // move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);

        // smooth it out and apply to player
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

        // flip the sprite depending on the input and direcetion they're facing
        if ((move > 0 && !facingRight) || (move < 0 && facingRight))
        {
            Flip();
        }

        // handle the jump
        if(coyoteTimeCounter > 0 && jumpBufferCounter > 0)
        {
            // Add a vertical force to the player
            grounded = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0f, jumpForce));
            jumpBufferCounter = 0;
        }

        // released jump button, so fall faster
        if(releaseJump && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpDecay);
            coyoteTimeCounter = 0f; // prevents the player from infinite jumping by spamming the jump button
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

    public void TakeDamage(Transform enemy, int dmg)
    {
        if (invincible || hp <= 0)
        {
            return;
        }

        // check if spikes dealt the damage
        if (enemy.gameObject.tag == "Spikes")
        {
            // teleport to last ground position
            transform.position = lastGroundedPosition;
        }
        else
        {
            // recoil
            Vector3 direction = transform.position - enemy.position;
            rb.AddForce(direction * 25, ForceMode2D.Impulse);
            damageTimer = 10f; // wait this many frames
        }

        // Decrease health
        hp -= dmg;

        // check for dead
        if(hp <= 0)
        {
            GameOver();
        }

        // set invincible
        invincible = true;
        invincibleTimer = 60f;

        // update UI
        Destroy(HPUIBar.transform.GetChild(0).gameObject);
    }

    public void IncreaseMaxHP()
    {
        maxHP += 1;

    }
    
    public void heal(int amount)
    {
        Debug.Log("HEALING");
        //Save the original HP amount
        int originalHP = hp;
        //add healing amount 
        hp += amount; 
        //make sure that its not over the max health
        hp = Mathf.Min(hp, maxHP);
        //Update the UI to refelct the changes using the original difference that was calculated
        //instantiate the new heart objects amount of times equal to the difference calculated with a seperation on the x axis of 125 to seperate them
        if (amount + originalHP < maxHP + 1){
            for(int i = 0; i < amount; i++)
            {
                Debug.Log("Inserting: " + amount);
                Instantiate(heartObject, new Vector2 (HPUIBar.transform.GetChild(originalHP - 1).gameObject.transform.position.x + 125, HPUIBar.transform.GetChild(originalHP - 1).gameObject.transform.position.y), Quaternion.identity, HPUIBar.transform);
            }
        }
    }

    void GameOver()
    {
        GameOverPanel.SetActive(true);
        speed = 0f;
        jumpForce = 0f;
    }
}
