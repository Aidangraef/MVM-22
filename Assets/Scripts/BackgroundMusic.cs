using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("musicStart", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
