using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    Animator animator;
    bool on = false;

    [SerializeField] private Animator linkedObject; // the animator of the object the button controls
    [SerializeField] private string boolName; // the name of the bool in the linked object's animator


    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetTrigger("Press");
        on = !on;

        // affect the triggered object
        linkedObject.SetBool(boolName, on);
    }
}
