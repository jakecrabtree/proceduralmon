using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceMonsterSpawner : MonoBehaviour {
	public int myID;
	public Creature myCreature;
	public GameObject coord;
	Coordinator co;
	GameObject myO;
	// Use this for initialization
	void Start () {
		co = coord.GetComponent<Coordinator> ();
		Monster m = co.getMonster(myID);
		GameObject o = m.GenerateMonsterAtPosition (transform.position, false);
		myO = o;
		co.creatures [myID] = o;
		myCreature = o.GetComponent<Creature> ();
		myCreature.setShouldWalk (false);
		myCreature.myID = myID;
		myCreature.co = co;
	}
	
	// Update is called once per frame
	void Update () {
		if (!co.hasSelected) {
			myO.transform.position = transform.position;
		}
	}
}
