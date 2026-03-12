using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public Rigidbody self;
    Quaternion idealrotation;
    public bool backwards;

    // Start is called before the first frame update
    void Start()
    {
        self = gameObject.GetComponent<Rigidbody>();
        //transform.localScale = new Vector3(0.02890174f, 50f, 0.05f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0,3f,0), -Vector3.up, out hit, 5, LayerMask.GetMask("ground","TurnL","TurnR","groundb")))
        {
            if (hit.collider.gameObject.layer == 6)
            {
                idealrotation = Quaternion.Euler(new Vector3((backwards ? -1 : 1) * hit.collider.gameObject.transform.rotation.eulerAngles.z, hit.collider.gameObject.transform.rotation.eulerAngles.y,(backwards?-1:1)* hit.collider.gameObject.transform.rotation.eulerAngles.x) + new Vector3(0, backwards?270:90, 0)); //CHANGE THIS IF ANGLE EVER CHANGES
            }
            else if (hit.collider.gameObject.layer == 10)
            {
                idealrotation = Quaternion.Euler(new Vector3(hit.collider.gameObject.transform.rotation.eulerAngles.z, hit.collider.gameObject.transform.rotation.eulerAngles.y, hit.collider.gameObject.transform.rotation.eulerAngles.x) + new Vector3(0, backwards?135:45, 0));
            }
            else if (hit.collider.gameObject.layer == 11)
            {
                idealrotation = Quaternion.Euler(new Vector3(hit.collider.gameObject.transform.rotation.eulerAngles.z, hit.collider.gameObject.transform.rotation.eulerAngles.y, hit.collider.gameObject.transform.rotation.eulerAngles.x) + new Vector3(0, backwards?45:135, 0));
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.position += 15f * (idealrotation * Vector3.right) * Time.deltaTime;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, idealrotation, 10 * Time.deltaTime);
    }
}
