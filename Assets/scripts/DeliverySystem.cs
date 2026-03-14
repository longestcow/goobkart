using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySystem : MonoBehaviour
{
    public ProceduralGeneration progen;
    public Player player;

    public int DecideDelivery()
    {
        int roadnumber = UnityEngine.Random.Range(Mathf.Max(player.deliveriesqueue[0] + 1,player.latestindex + 1), Mathf.Min(progen.latestindex-2,player.latestindex + 10));
        GameObject[] arr = progen.lastobjects.ToArray();
        GameObject deliveryroad = arr[roadnumber - arr[0].GetComponent<GeneratedRoad>().index];
        print(roadnumber);
        if (deliveryroad.GetComponent<GeneratedRoad>().slope)
        {
            roadnumber++;
            deliveryroad = arr[roadnumber - arr[0].GetComponent<GeneratedRoad>().index];
        }
        int housenumber = UnityEngine.Random.Range(0,2);
        deliveryroad.GetComponent<GeneratedRoad>().houses[housenumber].transform.GetChild(0).gameObject.SetActive(true);
        return roadnumber;
    }

}
