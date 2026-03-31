using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public static string[] statnames = {"Deliveries", "Max Multiplier", "Tricks Performed", "Mid-Air", "Crazy Precision", "Sniped", "By A Hair", "Less Than a Sec", "Funny Number"};
    public static int[] statcount = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < statcount.Length; i++)
        {
            statcount[i] = 0;
        }
    }
}
