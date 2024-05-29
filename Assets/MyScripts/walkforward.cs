using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkforward : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 0;

    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.0f, 0.0f, speed * Time.deltaTime);
    }
}
