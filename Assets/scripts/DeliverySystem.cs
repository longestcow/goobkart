using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    public ProceduralGeneration progen;
    public Player player;

    public Player.DeliveryRequest DecideDelivery()
    {
        int roadnumber = UnityEngine.Random.Range(Mathf.Max((player.deliveriesqueue[0].road != null ? player.deliveriesqueue[0].road.index : 0) + 1,Mathf.Min(player.latestindex + 10, progen.latestindex - 2)), Mathf.Min(progen.latestindex-2,player.latestindex + 20));
        GameObject[] arr = progen.lastobjects.ToArray();
        GameObject deliveryroad = arr[roadnumber - arr[0].GetComponent<GeneratedRoad>().index];
        checkifmodified:    
        if (deliveryroad.GetComponent<GeneratedRoad>().slope)
        {
            roadnumber++;
            deliveryroad = arr[roadnumber - arr[0].GetComponent<GeneratedRoad>().index];
            goto checkifmodified;
        }
        int housenumber = UnityEngine.Random.Range(0,2);
        if (deliveryroad.GetComponent<GeneratedRoad>().modified) housenumber = 0;
        deliveryroad.GetComponent<GeneratedRoad>().houses[housenumber].transform.GetChild(0).gameObject.SetActive(true);
        Player.DeliveryRequest req;
        req.road = deliveryroad.GetComponent<GeneratedRoad>();
        req.house = deliveryroad.GetComponent<GeneratedRoad>().houses[housenumber];
        req.starttime = Time.time;
        return req;
    }

}
