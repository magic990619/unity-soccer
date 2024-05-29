using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkingNet : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Ads;
    void Start()
    {
        if (Application.internetReachability==NetworkReachability.NotReachable)
        {
            for (int i = 0;i< Ads.Length;i++)
            {
                Ads[i ].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < Ads.Length; i++)
            {
                Ads[i].gameObject.SetActive(true);
            }
        }
    }
}
