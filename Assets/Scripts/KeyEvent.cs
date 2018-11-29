using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEvent : MonoBehaviour {
    Rigidbody rb;
    public float magnitude = 200;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddRelativeTorque(magnitude, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rb.AddRelativeTorque(0, magnitude, 0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.AddRelativeTorque(-magnitude, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rb.AddRelativeTorque(0, -magnitude, 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            magnitude *= 2;
            print("magnitude = " + magnitude);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            magnitude /= 2;
            print("magnitude = " + magnitude);
        }
        if (Input.GetKeyDown(KeyCode.Space))
		{
            magnitude = 7;
            print("magnitude = " + magnitude);
        }
	}
}
