using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineTrees : MonoBehaviour {

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
		if (Input.GetKeyDown (KeyCode.Alpha1) && monsters.Count > 0) {
			Monster m = monsters[0].Asexual();
			m.GenerateMonster ();
		}
		if (Input.GetKeyDown (KeyCode.B) && monsters.Count >= 2) {
			Monster m = monsters[0].Breed(monsters[1]);
			m.GenerateMonster ();
		}
	}
}
