using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHouse : MonoBehaviour
{

    public GameObject[] housesprefabs;
    public GameObject[] nonhousesprefabs;
    public GameObject obj;

    public float chanceofnonhouse;

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position + Vector3.up * 50, -Vector3.up, out hit, 50f, LayerMask.GetMask("HouseCheck")))
        {
            GameObject objtobespawned;
            if (Random.Range(0, 100) < chanceofnonhouse)
            {
                objtobespawned = nonhousesprefabs[Random.Range(0, nonhousesprefabs.Length)];
            }
            else
            {
                objtobespawned = housesprefabs[Random.Range(0, housesprefabs.Length)];
            }
            obj = Instantiate(objtobespawned, transform.position, transform.rotation, null);
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
        else
        {
            transform.parent.GetComponent<GeneratedRoad>().modified = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
