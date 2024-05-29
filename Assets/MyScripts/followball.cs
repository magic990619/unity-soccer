using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followball : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Football;
    public float moveSpeed;

    void Update()
    {
        gameObject.transform.position = new Vector3 (Football.transform.position.x , gameObject .transform.position.y, Football.transform.position.z);

    }
}
