using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTree {
	MonsterTreeNode root;
	List<MonsterTreeNode> nodes;

	public MonsterTree(){
		nodes = new List<MonsterTreeNode>();
	}

	private MonsterTree clone(){
		MonsterTree newTree = new MonsterTree(); 
		//foreach(MonsterTreeNode node in nodes){
		//	newTree.nodes.Add(node.clone());
		//}
		return newTree;
	}

	private MonsterTree graft(MonsterTree tree){
		return this;
	}
	private MonsterTree crossover(MonsterTree tree){
		return this;
	}
	private MonsterTree asexual(MonsterTree tree){
		float type = Random.Range(0.0f, 1.0f);
		if(type <= 0.5f){
			return this;
		}
		else{
			return tree;
		}
	}
	public MonsterTree breed(MonsterTree tree){
		float type = Random.Range(0.0f,1.0f);
		if(type <= 0.3f){
			//Crossover
			return crossover(tree);
		}
		else if (type <= 0.6f){
			//Grafting
			return graft(tree);
		}
		else{
			//Asexual
			return asexual(tree);
		}
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
public abstract class MonsterTreeNode {
	protected MonsterTreeNode[] children;
	protected GameObject obj;
	protected int parent;
	public abstract Vector3 getPositionOfChild(int child);
	public MonsterTreeNode clone(){
		//TODO:Actually clone it 
		return this;
	}
}

public class CubeTreeNode : MonsterTreeNode { 
	CubeTreeNode(int parent = -1){
		children = new MonsterTreeNode[20];
		//TODO: obj = makecube
	}
	public override Vector3 getPositionOfChild(int child){
		return new Vector3();
	}
}
