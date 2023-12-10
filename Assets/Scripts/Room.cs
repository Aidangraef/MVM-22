using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Pathfinding;
using Unity.VisualScripting;
using System.Collections.ObjectModel;
using UnityEngine.Analytics;

public class Room : MonoBehaviour
{
    //makes a list of the specified enemy objects 
    private List<GameObject> enemies = new List<GameObject>();
    private List<Transform> HomePositions = new List<Transform>();

    private GameObject player; 

    public float WaitTime; //The overall time the ai should wait before returning to their home spot
    private float TimeElapsed; //Current value on the timer
    private bool TimerRunning = false; //Is the timer running? 

    // Start is called before the first frame update
    void Start()
    {
        //Grabbing private variables, normal unity stuff keep scrolling :)
        player = GameObject.FindGameObjectWithTag("Player");

        // Add each child of the room (enemies) to the list of enemies
        
        foreach (Transform child in transform)
        {
            enemies.Add(child.gameObject);
        }

        // Disable the AI Destination Setter script on startup
        // Then gets each of its starting positions to know where to return back to after a certain amount of time
        foreach (var enemy in enemies)
        {
            DisableAIPath(enemy); //3 is the location of the ai path setter

            HomePositions.Add(GrabHomePosition(enemy)); //grab the home positions and store them in the home positions vector 
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the timer is running
        if (TimerRunning)
        {
            // Decrease the timer value
            TimeElapsed += Time.deltaTime;

            // Check if the timer has reached zero
            if (TimeElapsed >= WaitTime)
            {
                TimerRunning = false; 
                ReturnHome();
                
            }
        }
    }

    void FixedUpdate(){

    }


    private void OnTriggerEnter2D(Collider2D col)
    {

        StopTimer();
        //if the player enters the hitbox, than it turns ON the ai destination setter script
        if (col.gameObject.CompareTag("Player"))
        {
            foreach (var enemy in enemies)
            {
                AIDestinationSetter AIPathSetter = enemy.GetComponent<AIDestinationSetter>();
                AIPathSetter.target = player.transform;
                EnableAIPath(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        StartTimer();
        //if the player enters the hitbox, than it turns OFF the ai destination setter script
        if (col.gameObject.CompareTag("Player"))
        {
            foreach (var enemy in enemies)
            {
                // Disable the specific component at the specified index
                DisableAIPath(enemy); 
            }
        }
    }

    void DisableAIPath(GameObject obj)
    {
        // Get all components attached to the GameObject
        Component[] components = obj.GetComponents<Component>();


        // Disable the component at the specified index
        Behaviour componentToDisable = components[2] as Behaviour;
        Behaviour componentToDisable2 = components[3] as Behaviour;
        componentToDisable.enabled = false;
        componentToDisable2.enabled = false;
    }

    void EnableAIPath(GameObject obj)
    {

        // Get all components attached to the GameObject
        Component[] components = obj.GetComponents<Component>();


        // Disable the component at the specified index
        Behaviour componentToEnable = components[2] as Behaviour;
        Behaviour componentToEnable2 = components[3] as Behaviour;
        componentToEnable.enabled = true;
        componentToEnable2.enabled = true;
    }

    void StartTimer(){
        Debug.Log("Start Timer");
        TimeElapsed = 0f;

        TimerRunning = true; 
        StartCoroutine(TimerCoroutine());
    }
    
    void StopTimer(){
        TimerRunning = false; 
        StopCoroutine(TimerCoroutine());
    }

    void TimerCompleted(){
        // Action to be performed when the timer is completed
        Debug.Log("Timer completed!");
        ReturnHome();
        // Stop the timer
        StopTimer();
    }


    Transform GrabHomePosition(GameObject obj){

    // Instantiate a temporary GameObject at the same position as the enemy
        GameObject homeTarget = new GameObject("HomeTarget");
        homeTarget.transform.position = obj.transform.position;
        return homeTarget.transform;        
    }


    void ReturnHome()
    {
        int iterationCount = 0;

        foreach (var enemy in enemies)
        {
            // Turn on the scripts
            EnableAIPath(enemy); 

            //Set the home destination as the current place to go
            AIDestinationSetter AIPathSetter = enemy.GetComponent<AIDestinationSetter>();
            AIPathSetter.target = HomePositions[iterationCount];

            iterationCount++; 
        }
    }


    private System.Collections.IEnumerator TimerCoroutine()
    {
        // Coroutine to handle the timer countdown
        while (TimerRunning)
        {
            // Wait for the next frame
            yield return null;
        }
    }
}
