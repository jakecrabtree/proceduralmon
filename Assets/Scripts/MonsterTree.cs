using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTree {
	MonsterTreeNode root;
	public MonsterTree breed(MonsterTree tree){
		return this;
	}
	private class MonsterTreeNode {
		public MonsterTreeNode[] children = new MonsterTreeNode[20];
		public GameObject obj;
	}
	public GameObject generateMonster() {
		GameObject o = GameObject.CreatePrimitive (PrimitiveType.Cube);
		Rigidbody rb = o.AddComponent<Rigidbody>();
		GameObject o2 = GameObject.CreatePrimitive (PrimitiveType.Cube);
		Rigidbody rb2 = o2.AddComponent<Rigidbody> (); 
		o2.transform.Translate (new Vector3 (1, 0, 0));
		HingeJoint hj = o2.AddComponent<HingeJoint> ();
		hj.connectedBody = rb;
		return o;
	}
}
