using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleMovement : MonoBehaviour
{
    [SerializeField] private GameObject blackHole;

    private bool updatePosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // only move the black hole if the mouse button is held down
        if(Input.GetMouseButtonDown(0))
        {
            updatePosition = true;
        }
        else
        {
            //updatePosition = false;
        }

        if(Input.GetMouseButtonDown(1))
        {
            blackHole.transform.position = new Vector3(-3000, -3000, 0); // "delete" the black hole
        }
    }

    private void FixedUpdate()
    {
        if (!updatePosition) return; // mouse is not being clicked so nothing needs to happen

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // 2D
        blackHole.transform.position = mousePos;
        updatePosition = false;
    }
}
