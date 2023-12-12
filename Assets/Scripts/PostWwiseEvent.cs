using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEvent : MonoBehaviour
{
    public AK.Wwise.Event MyEvent;
    public AK.Wwise.Event MyEvent2;
    public AK.Wwise.Event MyEvent3;
    // Use this for initialization.
    public void PlayEventSound()
    {
        MyEvent.Post(gameObject);
        Debug.Log("Posted" + MyEvent);
    }

    public void PlayEvent2Sound()
    {
        MyEvent2.Post(gameObject);
        Debug.Log("Posted" + MyEvent2);
    }

    public void PlayEvent3Sound()
    {
        MyEvent3.Post(gameObject);
        Debug.Log("Posted" + MyEvent3);
    }

    // Update is called once per frame.
    void Update()
    {

    }
}
