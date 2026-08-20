using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEverything : MonoBehaviour
{
    public Transform PlayerSphere;
    public Transform Everything;
    public Rigidbody playerb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            ShiftOrigin();
        }
    }

    public void ShiftOrigin()
    {
        Vehicle.dontchecknow = true;
        Vector3 delta = PlayerSphere.position;
        Everything.transform.position -= delta;
        playerb.position -= delta;
        StartCoroutine(NextFrameReenableCheck());
    }

    IEnumerator NextFrameReenableCheck()
    {
        yield return null;
        Vehicle.dontchecknow = false;
    }
}
