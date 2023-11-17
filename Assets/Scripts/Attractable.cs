using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractable : MonoBehaviour
{
    [SerializeField] private float attractableRadius = 0.5f;
    [SerializeField] private float attractionStrength = 1f;
    [SerializeField] private LayerMask blackHoleLayer;
    [SerializeField] private bool storable;

    private GameObject blackHole;
    private Rigidbody2D rb;
    private bool shrinking;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shrinking = false;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attractableRadius, blackHoleLayer); // check for nearby black holes

        // no nearby black holes
        if (colliders.Length == 0)
        {
            blackHole = null;
        }

        for(int i = 0; i < colliders.Length; i++) // cycle through all found colliders
        {
            blackHole = colliders[i].gameObject; // right now we just get the last collider. If we want to have multiple black holes, this will become a TODO
        }
    }

    private void FixedUpdate()
    {
        if (blackHole == null) return; // don't do anything if no black hole

        float magsqr; // offset squared between object and black hole
        Vector3 offset; // distance to black hole

        offset = blackHole.transform.position - transform.position;
        offset.z = 0; // because 2D

        magsqr = offset.sqrMagnitude;

        // prevent division by 0
        if(magsqr > 0.0001f)
        {
            rb.AddForce((attractionStrength * offset.normalized / magsqr) * rb.mass);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // only store storable objects
        if (!storable) return;

        // check for collision with black hole
        if (collision.gameObject.tag != "Black Hole") return;

        // don't start the coroutine if it is already happening
        if (shrinking) return;

        shrinking = true;
        StartCoroutine(DoShrink());
    }

    public IEnumerator DoShrink()
    {
        for(int i = 0; i < 20; i++)
        {
            Vector3 newScale = transform.localScale;
            newScale.x -= 0.05f;
            newScale.y -= 0.05f;
            transform.localScale = newScale;
            yield return new WaitForSeconds(0.05f);
        }

        // Destroy this object now
        Destroy(gameObject);
    }
}
