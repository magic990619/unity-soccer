using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomshow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] objecttoshow;

    public bool isGround = false;
    public Renderer Ground;
    public Material[] groundMat;
    private void OnEnable()
    {
        if (isGround)
        {
            ApplyRandomMat();
        }
        else
        {
            showrandom();
        }
    }

    private void OnDisable()
    {
        
    }

    void showrandom()
    {
        for (int i = 0; i < objecttoshow.Length; i++)
        {
            objecttoshow[i].SetActive(false);
        }
        objecttoshow[Random.Range(0, objecttoshow.Length)].gameObject .SetActive (true);
    }

    void ApplyRandomMat()
    {
        Material newmat = Ground.material;
        newmat = groundMat[Random.Range(0, 1)];
        Ground.material = newmat;
    }
}
