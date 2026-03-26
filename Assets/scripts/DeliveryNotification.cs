using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryNotification : MonoBehaviour
{

    public Text timeleft;
    public Text distance;
    public Player player;
    public Player.DeliveryRequest req;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //timeleft.text = (req.timelimit - (Time.time - req.starttime)).ToString("F2") + "s";
        //distance.text = (Vector3.Distance(req.house.transform.position, player.transform.position)/10).ToString("F1");
    }

    private void FixedUpdate()
    {
        
    }
}
