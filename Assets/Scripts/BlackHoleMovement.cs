using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleMovement : MonoBehaviour
{
    [SerializeField] private GameObject blackHole;
    [SerializeField] private GameObject blackHoleSprite;
    [SerializeField] private MapSwitcher map;

    private bool updatePosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // cannot move black hole in map mode
        if (map != null && map.mapIsActive) return;

        // only move the black hole if the mouse button is held down
        if(Input.GetMouseButtonDown(1))
        {
            updatePosition = true;
            AkSoundEngine.PostEvent("gravStart", this.gameObject);
        }
        else
        {
            //updatePosition = false;
        }

        if(Input.GetKeyDown("v") || Input.GetMouseButtonUp(1))
        {
            blackHole.transform.position = new Vector3(-3000, -3000, 0); // "delete" the black hole
            blackHoleSprite.transform.position = new Vector3(-3000, -3000, 0); // "delete" the black hole sprite
            AkSoundEngine.PostEvent("gravEnd", this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        // get mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // 2D

        // rotate black hole towards mouse always
        Vector2 direction = new Vector2(mousePos.x - blackHole.transform.position.x, mousePos.y - blackHole.transform.position.y);
        blackHole.transform.up = direction;

        if (!updatePosition) return; // mouse is not being clicked so nothing needs to happen

        blackHole.transform.position = mousePos;
        blackHoleSprite.transform.position = mousePos;
        updatePosition = false;
    }
}
