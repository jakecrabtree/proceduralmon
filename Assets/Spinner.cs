using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {
	int count = 0;
	public GameObject camera;
	public GameObject holder;
	public GameObject MP1;
	public GameObject MP2;
	public GameObject MP3;
	public GameObject MP4;
	public GameObject MP5;
	public GameObject MP6;
	public GameObject coord;
	Coordinator co;
	// Use this for initialization
	void Start () {
		co = coord.GetComponent<Coordinator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			transform.Rotate (new Vector3(0, 60, 0));
			count = (count + 5) % 6;
			co.current = count;
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			transform.Rotate (new Vector3(0, -60, 0));
			count = (count + 1) % 6;
			co.current = count;
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			camera.transform.position = holder.transform.position;
			camera.transform.LookAt (transform);
			camera.transform.SetParent (holder.transform);
			MP1.GetComponent<RaceMonsterSpawner> ().myCreature.setShouldWalk (true);
			MP2.GetComponent<RaceMonsterSpawner> ().myCreature.setShouldWalk (true);
			MP3.GetComponent<RaceMonsterSpawner> ().myCreature.setShouldWalk (true);
			MP4.GetComponent<RaceMonsterSpawner> ().myCreature.setShouldWalk (true);
			MP5.GetComponent<RaceMonsterSpawner> ().myCreature.setShouldWalk (true);
			MP6.GetComponent<RaceMonsterSpawner> ().myCreature.setShouldWalk (true);
			Destroy (transform.gameObject);
		}
	}
}
