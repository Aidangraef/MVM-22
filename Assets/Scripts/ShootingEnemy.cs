using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] private bool facingLeft;
    [SerializeField] private float framesBetweenShots = 260f;
    [SerializeField] private float offsetFrames;
    private float shotTimer = 0f;
    [SerializeField] GameObject bulletPrefab;
    private Animator animator;
    Transform shootPoint;

    // Start is called before the first frame update
    void Start()
    {
        // make the cannon face the correct way
        if(facingLeft)
        {
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        animator = GetComponent<Animator>();
        shootPoint = transform.GetChild(0);

        shotTimer = offsetFrames + framesBetweenShots;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // time to shoot again
        if(shotTimer <= 0f)
        {
            animator.SetTrigger("Shoot");
            Instantiate(bulletPrefab, shootPoint);
            shotTimer = framesBetweenShots;
        }
        else
        {
            shotTimer -= 1;
        }
    }


}
