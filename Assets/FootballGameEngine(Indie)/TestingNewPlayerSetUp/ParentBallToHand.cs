using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentBallToHand : MonoBehaviour
{
    public Transform _ball;
    public Transform _hand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _ball.transform.position = _hand.transform.position;
        _ball.transform.rotation = _hand.transform.rotation;
    }
}
