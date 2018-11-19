using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		MonsterTree t = new MonsterTree ();
		t.root = new CubeTreeNode (-1);
		CubeTreeNode ct = new CubeTreeNode (1);
		t.root.children [6] = ct;
		ct.children [1] = t.root;
		t.generateMonster ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
