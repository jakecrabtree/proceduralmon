using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTree {
	MonsterTreeNode root;
	public MonsterTree breed(MonsterTree tree){
		return this;
	}
}
public abstract class MonsterTreeNode {
	protected MonsterTreeNode[] children;
	protected GameObject obj;
	protected int parent;
	public abstract Vector3 getPositionOfChild(int child);
}

public class CubeTreeNode : MonsterTreeNode { 
	CubeTreeNode(int parent = -1){
		children = new MonsterTreeNode[20];
		//TODO: obj = makecube
	}
	public override Vector3 getPositionOfChild(int child){
		return new Vector3();
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
