﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTree {
	//TODO: Make private
	public MonsterTreeNode root;
	List<MonsterTreeNode> nodes;

	public MonsterTree(){
		nodes = new List<MonsterTreeNode>();
	}

	private MonsterTree clone(){
		MonsterTree newTree = new MonsterTree();
		newTree.nodes = root.CopySubTree();
		newTree.root = newTree.nodes[0];
		return newTree;
	}

	private MonsterTree graft(MonsterTree tree){
		//Select Node and where to insert in this node from caller's tree
		int index = Random.Range(0, nodes.Count);
		MonsterTreeNode selectedNode = nodes[index];
		int insertionPos = selectedNode.parent;
		while (insertionPos != selectedNode.parent){
			insertionPos = Random.Range(0, selectedNode.children.Length);
		} 
		
		//Select Node from param's tree
		int targetIndex = Random.Range(0, tree.nodes.Count);
		MonsterTreeNode targetNode = nodes[targetIndex];

		//Copy Caller's Tree and target's subtree
		MonsterTree res = this.clone();
		List<MonsterTreeNode> newNodes = targetNode.CopySubTree();
		res.nodes[index].children[insertionPos] = newNodes[0];
		res.nodes.AddRange(newNodes);
		return res;
	}
	private MonsterTree crossover(MonsterTree tree){
		MonsterTree res = this.clone();
		//Get positions of all the caller's nodes
		Dictionary<MonsterTreeNode, int> callerMap = new Dictionary<MonsterTreeNode, int>();
		for (int i = 0; i < res.nodes.Count; ++i){
			callerMap.Add(res.nodes[i], i);
		}
		//Get positions of all the param's nodes
		Dictionary<MonsterTreeNode, int> paramMap = new Dictionary<MonsterTreeNode, int>();
		for (int i = 0; i < tree.nodes.Count; ++i){
			paramMap.Add(tree.nodes[i], i);
		}

		//Find Crossover Point
		int crossoverPoint = Random.Range(0, nodes.Count);
		
		/* 	for (int i = crossoverPoint; i < tree.nodes.Count; ++i){
			MonsterTreeNode currNode = res.nodes[i];
			for(int c = 0; c < currNode.children.Length; ++c){
				MonsterTreeNode child = currNode.children[c];
				if (child == null || c == currNode.parent) continue;
				int childPos = [child];
			}
		}*/

		
		return res;
	}
	public MonsterTree asexual(){
		return this.clone();
	}
	public MonsterTree breed(MonsterTree tree){
		float type = UnityEngine.Random.Range(0.0f,1.0f);
		if(type <= 0.5f){
			//Crossover
			return crossover(tree);
		}
		else{
			//Grafting
			return graft(tree);
		}
	}
	public GameObject generateMonster() {
		/*
		GameObject o = GameObject.CreatePrimitive (PrimitiveType.Cube);
		Rigidbody rb = o.AddComponent<Rigidbody>();
		GameObject o2 = GameObject.CreatePrimitive (PrimitiveType.Cube);
		Rigidbody rb2 = o2.AddComponent<Rigidbody> (); 
		o2.transform.Translate (new Vector3 (1, 0, 0));
		HingeJoint hj = o2.AddComponent<HingeJoint> ();
		hj.connectedBody = rb;
		return o;*/
		return root.generateMonster (new Vector3(0, 40, 0), 0, null);
	}
}
public abstract class MonsterTreeNode {
	public MonsterTreeNode[] children;
	public GameObject obj;
	public int parent;
	public abstract Vector3 getPositionOfChild(int child);
	protected abstract MonsterTreeNode createEmptyClone();

	//Does NOT clone children, only local data to the node
	public MonsterTreeNode LocalClone(){
		MonsterTreeNode cloneNode = createEmptyClone();
		cloneNode.parent = parent;
		cloneNode.obj = GameObject.Instantiate(obj);
		cloneNode.children = new MonsterTreeNode[children.Length];
		return cloneNode;
	}

	private MonsterTreeNode CopySubTreeHelper(List<MonsterTreeNode> nodes, MonsterTreeNode parentNode){
		MonsterTreeNode copyNode = this.LocalClone();
		nodes.Add(copyNode);
		for (int i = 0; i < children.Length; ++i){
			if (children[i] == null) continue;
			if (i == parent){ 
				copyNode.children[i] = parentNode;
			}
			else{
				copyNode.children[i] = children[i].CopySubTreeHelper(nodes, this);
			}
		}
		return copyNode;
	}
	//returned list is in pre-order traversal order!
	public List<MonsterTreeNode> CopySubTree(){
		List<MonsterTreeNode> ret = new List<MonsterTreeNode>();
		this.CopySubTreeHelper(ret, null);
		return ret;
	}
	public Vector3 getScaledPositionOfChild(int child) {
		return Vector3.Scale(obj.transform.localScale, getPositionOfChild(child));
	}
	public GameObject generateMonster(Vector3 basePos, int depth, GameObject par) {
		GameObject o = GameObject.CreatePrimitive (PrimitiveType.Cube);
		obj = o;
		o.transform.localScale = new Vector3 (Random.Range(10, 100) / 20.0f, Random.Range(10, 100) / 20.0f, Random.Range(10,100) / 20.0f);
		//o.transform.localScale = new Vector3(1, 1, 1);
		//o.transform.localScale = new Vector3(1, 1, 1);
		o.AddComponent<Rigidbody> ();
		if (parent != -1) {
			basePos -= getScaledPositionOfChild (parent);
		}
		Debug.Log ("POS: " + basePos);
		o.transform.position = basePos;
		if (par != null) {
			//o.transform.SetParent (par.transform);
			HingeJoint cj = o.AddComponent<HingeJoint> ();
			cj.connectedBody = par.transform.GetComponent<Rigidbody> ();
			cj.anchor = getPositionOfChild (parent);
		}
		for (int i = 0; i < children.Length; i++) {
			if (/*children [i] != null && i != parent*/ Random.Range(0, 100) < (100 / (depth + 5)) && depth < 3 && i != parent) {
				children[i] = new CubeTreeNode (Random.Range(0, 20));
				children[i].children [children[i].parent] = this;
				children[i].generateMonster (o.transform.position + getScaledPositionOfChild(i), depth + 1, o);
			}
		}
		return o;
	}
}

public class CubeTreeNode : MonsterTreeNode { 
	public CubeTreeNode(int p = -1){
		parent = p;
		children = new MonsterTreeNode[20];
		//TODO: obj = makecube
	}

	protected override MonsterTreeNode createEmptyClone(){
		return new CubeTreeNode();
	}
	public override Vector3 getPositionOfChild(int child){
		float L = -0.5f;
		float H = 0.5f;
		if (child < 8) {
			float x = ((child & 1) == 0) ? L : H;
			float y = (((child >> 1) & 1) == 0) ? L : H;
			float z = (((child >> 2) & 1) == 0) ? L : H;
			return new Vector3 (x, y, z);
		} else {
			switch (child) {
			case 8:
				return (getPositionOfChild (0) + getPositionOfChild (1)) / 2;
			case 9:
				return (getPositionOfChild (0) + getPositionOfChild (2)) / 2;
			case 10:
				return (getPositionOfChild (0) + getPositionOfChild (4)) / 2;
			case 11:
				return (getPositionOfChild (1) + getPositionOfChild (3)) / 2;
			case 12:
				return (getPositionOfChild (1) + getPositionOfChild (5)) / 2;
			case 13:
				return (getPositionOfChild (2) + getPositionOfChild (3)) / 2;
			case 14:
				return (getPositionOfChild (2) + getPositionOfChild (6)) / 2;
			case 15:
				return (getPositionOfChild (3) + getPositionOfChild (7)) / 2;
			case 16:
				return (getPositionOfChild (4) + getPositionOfChild (5)) / 2;
			case 17:
				return (getPositionOfChild (4) + getPositionOfChild (6)) / 2;
			case 18:
				return (getPositionOfChild (5) + getPositionOfChild (7)) / 2;
			case 19:
				return (getPositionOfChild (6) + getPositionOfChild (7)) / 2;
			}
		}
		return new Vector3();
	}
}
