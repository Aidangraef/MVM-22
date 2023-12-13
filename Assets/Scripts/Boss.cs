using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    float attackTimer; // used to put time delays between attacks
    public bool inAttack = false;
    float inAttackTimer; // used to make sure we don't stay in an attack so long

    Animator animator;
    Collider2D idleCollider;
    GameObject attack1Collider;

    public GameObject BulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
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
                    idleCollider.enabled = false;
                    attack1Collider.SetActive(true);

                }
                else if (attackNum > 50)
                {
                    animator.SetTrigger("Attack2");
                }
            }
        }
    }

    public void ExitAttack()
    {
        inAttack = false;
        attack1Collider.SetActive(false);
    }

    public void ShootBullet()
    {
        Instantiate(BulletPrefab);
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
