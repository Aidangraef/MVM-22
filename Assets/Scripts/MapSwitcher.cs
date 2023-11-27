using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject playerCam;
    public bool mapIsActive = false;
    [SerializeField] private GameObject mapDot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("m"))
        {
            if(mapIsActive)
            {
                playerCam.SetActive(true);
                mapDot.SetActive(false);
            }
            else if (!mapIsActive)
            {
                playerCam.SetActive(false);
                mapDot.SetActive(true);
            }
            mapIsActive = !mapIsActive;
        }
    }
}
