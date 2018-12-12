using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutateTrees : MonoBehaviour {

	// Use this for initialization
	List<Monster> monsters;
	// Use this for initialization
	void Start () {
		monsters = new List<Monster>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Monster m = new Monster(3);
			m.GenerateMonster ();
			monsters.Add(m);
		}
		if (Input.GetKeyDown (KeyCode.M) && monsters.Count > 0) {
			Debug.Log("mutating");
			Monster m = monsters[0].Asexual();
			m.Mutate();
			m.GenerateMonster ();
			monsters.Add(m);
		}
	}
}
