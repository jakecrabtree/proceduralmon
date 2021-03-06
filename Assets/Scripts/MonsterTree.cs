﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTree {
	public MonsterTreeNode root;
	public List<MonsterTreeNode> nodes;

	//The Monster this monster tree belongs to
	public Monster monster;

	private static GameObject eye = Resources.Load<GameObject>("Eye");
	private static Material monsterMat = Resources.Load<Material>("MonsterBase");

	public MonsterTree(){
		nodes = new List<MonsterTreeNode>();
	}

	private void Randomize(int maxDepth){
		root = new CubeTreeNode (-1);
		root.Randomize(maxDepth, maxDepth);
		nodes = root.GetSubTree ();
	}

	public int NodeCount(){
		return nodes.Count;
	}

	public void RandomizeUntilSane(int maxDepth){
		Debug.Log ("HI!");
		int count = 0;
		do {
			Debug.Log("LOOPING: " + count);
			Randomize (maxDepth);
		} while((nodes.Count > 20 || nodes.Count <= 1 || selfIntersects ()) && (count++) < 1000);
	}

	public bool selfIntersects(){
		HashSet<long> spahash = new HashSet<long>();
		return root.checkForSelfIntersect (spahash, 0, 0, 0);
	}

	private MonsterTree clone(){
		MonsterTree newTree = new MonsterTree();
		newTree.nodes = root.CopySubTree();
		newTree.root = newTree.nodes[0];
		return newTree;
	}

	private MonsterTree graft(MonsterTree tree, out Dictionary<MonsterTreeNode, int> parent1Map, out Dictionary<MonsterTreeNode, int> parent2Map){
		//Select Node and where to insert in this node from caller's tree
		int index = Random.Range(0, nodes.Count);
		MonsterTreeNode selectedNode = nodes[index];
		int insertionPos;
		do{
			insertionPos = Random.Range(0, selectedNode.children.Length);
		}while(insertionPos == selectedNode.parent);

		
		//Select Node from param's tree
		int targetIndex = Random.Range(0, tree.nodes.Count);
		MonsterTreeNode targetNode = tree.nodes[targetIndex];

		//Copy Caller's Tree and target's subtree
		MonsterTree res = this.clone();
		parent1Map = new Dictionary<MonsterTreeNode, int>();
		for (int i = 0; i < res.nodes.Count; ++i){
			parent1Map.Add(res.nodes[i], i);
		}
		parent2Map = new Dictionary<MonsterTreeNode, int>();
		Dictionary<MonsterTreeNode, int> tempMap = new Dictionary<MonsterTreeNode, int>();
		for (int i = 0; i < tree.nodes.Count; ++i){
			tempMap.Add(tree.nodes[i],i);
		}
		List<MonsterTreeNode> subtree = targetNode.GetSubTree();
		List<MonsterTreeNode> newNodes = targetNode.CopySubTree();
		for (int i = 0; i < subtree.Count; ++i){
			if (tempMap.ContainsKey(subtree[i])){
				parent2Map.Add(newNodes[i], tempMap[subtree[i]]);
			}
		}

		if (index < res.nodes.Count && index > 0 && insertionPos< res.nodes[index].children.Length && insertionPos > 0 && newNodes.Count > 0){
			//Insert into Caller and add target's nodes to caller's list
			res.nodes[index].children[insertionPos] = newNodes[0];
			res.nodes.AddRange(newNodes);
		}
		return res;
	}
	private static int min(int a, int b){
		return a < b ? a : b;
	}
	private MonsterTree crossover(MonsterTree tree, out Dictionary<MonsterTreeNode, int> parent1Map, out Dictionary<MonsterTreeNode, int> parent2Map){
		MonsterTree parent1 = this.clone();
		MonsterTree parent2 = tree.clone();
		//Get positions of all the caller's nodes
		parent1Map = new Dictionary<MonsterTreeNode, int>();
		for (int i = 0; i < parent1.nodes.Count; ++i){
			parent1Map.Add(parent1.nodes[i], i);
		}
		//Get positions of all the param's nodes
		parent2Map = new Dictionary<MonsterTreeNode, int>();
		for (int i = 0; i < parent2.nodes.Count; ++i){
			parent2Map.Add(parent2.nodes[i], i);
		}

		//Find Crossover Point (one for simplicity)
		int crossoverPoint = Random.Range(0, parent1.nodes.Count);
		
		//Combine node lists bases on crossover in first parent
		parent1.nodes.RemoveRange(crossoverPoint, parent1.nodes.Count - crossoverPoint);
		parent2.nodes.RemoveRange(0, min(crossoverPoint,parent2.nodes.Count));
		parent1.nodes.AddRange(parent2.nodes);

		//Remap Parent1's nodes
		for (int i = 0; i < crossoverPoint; ++i){
			MonsterTreeNode currNode = parent1.nodes[i];
			for(int c = 0; c < currNode.children.Length; ++c){
				MonsterTreeNode child = currNode.children[c];
				if (child == null || c == currNode.parent) continue;
				int childPos = parent1Map[child];
				if (childPos < parent1.nodes.Count){
					currNode.children[c] = parent1.nodes[childPos];
				}
				else{
					currNode.children[c] = null;
				}
			}
		}
		//Remap Parent2's nodes
		for (int i = crossoverPoint; i < parent1.nodes.Count; ++i){
			MonsterTreeNode currNode = parent1.nodes[i];
			for(int c = 0; c < currNode.children.Length; ++c){
				MonsterTreeNode child = currNode.children[c];
				if (child == null || c == currNode.parent) continue;
				int childPos = parent2Map[child];
				if (childPos < parent1.nodes.Count){
					currNode.children[c] = parent1.nodes[childPos];
				}
				else{
					currNode.children[c] = null;
				}
			}
		}
		return parent1;
	}
	public MonsterTree Asexual(){
		return this.clone();
	}
	public MonsterTree Breed(MonsterTree tree, out Dictionary<MonsterTreeNode, int> parent1Map, out Dictionary<MonsterTreeNode, int> parent2Map){
		while(true) {
			try{
				float type = UnityEngine.Random.Range(0.0f,1.0f);
				if(type <= Monster.CROSSOVER_CHANCE){
					//Crossover
					return crossover(tree, out parent1Map, out parent2Map);
				}
				else{
					//Grafting
					return graft(tree, out parent1Map, out parent2Map);
				}
			} catch {
				Debug.Log ("Bad Tree: Ignoring");
			}
		}
	}

	public void Mutate(){
		bool selfCollides = false;
		bool dirty = false;
		MonsterTree copy = this.clone();
		do {
			for (int i = 0; i < copy.nodes.Count; ++i){
				MonsterTreeNode node = nodes[i];
				dirty |= mutateNode(node);
			}
			if (dirty && (selfCollides = selfIntersects())){
				this.nodes = copy.nodes;
				this.root = copy.root;
				copy = clone();
				dirty = false;
			}
		}
		while (!((selfCollides && dirty) || !dirty));
	}
	public bool mutateNode(MonsterTreeNode node){
		/**
		* Add a node
		* Remove a node
		* Move a connection's or connections' position
		* Change scale x, y, and z
		**/
		float random = Random.Range(0.0f, 1.0f);
		if (random <= Monster.PER_NODE_MUTATION_CHANCE){
			random = Random.Range(0.0f, 1.0f);
			if (random <= 0.1f){
				//Add a node at 10%
				MonsterTreeNode newNode = node.createEmptyClone();
				List<int> freeNodePositions = new List<int>();
				int count = 0;
				foreach(MonsterTreeNode child in node.children){
					if (child == null){
						freeNodePositions.Add(count);
					}
					++count;
				}
				if(freeNodePositions.Count != 0){
					int rand = Random.Range(0,freeNodePositions.Count);
					int pos = freeNodePositions[rand];
					node.children[pos] = newNode;
					this.nodes.Add(newNode);
					return true;		
				}
			}else if(random <= 0.2f){
				//Remove a node at 10%
				List<int> childNodePositions = new List<int>();
				int count = 0;
				foreach(MonsterTreeNode child in node.children){
					if (count != node.parent && child != null){
						childNodePositions.Add(count);
					}
					++count;
				}
				if(childNodePositions.Count != 0){
					node.children[childNodePositions[Random.Range(0,childNodePositions.Count)]] = null;
				}
			}
			else if (random <= 0.6f){
				//Move a Connection at 40%
				List<int> childNodePositions = new List<int>();
				List<int> freeNodePositions = new List<int>();
				int count = 0;
				foreach(MonsterTreeNode child in node.children){
					if (count != node.parent && child != null){
						childNodePositions.Add(count);
					}
					else if (count != node.parent){
						freeNodePositions.Add(count);
					}
					++count;
				}
				if(childNodePositions.Count > 0){
					MonsterTreeNode nodeToBeMoved = node.children[childNodePositions[Random.Range(0, childNodePositions.Count)]];
					int insertionPos = Random.Range(0, freeNodePositions.Count);
					node.children[insertionPos] = nodeToBeMoved;
					return true;
				}
			}
			else{
				//Scale at 40% 
				node.scale += node.randomScale() / 4.0f;
				return true;
			}
		}		
		return false;
	}
	public GameObject generateMonster() {
		return generateMonsterAtPosition(new Vector3(0,20,0));
	}
    
    public GameObject generateMonsterAtPosition(Vector3 pos, bool shouldWalk = true) {
		List<GameObject> goList = new List<GameObject> ();
		GameObject o = root.generateMonster (pos, 0, null, monsterMat, new Color(Random.Range(.5f, .8f), Random.Range(.5f, .8f), Random.Range(.5f, .8f)), goList);
		Creature cr = o.AddComponent<Creature> ();
		cr.nodeSetup(goList, monster.GetInstructions());
        cr.setShouldWalk(true);
		FitnessFunction fn = o.AddComponent<FitnessFunction>();
		fn.AssignMonster(monster);
		if (eye != null) {
			float eyeSmall = .3f;
			float eyeLarge = .5f;
			float eyeSize = Random.Range (eyeSmall, eyeLarge); 
			float eyeDiff = Random.Range (eyeSize / 2, .5f - eyeSize / 2);
			GameObject e1 = GameObject.Instantiate (eye);
			e1.transform.SetParent (o.transform);
			e1.transform.localScale = new Vector3 (eyeSize, eyeSize, eyeSize / 2);
			e1.transform.localPosition = new Vector3(-eyeDiff, 0, -.5f);
			GameObject e2 = GameObject.Instantiate (eye);
			e2.transform.SetParent (o.transform);
			e2.transform.localScale = new Vector3 (eyeSize, eyeSize, eyeSize / 2);
			e2.transform.localPosition = new Vector3(eyeDiff, 0, -.5f);
		}
		return o;
	}
}
public abstract class MonsterTreeNode {
	public MonsterTreeNode[] children;
	//public GameObject obj;
	public int parent;
	public Vector3 scale;

	public abstract Vector3 getPositionOfChild(int child);
	public abstract MonsterTreeNode createEmptyClone();

	//Does NOT clone children, only local data to the node
	public MonsterTreeNode LocalClone(){
		MonsterTreeNode cloneNode = createEmptyClone();
		cloneNode.parent = parent;
		//I think the GameObject shouldn't be cloned here, just the info about it (scale)
		//cloneNode.obj = GameObject.Instantiate(obj);
		cloneNode.children = new MonsterTreeNode[children.Length];
		cloneNode.scale = scale;
		return cloneNode;
	}

	public abstract Vector3 randomScale();

	private void getNodesList() {

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

	private MonsterTreeNode GetSubTreeHelper(List<MonsterTreeNode> nodes, MonsterTreeNode parentNode){
		MonsterTreeNode copyNode = this;
		nodes.Add(copyNode);
		for (int i = 0; i < children.Length; ++i){
			if (children[i] == null) continue;
			if (i == parent){ 
				copyNode.children[i] = parentNode;
			}
			else{
				copyNode.children[i] = children[i].GetSubTreeHelper(nodes, this);
			}
		}
		return copyNode;
	}
	//returned list is in pre-order traversal order!
	public List<MonsterTreeNode> GetSubTree(){
		List<MonsterTreeNode> ret = new List<MonsterTreeNode>();
		this.GetSubTreeHelper(ret, null);
		return ret;
	}
	public bool checkForSelfIntersect(HashSet<long> ha, float basex, float basey, float basez) {
		Debug.Log ("SI CHECK");
		if (parent != -1) {
			Vector3 ppos = getScaledPositionOfChild (parent);
			basex -= ppos.x;
			basey -= ppos.y;
			basez -= ppos.z;
		}
		Vector3 llpos = getScaledPositionOfChild (0);
		long x = Mathf.RoundToInt ((basex + llpos.x) * 10) + 500000;
		long y = Mathf.RoundToInt ((basey + llpos.y) * 10) + 500000;
		long z = Mathf.RoundToInt ((basez + llpos.z) * 10) + 500000; 
		int sx = Mathf.RoundToInt (scale.x * 10);
		int sy = Mathf.RoundToInt (scale.y * 10);
		int sz = Mathf.RoundToInt (scale.z * 10);
		for (long x0 = x; x0 < x + sx; x0++) {
			for (long y0 = y; y0 < y + sy; y0++) {
				long z0 = z;
					long myha = (z0 << 40) | (y0 << 20) | x0;
					//Debug.Log ("HASH: " + myha);
					if (ha.Contains (myha)) {
						return true;
					}
					ha.Add (myha);
			}
		}
		for (long x0 = x; x0 < x + sx; x0++) {
			for (long y0 = y; y0 < y + sy; y0++) {
				long z0 = z + sz - 1;
				long myha = (z0 << 40) | (y0 << 20) | x0;
				//Debug.Log ("HASH: " + myha);
				if (ha.Contains (myha)) {
					return true;
				}
				ha.Add (myha);
			}
		}
		for (long z0 = z + 1; z0 < z + sz - 1; z0++) {
			for (long y0 = y; y0 < y + sy; y0++) {
				long x0 = x;
				long myha = (z0 << 40) | (y0 << 20) | x0;
				//Debug.Log ("HASH: " + myha);
				if (ha.Contains (myha)) {
					return true;
				}
				ha.Add (myha);
			}
		}
		for (long z0 = z + 1; z0 < z + sz - 1; z0++) {
			for (long y0 = y; y0 < y + sy; y0++) {
				long x0 = x + sx - 1;
				long myha = (z0 << 40) | (y0 << 20) | x0;
				//Debug.Log ("HASH: " + myha);
				if (ha.Contains (myha)) {
					return true;
				}
				ha.Add (myha);
			}
		}
		for (long x0 = x + 1; x0 < x + sx - 1; x0++) {
			for (long z0 = z + 1; z0 < z + sz - 1; z0++) {
				long y0 = y;
				long myha = (z0 << 40) | (y0 << 20) | x0;
				//Debug.Log ("HASH: " + myha);
				if (ha.Contains (myha)) {
					return true;
				}
				ha.Add (myha);
			}
		}
		for (long x0 = x + 1; x0 < x + sx - 1; x0++) {
			for (long z0 = z + 1; z0 < z + sz - 1; z0++) {
				long y0 = y + sy - 1;
				long myha = (z0 << 40) | (y0 << 20) | x0;
				//Debug.Log ("HASH: " + myha);
				if (ha.Contains (myha)) {
					return true;
				}
				ha.Add (myha);
			}
		}
		for (int i = 0; i < children.Length; i++) {
			if (children [i] != null && i != parent) {
				Vector3 relpos = getScaledPositionOfChild (i);
				if (children [i].checkForSelfIntersect (ha, basex + relpos.x, basey + relpos.y, basez + relpos.z)) {
					return true;
				}
			}
		}
		return false;
	}
	public Vector3 getScaledPositionOfChild(int child) {
		return Vector3.Scale(scale, getPositionOfChild(child));
	}
	public void Randomize (int maxDepth, int totalDepth) {
		for (int i = 0; i < 20; i++) {
			if (Random.Range(0, 100) < (12 / totalDepth * maxDepth) && maxDepth > 0 && i != parent) {
				children[i] = new CubeTreeNode (Random.Range(0, children.Length));
				children[i].children [children[i].parent] = this;
				children [i].Randomize (maxDepth - 1, totalDepth);
			}
		}
	}

	public GameObject generateMonster(Vector3 basePos, int depth, GameObject par, Material ma, Color co, List<GameObject> goList) {
		GameObject o = GameObject.CreatePrimitive (PrimitiveType.Cube);
		goList.Add (o);
		if (ma != null) {
			Material myMat = new Material (ma);
			myMat.color = co;
			o.GetComponent<Renderer> ().material = myMat;
		}
		//obj = o;
		/*Material myMat = Material
		myMat.color = new Color(Random.Range(0, 256) / 256.0f, Random.Range(0, 256) / 256.0f, Random.Range(0, 256) / 256.0f);*/
		o.transform.localScale = scale;
		//o.transform.localScale = new Vector3(1, 1, 1);
		//o.transform.localScale = new Vector3(1, 1, 1);
		o.AddComponent<Rigidbody> ();
		if (parent != -1) {
			basePos -= getScaledPositionOfChild (parent);
		}
		//Debug.Log ("POS: " + basePos);
		o.transform.position = basePos;
		if (par != null) {
			//o.transform.SetParent (par.transform);
			HingeJoint cj = o.AddComponent<HingeJoint> ();
			cj.enableCollision = true;
			cj.useMotor = true;
			JointMotor jm = cj.motor;
			int x = Random.Range (0, 3);
			if (x == 0) {
				cj.axis = new Vector3 (1, 0, 0);
			} else if (x == 1) {
				cj.axis = new Vector3 (0, 1, 0);
			} else {
				cj.axis = new Vector3 (0, 0, 1);
			}
			//jm.targetVelocity = Random.Range(-1000, 1000);
			jm.force = 300;
			cj.motor = jm;
			cj.connectedBody = par.transform.GetComponent<Rigidbody> ();
			cj.anchor = getPositionOfChild (parent);
		}
		for (int i = 0; i < children.Length; i++) {
			if (children [i] != null && i != parent) {
				int r = Random.Range (0, 5);
				Color touse;
				float h, s, v;
				Color.RGBToHSV (co, out h, out s, out v);
				if (r == 0) {
					touse = co;
				} else if (r == 1) {
					touse = new Color (1.2f * co.r, 1.2f * co.g, 1.2f * co.b);
				} else if (r == 2) {
					touse = new Color (.7f * co.r, .7f * co.g, .7f * co.b);
				} else if (r == 3) {
					touse = Color.HSVToRGB ((h + .1f) % 1, s, v);
				} else {
					touse = Color.HSVToRGB ((h + .9f) % 1, s, v);
				}
				children[i].generateMonster (o.transform.position + getScaledPositionOfChild(i), depth + 1, o, ma, touse, goList);
			}
		}
		return o;
	}
}

public class CubeTreeNode : MonsterTreeNode { 
	public CubeTreeNode(int p = -1){
		parent = p;
		children = new MonsterTreeNode[20];
		scale = randomScale();
	}

	public override Vector3 randomScale(){
		return new Vector3 (Random.Range (5, 50) / 10.0f, Random.Range (5, 50) / 10.0f, Random.Range (5, 50) / 10.0f);
	}

	public override MonsterTreeNode createEmptyClone(){
		return new CubeTreeNode();
	}
	public override Vector3 getPositionOfChild(int child){
		float L = -0.5005f;
		float H = 0.5005f;
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
