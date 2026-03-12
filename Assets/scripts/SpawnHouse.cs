using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHouse : MonoBehaviour
{

    public GameObject house;

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position + Vector3.up * 50, -Vector3.up, out hit, 50f, LayerMask.GetMask("House")))
        {
            Instantiate(house, transform.position, transform.rotation, null);
        }
        else
        {
            print("I found one");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
