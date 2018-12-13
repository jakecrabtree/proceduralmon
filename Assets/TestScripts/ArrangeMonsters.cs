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
        
    	Monster m1 = new Monster(3);
        m1.GenerateMonsterAtPosition (new Vector3(10, 0, -10));
        
    	Monster m2 = new Monster(3);
        m2.GenerateMonsterAtPosition (new Vector3(10, 0, 10));

    	Monster m3 = new Monster(3);
        m3.GenerateMonsterAtPosition (new Vector3(-10, 0, -10));

    	Monster m4 = new Monster(3);
        m4.GenerateMonsterAtPosition (new Vector3(-10, 0, 10));
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
		
	}
}
