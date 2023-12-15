using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    int hp = 100;

    float attackTimer; // used to put time delays between attacks
    public bool inAttack = false;
    float inAttackTimer; // used to make sure we don't stay in an attack so long

    Animator animator;
    Collider2D idleCollider;
    GameObject attack1Collider;

    public GameObject BulletPrefab;

    [SerializeField]
    float playDelay = 2.5f;

    [SerializeField]
    ImageFadeEffect fadeEffect;

    IEnumerator WaitAndEndGame()
    {

        yield return new WaitForSeconds(playDelay);
        SceneManager.LoadScene(3);
    }

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("bossStart", gameObject);

        attackTimer = Random.Range(1f, 3f);
        animator = GetComponent<Animator>();
        idleCollider = GetComponent<Collider2D>();
        attack1Collider = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(inAttack)
        {
            inAttackTimer -= Time.fixedDeltaTime;
            if(inAttackTimer <= 0)
            {
                inAttack = false;
                attack1Collider.SetActive(false);
                idleCollider.enabled = true;
            }
        }

        if (!inAttack)
        {
            // subtract from attackTimer
            attackTimer -= Time.fixedDeltaTime;

            // check if it is time for an attack
            if(attackTimer <= 0)
            {
                inAttack = true;
                attackTimer = Random.Range(1f, 3f);
                inAttackTimer = 3f;

                // pick a random attack
                float attackNum = Random.Range(0, 100);
                Debug.Log(attackNum);
                if(attackNum <= 50)
                {
                    animator.SetTrigger("Attack1");
                    AkSoundEngine.PostEvent("bossDash", gameObject);
                    idleCollider.enabled = false;
                    attack1Collider.SetActive(true);

                }
                else if (attackNum > 50)
                {
                    animator.SetTrigger("Attack2");
                    AkSoundEngine.PostEvent("bossShoot", gameObject);
                }
            }
        }
    }

    public void ExitAttack()
    {
        inAttack = false;
        attack1Collider.SetActive(false);
        idleCollider.enabled = true;
    }

    public void ShootBullet()
    {
        Instantiate(BulletPrefab);
    }

    public void TakeDamage(int damage)
    {
        AkSoundEngine.PostEvent("bossHurt", gameObject);

        hp -= damage;
        StartCoroutine(DoFlashRed());

        if (hp < 0)
        {
            AkSoundEngine.PostEvent("bossDie", gameObject);
            AkSoundEngine.PostEvent("toCredits", gameObject);
            // Start fade effect
            fadeEffect.FadeSpeed = 1f / playDelay;
            fadeEffect.TargetAlpha = 1f;
            //SceneManager.LoadScene(3);
            StartCoroutine(WaitAndEndGame());
            //Destroy(gameObject);
        }

    }

    IEnumerator DoFlashRed()
    {
        // initial setup
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color originalColor = new Color(1, 1, 1, 1);
        Color redColor = new Color(1, 0, 0, 1);

        // flash and wait and turn back
        for (int i = 0; i < 3; i++)
        {
            sprite.color = redColor;
            yield return new WaitForSeconds(0.1f);
            sprite.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // make player do their stuff
            collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(transform, 1);
        }
    }
}
