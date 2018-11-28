using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxPhysics : MonoBehaviour {
    //public float angDrag;
    //public float spinForce;
    public float maxAngVel;

    void FixedUpdate()
    {
        //GetComponent<Rigidbody>().angularDrag = angDrag;
        GetComponent<Rigidbody>().maxAngularVelocity = maxAngVel;
        //GetComponent<Rigidbody>().AddTorque(0, 10 * spinForce, 0);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            this.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.right * 1000, transform.position);
            //this.rigidbody.AddTorque(Vector3.left * 20000);
        }
    }

    // Use this for initialization
    void Start () {
        //angDrag = GetComponent<Rigidbody>().angularDrag;
        maxAngVel = GetComponent<Rigidbody>().maxAngularVelocity;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

