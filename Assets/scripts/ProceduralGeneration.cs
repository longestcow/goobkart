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

    public Slider widthslider;
    public Slider heightslider;
    public Slider lengthslider;
    public Slider renderdistanceslider;

    public Text widthtext;
    public Text heighttext;
    public Text lengthtext;
    public Text renderdistancetext;

    void Start()
    {
        widthslider.value = width;
        lengthslider.value = length;
        heightslider.value = height;
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
        width = widthslider.value;
        length = lengthslider.value;
        height = heightslider.value;
        RenderDistance = (int)renderdistanceslider.value;

        widthtext.text = "Width - " + width;
        heighttext.text = "Height - " + height;
        lengthtext.text = "Length - " + length;
        renderdistancetext.text = "Render Distance - " + RenderDistance;
    }

    public int GenerateNextThing(int laststate)
    {
        if (laststate == 1 || laststate == 2 || laststate == 3)
        {
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
                objj.layer = 0;
                lastobjects.Enqueue(objj);
                if (Random.Range(0, 2) < 1)
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
                    Destroy(objj.transform.GetChild(3).GetChild(1).gameObject);
                    objj.GetComponent<Renderer>().material = turnmat1;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + -90, 0);
                    Destroy(objj.transform.GetChild(3).GetChild(0).gameObject);
                    objj.GetComponent<Renderer>().material = turnmat2;
                }
                Destroy(objj.transform.GetChild(2).gameObject);
                Destroy(objj.transform.GetChild(1).gameObject);
                if (lastobjects.Count > RenderDistance) Destroy(lastobjects.Dequeue());
                transform.position += transform.forward * ((width - length) / 2);
                return 3;
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
                objj.layer = 0;
                lastobjects.Enqueue(objj);
                Destroy(objj.transform.GetChild(4).gameObject);
                if (transform.rotation.eulerAngles.y < 170)
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
                    Destroy(objj.transform.GetChild(3).GetChild(1).gameObject);
                    objj.GetComponent<Renderer>().material = turnmat1;
                }
                else if (transform.rotation.eulerAngles.y > 190)
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 90, 0);
                    Destroy(objj.transform.GetChild(3).GetChild(0).gameObject);
                    objj.GetComponent<Renderer>().material = turnmat2;
                }
                Destroy(objj.transform.GetChild(2).gameObject);
                Destroy(objj.transform.GetChild(1).gameObject);
                if (lastobjects.Count > RenderDistance) Destroy(lastobjects.Dequeue());
                transform.position += transform.forward * ((width - length) / 2);
                return 3;
            }
        }
        spawn:
        transform.position += transform.forward * length;
        print(laststate);
        GameObject obj = Instantiate(flatground, transform.position + new Vector3(0, laststate == 2 ? height / 2 : laststate == 1 ? -(height / 2) : 0, 0), Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(laststate == 2 ? angle : laststate == 1 ? -angle : 0, 0, 0)), null);
        obj.transform.localScale = new Vector3(width,0.01f,(laststate == 0)?length:slopeheight);
        if (!(transform.rotation.eulerAngles.y > 170 && transform.rotation.eulerAngles.y < 190))
        {
            Destroy(obj.transform.GetChild(4).gameObject);
        }
        Destroy(obj.transform.GetChild(3).gameObject);
        if (laststate != 0)
        {
            Destroy(obj.transform.GetChild(2).gameObject);
            Destroy(obj.transform.GetChild(1).gameObject);
        }
        else
        {
            if (Random.Range(0, 5) < 4)
            {
                if (Random.Range(0, 2) < 1)
                {
                    Instantiate(cars[Random.Range(0, 4)], transform.position + (transform.right * width / 4f), transform.rotation, null);
                }
                else
                {
                    Instantiate(cars[Random.Range(0, 4)], transform.position - (transform.right * width / 4f), transform.rotation, null).GetComponent<Vehicle>().backwards = true;
                }
            }
        }
        lastobjects.Enqueue(obj);
        if(lastobjects.Count > RenderDistance) Destroy(lastobjects.Dequeue());
        return laststate;


    }
}
