using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{

    // Okay so this whole script is just to make the platforms crouch-throughable when pressing down arrow

    private PlatformEffector2D effector;
    float waitTime = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.S))
        {
            //effector.rotationalOffset = 0f;
            waitTime = 0.2f;
        }

        if(Input.GetKey(KeyCode.S))
        {
            if(waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = 0.2f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            effector.rotationalOffset = 0f;
        }
    }
}
