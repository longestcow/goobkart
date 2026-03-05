using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
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



    [Header("Everything else")]
    public GameObject mesh;
    public GameObject parentobject;
    public GameObject fakecam;
    public GameObject cam;
    public LayerMask groundLayer;
    public Text Speedometer;
    public Transform wheel1;
    public Transform wheel2;
    public Transform frontthing;
    public Transform cyclemain;
    
    SphereCollider coll;
    Rigidbody rb;
    float input = 0f;
    float rotinput = 0f;
    float driftdir;
    bool driftstart;
    bool drifting;
    Quaternion rot;
    bool isgrounded;
    void Start()
    {
        coll = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        mesh.transform.parent = parentobject.transform;
        rb.mass = weight;
    }
    void FixedUpdate()
    {
        rb.AddForce(mesh.transform.forward * input, ForceMode.Force);
        wheel2.localRotation = Quaternion.Euler(0,0,wheel2.localRotation.eulerAngles.z + rb.velocity.magnitude);
        wheel1.localRotation = Quaternion.Euler(0,0,wheel1.localRotation.eulerAngles.z + rb.velocity.magnitude);
        frontthing.transform.localRotation = Quaternion.Lerp(frontthing.transform.localRotation,Quaternion.Euler(0,rotinput*100,0),0.1f);
        cyclemain.localRotation = Quaternion.Euler(0.3f * ((Vector3.SignedAngle(mesh.transform.forward,rb.velocity,mesh.transform.up))),270,0);
    }

    private void LateUpdate()
    {

        cam.transform.position = mesh.transform.position;
        fakecam.transform.position = mesh.transform.position;
        if (rb.velocity.magnitude > 0.1f) fakecam.transform.rotation = Quaternion.LookRotation(rb.velocity);// + new Vector3(0, input * 20f, 0);
        if (rb.velocity.magnitude > 0.1f) fakecam.transform.rotation = Quaternion.Euler(0, fakecam.transform.rotation.eulerAngles.y + driftdir * 45, 0);// + new Vector3(0, input * 20f, 0);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, fakecam.transform.rotation, 0.05f);

    }

    private void Update()
    {
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
    }


    void AlignKart(){
        Ray ray = new Ray(mesh.transform.position, -mesh.transform.up);
        RaycastHit info;

        isgrounded = Physics.Raycast(ray, out info, groundLayer);

        if (Physics.Raycast(ray, out info, 2f, groundLayer))
        {
            // mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation, 
            mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation,Quaternion.FromToRotation(mesh.transform.up, info.normal) * mesh.transform.rotation,0.01f);


            // Time.deltaTime * 10f);
            //rot = Quaternion.FromToRotation(Vector3.up, info.normal);
            //rot.z = mesh.transform.rotation.z;
            //rot.y = mesh.transform.rotation.y;
            //mesh.transform.rotation = rot;
        }
    }
}
