using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {
	GameObject eye;
	Material monsterMat;
	// Use this for initialization
	void Start () {
		eye = Resources.Load<GameObject>("Eye");
		monsterMat = Resources.Load<Material>("MonsterBase");
		Debug.Log (eye);
		MonsterTree t = new MonsterTree ();
		t.root = new CubeTreeNode (-1);
		CubeTreeNode ct = new CubeTreeNode (1);
		t.root.children [6] = ct;
		ct.children [1] = t.root;
		t.generateMonster ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			MonsterTree t = new MonsterTree ();
			t.RandomizeUntilSane (3);
			t.generateMonster ();
		}
	}

	void FixedUpdate() {
		
	}
}
