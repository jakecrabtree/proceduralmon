using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour {
	public GameObject coord;
	Coordinator co;
	// Use this for initialization
	void Start () {
		co = coord.GetComponent<Coordinator> ();
	}
	
	// Update is called once per frame
	void Update () {
		GameObject target = co.creatures [co.current];
		transform.position = new Vector3 (target.transform.position.x, 0, target.transform.position.z);
	}
}
