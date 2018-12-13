﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {
	GameObject eye;
	Material monsterMat;
	// Use this for initialization
	void Start () {
		eye = Resources.Load<GameObject>("Eye");
		monsterMat = Resources.Load<Material>("MonsterBase");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Monster m = new Monster(3);
			m.GenerateMonster ();
			Debug.Log("Wrote to " + m.WriteToFile ());
		}
	}

	void FixedUpdate() {
		
	}
}
