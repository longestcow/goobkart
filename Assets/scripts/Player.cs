using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject mesh;
    public GameObject cam;
    public GameObject directionMarker;
    public LayerMask groundLayer;
    SphereCollider coll;
    Rigidbody rb;
    float input = 0f;
    Quaternion rot;
    public float spe = 10;
    void Start()
    {
        coll = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        //AlignKart();
        input += 0.025f * Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < 0.5) input += (input > -0.1f) ? -0.1f : (input < 0.1f) ? 0.1f : 0f;
        input = Mathf.Clamp(input, -5, 5);
        //mesh.transform.localRotation = Quaternion.Euler(0, input * 20f, 0);
        Debug.Log(rb.velocity.x + ", " + rb.velocity.y + ", " + rb.velocity.z);
        if (rb.velocity.magnitude > 0.1f)mesh.transform.rotation = Quaternion.LookRotation(rb.velocity);// + new Vector3(0, input * 20f, 0);
        if (rb.velocity.magnitude > 0.1f)mesh.transform.rotation = Quaternion.Euler(0, mesh.transform.rotation.eulerAngles.y + input * 20f, 0);// + new Vector3(0, input * 20f, 0);
        if (rb.velocity.magnitude > 0.1f)cam.transform.rotation = Quaternion.LookRotation(rb.velocity);// + new Vector3(0, input * 20f, 0);
        if (rb.velocity.magnitude > 0.1f)cam.transform.rotation = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0);// + new Vector3(0, input * 20f, 0);
        //mesh.transform.rotation = Quaternion.Euler(0, mesh.transform.rotation.eulerAngles.y, 0);
        if (Input.GetButton("Fire1"))
        {
            rb.AddForce(mesh.transform.forward * spe);
        }
        if (Input.GetButton("Fire2"))
        {
            rb.AddForce(-mesh.transform.forward * spe);
        }
    }


    void AlignKart(){
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit info;

        if (Physics.Raycast(ray, out info, 2f, groundLayer))
        {
            // mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation, 
            // Quaternion.FromToRotation(Vector3.up, info.normal),
            // Time.deltaTime * 10f);
            rot = Quaternion.FromToRotation(Vector3.up, info.normal);
            rot.z = mesh.transform.rotation.z;
            rot.y = mesh.transform.rotation.y;
            mesh.transform.rotation = rot;
        }
    }
}
