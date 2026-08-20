using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public Rigidbody self;
    Quaternion idealrotation;
    public static bool dontchecknow;
    public bool backwards;
    bool pushing;
    Rigidbody playerb;
    public AudioSource metalhit;

    // Start is called before the first frame update
    void Start()
    {
        dontchecknow = false;
        self = gameObject.GetComponent<Rigidbody>();
        pushing = false;
        //transform.localScale = new Vector3(0.02890174f, 50f, 0.05f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        if (!dontchecknow)
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 3f, 0), -Vector3.up, out hit, 5, LayerMask.GetMask("ground", "TurnL", "TurnR", "groundb")))
            {
                if (hit.collider.gameObject.layer == 6)
                {
                    idealrotation = Quaternion.Euler(new Vector3((backwards ? -1 : 1) * hit.collider.gameObject.transform.rotation.eulerAngles.z, hit.collider.gameObject.transform.rotation.eulerAngles.y, (backwards ? -1 : 1) * hit.collider.gameObject.transform.rotation.eulerAngles.x) + new Vector3(0, backwards ? 270 : 90, 0)); //CHANGE THIS IF ANGLE EVER CHANGES
                }
                else if (hit.collider.gameObject.layer == 10)
                {
                    idealrotation = Quaternion.Euler(new Vector3(hit.collider.gameObject.transform.rotation.eulerAngles.z, hit.collider.gameObject.transform.rotation.eulerAngles.y, hit.collider.gameObject.transform.rotation.eulerAngles.x) + new Vector3(0, backwards ? 135 : 45, 0));
                }
                else if (hit.collider.gameObject.layer == 11)
                {
                    idealrotation = Quaternion.Euler(new Vector3(hit.collider.gameObject.transform.rotation.eulerAngles.z, hit.collider.gameObject.transform.rotation.eulerAngles.y, hit.collider.gameObject.transform.rotation.eulerAngles.x) + new Vector3(0, backwards ? 45 : 135, 0));
                }
                transform.localRotation = Quaternion.Lerp(transform.localRotation, idealrotation, 0.2f);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (pushing)
        {
            if ((transform.position - playerb.transform.position).normalized.y > 0f)
            {
                playerb.velocity = new Vector3(playerb.velocity.x, (-10 * (transform.position - playerb.transform.position).normalized.y), playerb.velocity.z);
            }
            else
            {
                playerb.velocity = (-10 * (transform.position - playerb.transform.position).normalized);
            }
        }
    }

    private void Update()
    {
        if (Player.deliverycam != 0)
        {
            return;
        }
        transform.position += (idealrotation * Vector3.right) * 60 * 0.1f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 17)
        {
            print("collision");
            playerb = other.transform.parent.gameObject.GetComponent<Rigidbody>();
            pushing = true;
            metalhit.pitch = Random.Range(9.9f,1.1f);
            metalhit.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 17)
        pushing = false;
    }
}
