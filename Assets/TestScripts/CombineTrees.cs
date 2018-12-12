using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineTrees : MonoBehaviour {

	List<MonsterTree> trees;
	// Use this for initialization
	void Start () {
		trees = new List<MonsterTree>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			MonsterTree t = new MonsterTree ();
			t.RandomizeUntilSane (3);
			t.generateMonster ();
			trees.Add(t);
		}
		if (Input.GetKeyDown (KeyCode.Alpha1) && trees.Count > 0) {
			MonsterTree t = trees[0].asexual();
			t.generateMonster ();
		}
		if (Input.GetKeyDown (KeyCode.B) && trees.Count >= 2) {
			MonsterTree t = trees[0].Breed(trees[1]);
			t.generateMonster ();
		}
	}
}
