using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHouse : MonoBehaviour
{

    public GameObject house;
    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position + Vector3.up * 50, -Vector3.up, out hit, 50f, LayerMask.GetMask("HouseCheck")))
        {
            obj = Instantiate(house, transform.position, transform.rotation, null);
            obj.transform.parent = this.transform.parent;
            if (obj.transform.parent.GetComponent<GeneratedRoad>().houses[0] == null)
            {
                obj.transform.parent.GetComponent<GeneratedRoad>().houses[0] = obj;
            }
            else
            {
                obj.transform.parent.GetComponent<GeneratedRoad>().houses[1] = obj;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
