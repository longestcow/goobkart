using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHouse : MonoBehaviour
{

    public GameObject[] housesprefabs;
    public GameObject[] nonhousesprefabs;
    public GameObject obj;
    public bool unimportanthouse;
    public bool terrain;

    public float chanceofnonhouse;

    // Start is called before the first frame update
    void Start()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position + Vector3.up * 50, -Vector3.up, out hit, 50f, LayerMask.GetMask("HouseCheck")))
        {
            spawn();   
        }
        else
        {
            if (terrain)
            {
                Destroy(this.gameObject);
            }
            else if (hit.collider.transform.parent.tag == "Terrain")
            {
                Destroy(hit.collider.transform.parent.gameObject);
                spawn();
            }
            if (!unimportanthouse)
                if (!transform.parent.GetComponent<GeneratedRoad>().modified)
                {
                    transform.parent.GetComponent<GeneratedRoad>().modified = true;
                }
                else
                {
                    transform.parent.GetComponent<GeneratedRoad>().slope = true;
                }
        }
    }

    void spawn()
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
        if (terrain) obj.GetComponent<HouseScript>().terrainhouse = true;
        if (!unimportanthouse)
        {
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

}
