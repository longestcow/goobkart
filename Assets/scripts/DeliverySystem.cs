using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeliverySystem : MonoBehaviour
{
    public ProceduralGeneration progen;
    public Player player;
    public GameObject NotifTemp;
    public Transform canvas;
    public Transform notifspawnspot;


    public Player.DeliveryRequest DecideDelivery()
    {
        int roadnumber = UnityEngine.Random.Range(Mathf.Max((player.deliveriesqueue[0].road != null ? player.deliveriesqueue[0].road.index : 0) + 1,Mathf.Min(player.latestindex + 7, progen.latestindex - 2)), Mathf.Min(progen.latestindex-2,player.latestindex + 13));
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
        print(deliveryroad.GetComponent<GeneratedRoad>().index);
        if (deliveryroad.GetComponent<GeneratedRoad>().houses[0] == null) goto sendnullreq;
        deliveryroad.GetComponent<GeneratedRoad>().houses[housenumber].transform.GetChild(0).gameObject.SetActive(true);
        Player.DeliveryRequest req;
        req.road = deliveryroad.GetComponent<GeneratedRoad>();
        req.house = deliveryroad.GetComponent<GeneratedRoad>().houses[housenumber];
        req.starttime = Time.time;


        int timelimit = (int)((55f / 2f) * (3.2f - player.difficulty));
        int distlimit = (int)(50 * (4 - player.difficulty)) - 25;

        req.timelimit = timelimit;
        req.distlimit = distlimit;

        GameObject notif = Instantiate(NotifTemp,notifspawnspot.position,Quaternion.Euler(Vector3.zero),canvas);
        DeliveryNotification notifproperties = notif.GetComponent<DeliveryNotification>();
        req.notif = notifproperties;
        notifproperties.req = req;
        notifproperties.player = player;

        return req;

        sendnullreq:
        Player.DeliveryRequest nullreq;
        nullreq.house = null;
        nullreq.road = null;
        nullreq.starttime = 0;
        nullreq.timelimit = 0;
        nullreq.distlimit = 0;
        nullreq.notif = null;

        return nullreq;
    }

}
