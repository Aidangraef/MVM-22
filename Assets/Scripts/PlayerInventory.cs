using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> inventory;
    [SerializeField] private GameObject blackHole;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("v"))
        {
            //Instantiate(inventory[0], blackHole.transform.position, Quaternion.identity);
            inventory[0].SetActive(true);
            inventory[0].transform.position = blackHole.transform.position;
        }
    }
}
