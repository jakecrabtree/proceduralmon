using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {
	public GameObject eye;
	public Material monsterMat;
	// Use this for initialization
	void Start () {
		MonsterTree t = new MonsterTree ();
		t.root = new CubeTreeNode (-1);
		CubeTreeNode ct = new CubeTreeNode (1);
		t.root.children [6] = ct;
		ct.children [1] = t.root;
		t.generateMonster (eye, monsterMat);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			MonsterTree t = new MonsterTree ();
			t.RandomizeUntilSane (3);
			t.generateMonster (eye, monsterMat);
		}
	}

	void FixedUpdate() {
		
	}
}
