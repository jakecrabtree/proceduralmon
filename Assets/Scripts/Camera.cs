using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
	public bool isHori;
	public bool isCamera;
	public bool isVert;
	int counter = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKey (KeyCode.W)) {
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
		}*/
		if (Input.GetKey (KeyCode.A) && isHori) {
			transform.Rotate (new Vector3 (0, .4f, 0));
		}
		if (Input.GetKey (KeyCode.D) && isHori) {
			transform.Rotate (new Vector3 (0, -.4f, 0));
		}
		if (Input.GetKey (KeyCode.W) && isVert) {
			if (counter > -50) {
				counter--;
				transform.Rotate (new Vector3 (.4f, 0, 0));
			}
		}
		if (Input.GetKey (KeyCode.S) && isVert) {
			if (counter < 50) {
				counter++;
				transform.Rotate (new Vector3 (-.4f, 0, 0));
			}
		}
		if (Input.GetKey (KeyCode.UpArrow) && isCamera) {
			if (counter > -200) {
				counter--;
				transform.Translate (new Vector3(0, 0, .5f));
			}
		}
		if (Input.GetKey (KeyCode.DownArrow) && isCamera) {
			if (counter < 10) {
				counter++;
				transform.Translate (new Vector3(0, 0, -.5f));
			}
		}
	}
}
