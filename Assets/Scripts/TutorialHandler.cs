using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHandler : MonoBehaviour
{
    public GameObject startingDialogue;
    public GameObject walkingEnemyPrefab;
    bool spawnedEnemies = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // when this happens, it is time to spawn the opening enemies
        if(startingDialogue == null && !spawnedEnemies)
        {
            spawnedEnemies = true;
            GameObject newEnemy = Instantiate(walkingEnemyPrefab, transform);
            newEnemy.transform.position = newEnemy.transform.parent.position;
            newEnemy.GetComponent<WalkingEnemy>().dirX = -1;
            newEnemy.GetComponent<WalkingEnemy>().Flip();
            //Destroy(gameObject);
        }
    }
}
