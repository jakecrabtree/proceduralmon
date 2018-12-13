using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrangeMonsters : MonoBehaviour {
	GameObject eye;
	Material monsterMat;
	// Use this for initialization
	void Start () {
		eye = Resources.Load<GameObject>("Eye");
		monsterMat = Resources.Load<Material>("MonsterBase");
        
        for(int i=0; i<4; i++) {
       		Monster m = new Monster(3);
            m.GenerateMonster ();
        }
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
		
	}
}
