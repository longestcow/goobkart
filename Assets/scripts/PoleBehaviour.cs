using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleBehaviour : MonoBehaviour
{
    public GameObject[] points;
    public BoxCollider col;
    public PoleBehaviour closestpole;
    public bool taken;
    public bool leftside;

    // Start is called before the first frame update
    void Start()
    {
        closestpole = this;
        Collider[] possiblepoles = Physics.OverlapSphere(transform.position, 50, LayerMask.GetMask("Pole"));
        for (int i = 0; i < possiblepoles.Length; i++)
        {
            if (leftside != possiblepoles[i].GetComponent<PoleBehaviour>().leftside) continue;
            if (!possiblepoles[i].gameObject.GetComponent<PoleBehaviour>().taken)
            {
                if (closestpole != this)
                {
                    if (Vector3.Distance(transform.position,possiblepoles[i].transform.position) < Vector3.Distance(transform.position, closestpole.transform.position))
                    {
                        closestpole = possiblepoles[i].gameObject.GetComponent<PoleBehaviour>();
                    }
                }
                else
                {
                    closestpole = possiblepoles[i].gameObject.GetComponent<PoleBehaviour>();
                }
            }
        }
        if (closestpole != this) closestpole.taken = true;
        if (closestpole.taken)
        {
            for (int k = 0; k < 3; k++)
            {
                LineRenderer liner = points[k].transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
                Vector3 A = points[k].transform.position;
                Vector3 C = closestpole.points[k].transform.position;
                Vector3 B = ((points[k].transform.position + closestpole.points[k].transform.position) / 2f) + new Vector3(0, Random.Range(-1f, 0f), 0);
                Vector3[] Pos = new Vector3[6];
                print("A" + A);
                print("B" + B);
                print("C" + C);
                for (int j = 0; j <= 5; j++)
                {
                    Pos[j] = Vector3.Lerp(Vector3.Lerp(A, B, j / 5f), Vector3.Lerp(B, C, j / 5f), j / 5f);
                }
                liner.SetPositions(Pos);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (closestpole == this)
        {
            Collider[] possiblepoles = Physics.OverlapSphere(transform.position, 50, LayerMask.GetMask("Pole"));
            for (int i = 0; i < possiblepoles.Length; i++)
            {
                if (leftside != possiblepoles[i].GetComponent<PoleBehaviour>().leftside) continue;
                if (!possiblepoles[i].gameObject.GetComponent<PoleBehaviour>().taken)
                {
                    if (closestpole != this)
                    {
                        if (Vector3.Distance(transform.position, possiblepoles[i].transform.position) < Vector3.Distance(transform.position, closestpole.transform.position))
                        {
                            closestpole = possiblepoles[i].gameObject.GetComponent<PoleBehaviour>();
                        }
                    }
                    else
                    {
                        closestpole = possiblepoles[i].gameObject.GetComponent<PoleBehaviour>();
                    }
                }
            }
            if (closestpole != this) closestpole.taken = true;
            if (closestpole.taken)
            {
                for (int k = 0; k < 3; k++)
                {
                    LineRenderer liner = points[k].transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
                    Vector3 A = points[k].transform.position;
                    Vector3 C = closestpole.points[k].transform.position;
                    Vector3 B = ((points[k].transform.position + closestpole.points[k].transform.position) / 2f) + new Vector3(0, Random.Range(-1f, 0f), 0);
                    Vector3[] Pos = new Vector3[6];
                    print("A" + A);
                    print("B" + B);
                    print("C" + C);
                    for (int j = 0; j <= 5; j++)
                    {
                        Pos[j] = Vector3.Lerp(Vector3.Lerp(A, B, j / 5f), Vector3.Lerp(B, C, j / 5f), j / 5f);
                    }
                    liner.SetPositions(Pos);
                }
            }
        }
    }
}
