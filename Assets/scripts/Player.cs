using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Kart Stats")]
    public float spe = 10; //forward speed (10 is default)
    public float eps = -5; //reverse speed (-5 is default)
    public float rotspeed = 20; //speed of turning (20 is default)
    public float weight = 1; //weight of kart (1 is default)
    public float stylemultiplier = 1; //amount of style points earnt per style thing (not implemented yet)
    public float boostmultiplier = 1; //the amount of boost the kart gets from boosting on boost panels (not implemented yet)

    [Header("Crayon Stats")]
    public float shootspeed;
    public float shootstrengthchargeupspeed;
    public float deliverycamoffset;



    [Header("Everything else")]
    public GameObject mesh;
    public GameObject parentobject;
    public GameObject fakecam; //camera that moves based on velocity
    public GameObject cam; //camera that lerps to movement based on velocity
    public GameObject maincamera;
    public GameObject campos1;
    public GameObject campos2;
    public GameObject campos3;
    public GameObject camposdelivery;
    public GameObject mousecam; //camera that moves based on mouse movement
    public LayerMask groundLayer;
    public Text Speedometer;
    public Transform wheel1;
    public Transform wheel2;
    public Transform frontthing;
    public Transform cyclemain;
    public GameObject crosshair;
    public GameObject delivery;
    public ProceduralGeneration proceduralgen;
    public Scrollbar strengthscrollbar;
    public DeliverySystem deliverysys;
    public Text DeliveryQueueText;
    public LineRenderer liner;
    public Transform linepoint0;
    public Transform linepoint1;
    public GameObject linelengthcanvas;
    public Text linelengthtext;
    
    SphereCollider coll;
    Rigidbody rb;
    float input = 0f;
    float rotinput = 0f;
    float driftdir;
    bool driftstart;
    bool drifting;
    Quaternion rot;
    bool isgrounded;
    bool aimmode;
    bool aimodebuffer;
    float shootstrength;
    bool shootstrengthincreasing;
    public static int deliverycam;
    [NonSerialized]
    public int latestindex;
    GameObject latestdelivery;
    Rigidbody latestdeliveryrb;
    Vector3 savedvelocity;
    public struct DeliveryRequest
    {
        public GeneratedRoad road;
        public GameObject house;
        public float starttime;
    }
    public DeliveryRequest[] deliveriesqueue = new DeliveryRequest[5];
    public int currentdelivery;
    public GameObject currentdeliveryhouse;
    public DeliveryRequest currentdeliveryreq;
    int queueddeliveries;
    float aspectratio;
    float tanfov;
    void Start()
    {
        liner.positionCount = 2;
        liner.enabled = false;
        coll = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        mesh.transform.parent = parentobject.transform;
        rb.mass = weight;
        rb.mass = weight;
        for (int i = 0; i < deliveriesqueue.Length; i++)
        {
            deliveriesqueue[i].road = null;
            deliveriesqueue[i].house = null;
        }
        deliverycam = 0;
        currentdelivery = 0;
        queueddeliveries = 0;

        aspectratio = Screen.width / Screen.height;
        tanfov = Mathf.Tan(Mathf.Deg2Rad *  maincamera.GetComponent<Camera>().fieldOfView/2f);
    }
    void FixedUpdate()
    {
        if (deliverycam != 0)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        rb.AddForce(mesh.transform.forward * input, ForceMode.Force);
        wheel2.localRotation = Quaternion.Euler(0,0,wheel2.localRotation.eulerAngles.z + rb.velocity.magnitude);
        wheel1.localRotation = Quaternion.Euler(0,0,wheel1.localRotation.eulerAngles.z + rb.velocity.magnitude);
        frontthing.transform.localRotation = Quaternion.Lerp(frontthing.transform.localRotation,Quaternion.Euler(0,rotinput*100,0),0.1f * 25 * Time.deltaTime);
        if (Vector3.Dot(rb.velocity,mesh.transform.forward) > 0.1f)
        {
            cyclemain.localRotation = Quaternion.Euler(0.3f * ((Vector3.SignedAngle(mesh.transform.forward, rb.velocity, mesh.transform.up))), 270, 0);
        }
    }

    private void LateUpdate()
    {
        cam.transform.position = mesh.transform.position;
        fakecam.transform.position = mesh.transform.position;
        //mousecam.transform.position = mesh.transform.position;
        if (rb.velocity.magnitude > 0.1f) fakecam.transform.rotation = Quaternion.LookRotation(rb.velocity);// + new Vector3(0, input * 20f, 0);
        if (rb.velocity.magnitude > 0.1f) fakecam.transform.rotation = Quaternion.Euler(0, fakecam.transform.rotation.eulerAngles.y + driftdir * 45, 0);// + new Vector3(0, input * 20f, 0);
        cam.transform.rotation = Quaternion.Euler( Quaternion.Lerp(cam.transform.rotation, fakecam.transform.rotation, 0.05f).eulerAngles + (!aimmode && (deliverycam == 0)?new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0): Vector3.zero));
        //mousecam.transform.rotation = Quaternion.Euler(mousecam.transform.rotation.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X"), 0));
        print(1f / Time.deltaTime);
        if (!aimmode && deliverycam == 0)
        {
            maincamera.transform.position = Vector3.Lerp(maincamera.transform.position, campos1.transform.position, 0.05f * 250 * Time.deltaTime);
            maincamera.transform.rotation = Quaternion.Lerp(maincamera.transform.rotation, campos1.transform.rotation, 0.05f * 250 * Time.deltaTime);
        }
        else if (deliverycam == 0)
        {
            maincamera.transform.position = Vector3.Lerp(maincamera.transform.position, (campos2.transform.localRotation.eulerAngles.y  > 180?campos2:campos2).transform.position, 0.05f * 4);
            maincamera.transform.rotation = Quaternion.Lerp(maincamera.transform.rotation, (campos2.transform.localRotation.eulerAngles.y > 180 ? campos2 : campos2).transform.rotation, 0.05f * 4);
            campos2.transform.localRotation = Quaternion.Euler(campos2.transform.localRotation.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0));
            campos3.transform.localRotation = Quaternion.Euler(campos2.transform.localRotation.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0));
        }
        else 
        {
            if (deliverycam == 1)
            {
                camposdelivery.transform.position = latestdelivery.transform.position + (transform.position - latestdelivery.transform.position).normalized * deliverycamoffset;
                camposdelivery.transform.LookAt(latestdelivery.transform.position);

                if (latestdeliveryrb.velocity.magnitude < 0.1f)
                {
                    StartCoroutine("resultcam");
                    StopCoroutine("deliverycamtimeout");
                }
            }
            if (deliverycam == 2)
            {
                float offset = ((currentdeliveryhouse.transform.GetChild(0).position - latestdelivery.transform.position).magnitude / 2f / aspectratio) / tanfov;
                Vector3 midpoint = (currentdeliveryhouse.transform.GetChild(0).position + latestdelivery.transform.position) / 2f;
                camposdelivery.transform.position =  midpoint + (transform.position - latestdelivery.transform.position).normalized * 2 * (offset + deliverycamoffset);
                camposdelivery.transform.position = new Vector3(camposdelivery.transform.position.x, latestdelivery.transform.position.y + 1, camposdelivery.transform.position.z);
                //camposdelivery.transform.LookAt(midpoint);
                linepoint0.position = Vector3.Lerp(linepoint0.position, latestdelivery.transform.position, Time.deltaTime * 10);
                linepoint1.position = Vector3.Lerp(linepoint1.position, currentdeliveryhouse.transform.GetChild(0).transform.position, Time.deltaTime * 10);
                liner.SetPosition(0, linepoint0.transform.position);
                liner.SetPosition(1, linepoint1.transform.position);
                linelengthcanvas.transform.position = ((currentdeliveryhouse.transform.GetChild(0).position + latestdelivery.transform.position) / 2f) + Vector3.up;
                linelengthcanvas.transform.LookAt(camposdelivery.transform.position);
                linelengthtext.text = ((int)((linepoint0.position - linepoint1.position).magnitude * 100)).ToString();
            }
            maincamera.transform.position = Vector3.Lerp(maincamera.transform.position, camposdelivery.transform.position, 0.05f * 250 * Time.deltaTime);
            maincamera.transform.rotation = Quaternion.Lerp(maincamera.transform.rotation, camposdelivery.transform.rotation, 0.05f * 250 * Time.deltaTime);
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  //Restart code


        if (deliverycam != 0)
        {
            return;
        }

        AlignKart();

        if (Input.GetButton("Drift") && !drifting && isgrounded && rb.velocity.magnitude > 0.5f)
        {
            driftstart = true;
        }
        if (driftstart)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.1f)
            {
                driftdir = 1;
                driftstart = false;
                drifting = true;
            }
            if (Input.GetAxisRaw("Horizontal") < -0.1f)
            {
                driftdir = -1;
                driftstart = false;
                drifting = true;
            }
            
        }
        if (!Input.GetButton("Drift"))
        {
            driftdir = 0;
            drifting = false;
            driftstart = false;
        }

        rotinput = (driftdir + Input.GetAxisRaw("Horizontal"))/2f;
        if (!isgrounded) rotinput *= 0.3f;

        mesh.transform.Rotate(0, rotinput * Time.deltaTime * rotspeed * rb.velocity.magnitude ,0, Space.World);


        mesh.transform.position = this.transform.position;

        if (Input.GetButton("Fire1"))
        {
            input = spe;
        }
        else if (Input.GetButton("Fire2"))
        {
            input = eps;
        }
        else
        {
            input = 0;
        }

        Speedometer.text = ((int)(20f * rb.velocity.magnitude)).ToString();

        //Aim Mode 
        if (Input.GetButtonDown("Aim"))
        {
            Time.timeScale = 0.1f;
            aimmode = true;
            bool leftside = true;
            if (currentdelivery != 0)
            {
                leftside = !(Vector3.SignedAngle(mesh.transform.forward, currentdeliveryhouse.transform.position - transform.position, Vector3.up) > 0);
            }
            campos2.transform.LookAt(currentdeliveryhouse.transform.GetChild(0).position + Vector3.up * 2);
            //campos3.transform.localRotation = Quaternion.Euler(0, 180 + Vector3.SignedAngle(maincamera.transform.forward, currentdeliveryhouse.transform.position - transform.position, Vector3.up), 0);
            crosshair.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetButtonUp("Aim"))
        {
            aimmode = false;
            crosshair.SetActive(false);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            shootstrengthincreasing = false;
            strengthscrollbar.gameObject.SetActive(false);
        }

        if (aimmode && Input.GetButtonDown("Shoot"))
        {
            shootstrengthincreasing = true;
            shootstrength = 1;
            strengthscrollbar.gameObject.SetActive(true);
        }
        if (aimmode && shootstrengthincreasing)
        {
            shootstrength = Mathf.Clamp(shootstrength+(shootstrengthchargeupspeed * Time.deltaTime),1,20);
            strengthscrollbar.size = (shootstrength-1)/19;
        }
        if (aimmode && shootstrengthincreasing && Input.GetButtonUp("Shoot"))
        {
            shootstrengthincreasing = false;
            latestdelivery = Instantiate(delivery, mesh.transform.position, Quaternion.Euler(Vector3.zero), null);
            latestdeliveryrb = latestdelivery.GetComponent<Rigidbody>();
            latestdeliveryrb.velocity = maincamera.transform.forward * shootspeed * shootstrength;
            aimmode = false;
            deliverycam = 1;
            StartCoroutine("deliverycamtimeout");
            crosshair.SetActive(false);
            savedvelocity = rb.velocity;
            Time.timeScale = 1;
            strengthscrollbar.gameObject.SetActive(false);
        }
    }


    void AlignKart(){
        Ray ray = new Ray(mesh.transform.position, -mesh.transform.up);
        RaycastHit info;

        isgrounded = Physics.Raycast(ray, out info, groundLayer);

        if (Physics.Raycast(ray, out info, 2f, groundLayer))
        {
            // mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation, 
            mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation,Quaternion.FromToRotation(mesh.transform.up, info.normal) * mesh.transform.rotation,0.01f * 250 * Time.deltaTime);


            // Time.deltaTime * 10f);
            //rot = Quaternion.FromToRotation(Vector3.up, info.normal);
            //rot.z = mesh.transform.rotation.z;
            //rot.y = mesh.transform.rotation.y;
            //mesh.transform.rotation = rot;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            proceduralgen.laststate = proceduralgen.GenerateNextThing(proceduralgen.laststate);
            latestindex = other.transform.parent.gameObject.GetComponent<GeneratedRoad>().index;
            Destroy(other.gameObject);
            if (latestindex > currentdelivery + 3)
            {
                UpdateDeliveryQueue(currentdeliveryreq, false);
            }
            if (UnityEngine.Random.Range(0,30) < 15 && queueddeliveries < deliveriesqueue.Length && Time.timeSinceLevelLoad > 1f)
            {
                UpdateDeliveryQueue(deliverysys.DecideDelivery(),true);
            }
        }
    }

    void UpdateDeliveryQueue(DeliveryRequest req, bool enqueue)
    {
        if (req.road == null) return;
        GeneratedRoad road = req.road;
        GameObject house = req.house;
        int roadnumber = road.index;
        if (enqueue)
        {
            for (int i = deliveriesqueue.Length-1; i > 0; i--)
            {
                deliveriesqueue[i] = deliveriesqueue[i - 1];
            }
            deliveriesqueue[0] = req;
            queueddeliveries++;
        }
        else
        {
            for (int i = deliveriesqueue.Length - 1; i >= 0; i--)
            {
                if ((deliveriesqueue[i].road != null ? deliveriesqueue[i].road.index : 0) == roadnumber)
                {
                    for (int j = deliveriesqueue.Length-2; j >= i; j--)
                    {
                        deliveriesqueue[i] = deliveriesqueue[i + 1];
                    }
                    deliveriesqueue[deliveriesqueue.Length - 1].road = null;
                }
            }
            queueddeliveries--;
        }
        for (int i = deliveriesqueue.Length-1; i >= 0; i--)
        {
            if (deliveriesqueue[i].road != null)
            {
                currentdelivery = deliveriesqueue[i].road.index;
                currentdeliveryhouse = deliveriesqueue[i].house;
                currentdeliveryreq = deliveriesqueue[i];
                currentdeliveryhouse.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                currentdeliveryhouse.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                goto updatedeliverytext;
            }
        }
        currentdelivery = 0;
        currentdeliveryhouse = null;
        DeliveryRequest nullreq;
        nullreq.house = null;
        nullreq.road = null;
        nullreq.starttime = 0;
        currentdeliveryreq = nullreq;

    updatedeliverytext:;
    }

    IEnumerator deliverycamtimeout()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine("resultcam");
    }

    IEnumerator resultcam()
    {
        if (currentdelivery != 0)
        {
            deliverycam = 2;
            yield return new WaitForSeconds(0.2f);
            liner.enabled = true;
            linelengthtext.gameObject.SetActive(true);
            linepoint0.position = (currentdeliveryhouse.transform.GetChild(0).position + latestdelivery.transform.position) / 2f;
            linepoint1.position = (currentdeliveryhouse.transform.GetChild(0).position + latestdelivery.transform.position) / 2f;
            linelengthcanvas.transform.position = (currentdeliveryhouse.transform.GetChild(0).position + latestdelivery.transform.position) / 2f;
            yield return new WaitForSeconds(1);
            liner.enabled = false;
            linelengthtext.gameObject.SetActive(false);
            linelengthtext.text = "";
        }
        deliverycam = 0;
        rb.velocity = savedvelocity;
        if (currentdelivery != 0)
        {
            currentdeliveryhouse.transform.GetChild(0).gameObject.SetActive(false);
            UpdateDeliveryQueue(currentdeliveryreq, false);
        }
    }
}
