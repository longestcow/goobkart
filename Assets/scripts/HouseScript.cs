using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour
{
    public GameObject GreenDelivery; //Nothing to do with delivery but this is what will show up on the minimap
    public GameObject YellowDelivery;
    public GameObject RedDelivery;
    public GameObject HouseModel;
    public bool terrainhouse;
    public bool slope;

    // Start is called before the first frame update
    void Start()
    {
        if (terrainhouse)
        {
            HouseModel.transform.rotation = Quaternion.Euler(new Vector3(0, HouseModel.transform.rotation.eulerAngles.y, 0));
            GreenDelivery.SetActive(false);
            gameObject.tag = "Terrain";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
