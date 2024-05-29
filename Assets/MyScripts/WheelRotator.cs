using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotator : MonoBehaviour {



	public Transform [] wheels;
	public float speed;
	public Vector3 direction;

	void Update () {
		for(int a = 0;a<wheels.Length;a++)
			wheels[a].Rotate(direction*speed);
	}
}
