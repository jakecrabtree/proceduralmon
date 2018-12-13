using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			transform.Translate (new Vector3(0,0,1));
		}
		if (Input.GetKey (KeyCode.S)) {
			transform.Translate (new Vector3 (0,0,-1));
		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate (new Vector3 (-1, 0, 0));
		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Translate (new Vector3 (1, 0, 0));
		}
		if (Input.GetKey (KeyCode.C)) {
			transform.Translate (new Vector3 (0, -1, 0));
		}
		if (Input.GetKey (KeyCode.X)) {
			transform.Translate (new Vector3 (0, 1, 0));
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			transform.Rotate (new Vector3(0, .4f, 0));
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			transform.Rotate (new Vector3 (0, -.4f, 0));
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.Rotate (new Vector3 (-.4f, 0, 0));
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			transform.Rotate (new Vector3 (.4f, 0, 0));
		}
	}
}
