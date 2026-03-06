using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProceduralGeneration : MonoBehaviour
{
    // Start is called before the first frame update

    public static float width = 5;
    public static float height = 3;
    public static float length = 10;
    float angle;
    float slopeheight;

    public GameObject flatground;
    public int laststate;

    public Queue<GameObject> lastobjects = new Queue<GameObject>();
    public static int RenderDistance;
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
        GameObject obj = Instantiate(flatground, transform.position, Quaternion.Euler(0, 0, 0), null);
        obj.transform.localScale = new Vector3(width, 0.01f, (laststate == 0) ? length : slopeheight);
        lastobjects.Enqueue(obj);
        for (int i = 0; i < RenderDistance-3; i++)
        {
            laststate = GenerateNextThing(laststate);
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
        if (Random.Range(0, 4) < 1)
        {
            //Turn
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
            transform.position += transform.forward * ((width/2) + (length/2));
            GameObject objj = Instantiate(flatground, transform.position, transform.rotation, null);
            objj.transform.localScale = new Vector3(width,0.01f,length);
            lastobjects.Enqueue(objj);
            if (lastobjects.Count > RenderDistance) Destroy(lastobjects.Dequeue());
            return 3;
        }
        else if (Random.Range(0, 10) < 1)
        {
            //Slope Uphill
            laststate = 1;
            transform.position += new Vector3(0, height, 0);
        }
        else if (Random.Range(0, 10) < 1)
        {
            //Slope Downhill
            laststate = 2;
            transform.position -= new Vector3(0, height, 0);
        }
        spawn:
        transform.position += transform.forward * length;
        print(laststate);
        GameObject obj = Instantiate(flatground, transform.position + new Vector3(0, laststate == 2 ? height / 2 : laststate == 1 ? -(height / 2) : 0, 0), Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(laststate == 2 ? angle : laststate == 1 ? -angle : 0, 0, 0)), null);
        obj.transform.localScale = new Vector3(width,0.01f,(laststate == 0)?length:slopeheight);
        lastobjects.Enqueue(obj);
        if(lastobjects.Count > RenderDistance) Destroy(lastobjects.Dequeue());
        return laststate;


    }
}
