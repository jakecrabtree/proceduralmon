using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MonsterTree t = new MonsterTree ();
		t.generateMonster ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
