using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProceduralGeneration : MonoBehaviour
{
    // Start is called before the first frame update

    public static float width = 10;
    public static float height = 3;
    public static float length = 10;
    float angle;
    float slopeheight;

    public GameObject player;
    public GameObject flatground;
    public Material turnmat1;
    public Material turnmat2;
    public GameObject[] cars;
    public int laststate;

    public Queue<GameObject> lastobjects = new Queue<GameObject>();
    public static int RenderDistance = 20;
    public int latestindex;

    public bool americanmode;

    public Slider renderdistanceslider;

    public Text renderdistancetext;

    void Start()
    {
        renderdistanceslider.value = RenderDistance;

        angle = (Mathf.Atan2(height,length) * Mathf.Rad2Deg);
        slopeheight = Mathf.Sqrt(height * height + length * length);
        transform.position = new Vector3(0,0,0);
        GameObject obj = Instantiate(flatground, transform.position, Quaternion.Euler(0, 180, 0), null);
        obj.transform.localScale = new Vector3(width, 0.01f, (laststate == 0) ? length : slopeheight);
        lastobjects.Enqueue(obj);
        Destroy(obj.transform.GetChild(3).gameObject);
        for (int i = 0; i < RenderDistance-3; i++)
        {
            laststate = GenerateNextThing(laststate);
            if (i == RenderDistance / 2)
            {
                GenerateNextThing(3);
                player.transform.position = transform.position + new Vector3(0, 0, 0);
                player.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles);
                player.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        RenderDistance = (int)renderdistanceslider.value;

        renderdistancetext.text = "Render Distance - " + RenderDistance;
    }

    public int GenerateNextThing(int laststate)
    {
        int whichonetodelete = 0;
        if (laststate > 0)
        {
            whichonetodelete = laststate;
            laststate = 0;
            goto spawn;
        }
        laststate = 0;
        if (transform.rotation.eulerAngles.y > 170 && transform.rotation.eulerAngles.y < 190) {
            if (Random.Range(0, 4) < 1)
            {
                //Turn
                //transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
                transform.position += transform.forward * (length / 2 + width / 2);
                GameObject objj = Instantiate(flatground, transform.position, transform.rotation, null);
                objj.transform.localScale = new Vector3(width, 0.01f, width);
                objj.layer = 13;
                lastobjects.Enqueue(objj);
                objj.GetComponent<GeneratedRoad>().index = ++latestindex;
                if (Random.Range(0, 2) < 1)
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
                    Destroy(objj.transform.GetChild(3).GetChild(1).gameObject);
                    objj.GetComponent<Renderer>().material = turnmat1;
                    Destroy(objj.transform.GetChild(1).gameObject);
                    objj.transform.GetChild(5).gameObject.SetActive(true);
                    objj.transform.GetChild(5).GetChild(1).gameObject.SetActive(true);
                    objj.transform.GetChild(7).gameObject.SetActive(false);
                    laststate = 3;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + -90, 0);
                    Destroy(objj.transform.GetChild(3).GetChild(0).gameObject);
                    objj.GetComponent<Renderer>().material = turnmat2;
                    Destroy(objj.transform.GetChild(2).gameObject);
                    objj.transform.GetChild(5).gameObject.SetActive(true);
                    objj.transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
                    objj.transform.GetChild(6).gameObject.SetActive(false);
                    laststate = 4;
                }
                if (lastobjects.Count > RenderDistance) Destroy(lastobjects.Dequeue());
                transform.position += transform.forward * ((width - length) / 2);
                return laststate;
            }
            else if (Random.Range(0, 10) < 2)
            {
                //Slope Uphill
                laststate = 1;
                transform.position += new Vector3(0, height, 0);
            }
            else if (Random.Range(0, 10) < 2)
            {
                //Slope Downhill
                laststate = 2;
                transform.position -= new Vector3(0, height, 0);
            } 
        }
        else
        {
            if (Random.Range(1,4) < 3)
            {
                transform.position += transform.forward * (length / 2 + width / 2);
                GameObject objj = Instantiate(flatground, transform.position, transform.rotation, null);
                objj.transform.localScale = new Vector3(width, 0.01f, width);
                objj.layer = 13;
                lastobjects.Enqueue(objj);
                objj.GetComponent<GeneratedRoad>().index = ++latestindex;
                Destroy(objj.transform.GetChild(4).gameObject);
                if (transform.rotation.eulerAngles.y < 170)
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
                    Destroy(objj.transform.GetChild(3).GetChild(1).gameObject);
                    objj.GetComponent<Renderer>().material = turnmat1;
                    Destroy(objj.transform.GetChild(1).gameObject);
                    objj.transform.GetChild(5).gameObject.SetActive(true);
                    objj.transform.GetChild(5).GetChild(1).gameObject.SetActive(true);
                    objj.transform.GetChild(7).gameObject.SetActive(false);
                    laststate = 3;
                }
                else if (transform.rotation.eulerAngles.y > 190)
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 90, 0);
                    Destroy(objj.transform.GetChild(3).GetChild(0).gameObject);
                    objj.GetComponent<Renderer>().material = turnmat2;
                    Destroy(objj.transform.GetChild(2).gameObject);
                    objj.transform.GetChild(5).gameObject.SetActive(true);
                    objj.transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
                    objj.transform.GetChild(6).gameObject.SetActive(false);
                    laststate = 4;
                }
                if (lastobjects.Count > RenderDistance) Destroy(lastobjects.Dequeue());
                transform.position += transform.forward * ((width - length) / 2);
                return 3;
            }
        }
        spawn:
        transform.position += transform.forward * length;
        GameObject obj = Instantiate(flatground, transform.position + new Vector3(0, laststate == 2 ? height / 2 : laststate == 1 ? -(height / 2) : 0, 0), Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(laststate == 2 ? angle : laststate == 1 ? -angle : 0, 0, 0)), null);
        obj.transform.localScale = new Vector3(width,0.01f,(laststate == 0)?length:slopeheight);
        if (!(transform.rotation.eulerAngles.y > 170 && transform.rotation.eulerAngles.y < 190))
        {
            Destroy(obj.transform.GetChild(4).gameObject);
        }
        Destroy(obj.transform.GetChild(3).gameObject);
        if (laststate != 0)
        {
            obj.GetComponent<GeneratedRoad>().slope = true;
            Destroy(obj.transform.GetChild(2).gameObject);
            Destroy(obj.transform.GetChild(1).gameObject);
        }
        else
        {
            if (whichonetodelete == 3)
            {
                //Destroy(obj.transform.GetChild(2).gameObject);
            }
            else if (whichonetodelete == 4)
            {
                //Destroy(obj.transform.GetChild(1).gameObject);
            }
            if (Random.Range(0, 100) < player.GetComponent<Player>().difficulty* 25)
            {
                //Spawn car
                if (Random.Range(0, 2) < 1)
                {
                    Instantiate(cars[Random.Range(0, 4)], transform.position + (americanmode ? -1 : 1) * (transform.right * width / 4f), transform.rotation, null);
                }
                else
                {
                    Instantiate(cars[Random.Range(0, 4)], transform.position - (americanmode ? -1 : 1) * (transform.right * width / 4f), transform.rotation, null).GetComponent<Vehicle>().backwards = true;
                }
            }
        }
        lastobjects.Enqueue(obj);
        obj.GetComponent<GeneratedRoad>().index = ++latestindex;
        if(lastobjects.Count > RenderDistance) Destroy(lastobjects.Dequeue());
        return laststate;


    }
}
