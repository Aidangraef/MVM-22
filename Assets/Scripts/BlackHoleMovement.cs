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
        if(Input.GetMouseButton(0))
        {
            updatePosition = true;
        }
        else
        {
            updatePosition = false;
        }
    }

    private void FixedUpdate()
    {
        if (!updatePosition) return; // mouse is not being clicked so nothing needs to happen

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // 2D

        blackHole.transform.position = mousePos;
    }
}
