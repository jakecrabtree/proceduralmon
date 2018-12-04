using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutateTrees : MonoBehaviour {

	// Use this for initialization
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
		if (Input.GetKeyDown (KeyCode.M) && trees.Count > 0) {
			Debug.Log("mutating");
			MonsterTree t = trees[0].asexual();
			t.mutate();
			t.generateMonster ();
			trees.Add(t);
		}
	}
}
