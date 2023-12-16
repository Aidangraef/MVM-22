using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimizer : MonoBehaviour
{
    GameObject player;
    float renderDistance = 100f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Despawn();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.transform.position) < renderDistance)
        {
            Respawn();
        }
        else
        {
            Despawn();
        }
    }

    void Despawn()
    {
        // disable everything
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void Respawn()
    {
        // enable everything
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
