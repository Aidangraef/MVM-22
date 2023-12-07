using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    GameObject attackHitBox;
    float attackTimer = 0f;
    float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] public int attackDamage;

    string currentAttack = "none";
    string currentSound = "none";

    // Start is called before the first frame update
    void Start()
    {
        attackHitBox = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<PlayerMovement>().hp <= 0)
        {
            return; // do nothing if dead
        }

        if (GetComponent<PlayerMovement>().shieldActivated) return; // cannot attack if shielding

        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1))
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (attackTimer != 0)
        {
            attackTimer -= 1;
        }
        else
        {
            currentAttack = "none";
            attackHitBox.SetActive(false);
        }
    }

    void Attack()
    {
        attackHitBox.SetActive(true);
        attackTimer = 15f;

        // animation - cycle attacks
        if (currentAttack == "none" || currentAttack == "Attack3")
        {
            currentAttack = "Attack1";
            currentSound = "swing1";
        }
        else if (currentAttack == "Attack1")
        {
            currentAttack = "Attack2";
            currentSound = "swing2";

        }
        else if (currentAttack == "Attack2")
        {
            currentAttack = "Attack3";
            currentSound = "swing3";

        }
        attackHitBox.GetComponent<Animator>().SetTrigger(currentAttack);
        AkSoundEngine.PostEvent(currentSound, this.gameObject);

        // detect enemies
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackHitBox.transform.position, attackRange, enemyLayers);

        // apply damage
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyFlip>().TakeDamage(attackDamage);
            // TODO: write kill code
            //Destroy(enemy.gameObject);
        }
    }
}
