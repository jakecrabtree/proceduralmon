using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceMonsterSpawner : MonoBehaviour {
	public int myID;
	public Creature myCreature;
	public GameObject coord;
	Coordinator co;
	// Use this for initialization
	void Start () {
		co = coord.GetComponent<Coordinator> ();
		Monster m = new Monster(3);
		GameObject o = m.GenerateMonsterAtPosition (transform.position, false);
		co.creatures [myID] = o;
		myCreature = o.GetComponent<Creature> ();
		myCreature.setShouldWalk (false);
		myCreature.myID = myID;
		myCreature.co = co;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
