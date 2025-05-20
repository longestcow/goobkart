using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject mesh;
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
        AlignKart();
        input = Input.GetAxis("Horizontal");
        rot = mesh.transform.rotation;
        rot.y = input*0.5f;
        mesh.transform.rotation = rot;
        if (Input.GetButton("Fire1"))
        {
            print("shoot" + input);
            rb.AddForce(-mesh.transform.forward * spe, ForceMode.Acceleration);
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
