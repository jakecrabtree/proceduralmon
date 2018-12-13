using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster {
	//Represents the Monster's geometry
	private MonsterTree tree;

	//Represents the Monster's instruction set
	private InstructionSet set;

	//Fitness value
	public float fitness = 0;

	public static readonly float SEXUAL_REPRO_CHANCE = 0.4f;
	public static readonly float CROSSOVER_CHANCE = 0.5f;
	public static readonly float PER_NODE_MUTATION_CHANCE = 0.001f;

	public Monster(){
		SetMonsterTree(new MonsterTree());
		SetInstructions(new InstructionSet());
	}

	public Monster(int treeDepth){
		SetMonsterTree(new MonsterTree());
		tree.RandomizeUntilSane(treeDepth);
		SetInstructions(new InstructionSet(tree.NodeCount()));
	}

	public Monster(MonsterTree tree){
		SetMonsterTree(tree);
		SetInstructions(new InstructionSet());
	}

	public Monster(MonsterTree tree, InstructionSet set){
		SetMonsterTree(tree);
		SetInstructions(set);
	}

	//Deserialization constructor, bool is useless but avoids name conflict
	private Monster(bool b) {

	}

	public void SetInstructions(InstructionSet set){
		this.set = set;
		this.set.monster = this;
	}

	public void SetMonsterTree(MonsterTree tree){
		this.tree = tree;
		this.tree.monster = this;
	}

	public InstructionSet GetInstructions(){
		return set;
	}

	public MonsterTree GetMonsterTree(){
		return tree;
	}

	public void Randomize(int treeDepth){
		tree.RandomizeUntilSane(treeDepth);
		SetInstructions(new InstructionSet(tree.NodeCount()));
	}

	public Monster Breed(Monster other){
		float rand = Random.Range(0.0f, 1.0f);
		if (rand <= SEXUAL_REPRO_CHANCE){
			Dictionary<MonsterTreeNode, int> parent1Map;
			Dictionary<MonsterTreeNode, int> parent2Map;
			MonsterTree childTree = this.tree.Breed(other.tree, out parent1Map, out parent2Map);
			InstructionSet remappedSet1 = RemapInstructionSet(this, childTree, parent1Map);
			InstructionSet remappedSet2 = RemapInstructionSet(other, childTree, parent2Map);
			InstructionSet childSet = remappedSet1.Breed(remappedSet2);
			return new Monster(childTree, childSet);
		}else{
			return Asexual();
		}
	}

	static InstructionSet RemapInstructionSet(Monster parent, MonsterTree childTree, Dictionary<MonsterTreeNode, int> parent1Map){
		InstructionSet ret = parent.set.Asexual();
		//Get position map of node to position in array
		Dictionary<MonsterTreeNode, int> childMap = new Dictionary<MonsterTreeNode, int>();
		for(int i = 0; i < childTree.nodes.Count; ++i){
			childMap.Add(childTree.nodes[i], i);
		}
		//Find map from parent to child tree positions, or -1 if parent didn't pass down this node to child
		Dictionary<int, int> positionRemap = new Dictionary<int, int>();
		foreach(KeyValuePair<MonsterTreeNode, int> pair in parent1Map){
			if(childMap.ContainsKey(pair.Key)){
				positionRemap.Add(pair.Value, childMap[pair.Key]);
			}else{
				positionRemap.Add(pair.Value, -1);
			}
		}
		//Remap instructions based on map
		for(int i = 0; i < ret.getCount(); ++i){
			Instruction instruction = ret.getInstruction(i);
			//Remap if found
			if (positionRemap.ContainsKey(instruction.getNode())){
				if(positionRemap[instruction.getNode()] != -1){
					instruction.setNode(positionRemap[instruction.getNode()]);
				}
				else{ // if(instruction.getNode() >= childTree.NodeCount()){
					//Remove Instruction if now out of bounds
					ret.removeInstructionAt(i--);
				}
			}
			//Do nothing if not found but in bounds
		}
		return ret;
	}

	public Monster Asexual(){
		MonsterTree childTree = this.tree.Asexual();
		InstructionSet childSet = this.set.Asexual();
		return new Monster(childTree, childSet);	
	}

	public void Mutate(){
		tree.Mutate();
		set.Mutate();
	}

	public GameObject GenerateMonster(){
		return tree.generateMonster();
	}
    
    public void GenerateMonsterAtPosition(Vector3 pos) {
        tree.generateMonsterAtPosition(pos);
    }

	public void WriteBytes(byte[] bytes, List<byte> toOut) {
		for (int i = 0; i < bytes.Length; i++) {
			toOut.Add (bytes [i]);
		}
	}

	public void GetName(byte[] bytes, ref ulong name1, ref ulong name2) {
		for (int i = 0; i < 53; i++) {
			int sh = i % 16;
			ulong rel = bytes [i * bytes.Length / 53];
			name1 ^= (rel << (sh * 4));
		}
		for (int i = 0; i < 97; i++) {
			int sh = i % 16;
			ulong rel = bytes [i * bytes.Length / 97];
			name2 ^= (rel << (sh * 4));
		}
	}

	public string WriteToFile(){
		ulong name1 = 0;
		ulong name2 = 0;
		List<byte> toOut = new List<byte> ();
		WriteBytes (System.BitConverter.GetBytes(fitness), toOut);
		WriteBytes (System.BitConverter.GetBytes (set.getCount ()), toOut);
		for (int i = 0; i < set.getCount (); i++) {
			Instruction ins = set.getInstruction (i);
			WriteBytes (System.BitConverter.GetBytes (ins.getNode()), toOut);
			WriteBytes (System.BitConverter.GetBytes (ins.getSpeed()), toOut); 
		}
		WriteBytes (System.BitConverter.GetBytes (tree.nodes.Count), toOut);
		for (int i = 0; i < tree.nodes.Count; i++) {
			MonsterTreeNode mtn = tree.nodes [i];
			WriteBytes (System.BitConverter.GetBytes(mtn.parent), toOut);
			WriteBytes (System.BitConverter.GetBytes (mtn.scale.x), toOut);
			WriteBytes (System.BitConverter.GetBytes (mtn.scale.y), toOut);
			WriteBytes (System.BitConverter.GetBytes (mtn.scale.z), toOut);
			for (int j = 0; j < 20; j++) {
				MonsterTreeNode link = mtn.children [j];
				Debug.Log ("Looking at: " + j);
				bool isFound = false;
				if (link == null) {
					WriteBytes (System.BitConverter.GetBytes (-1), toOut);
				} else {
					Debug.Log ("NOT NULL");
					for (int k = 0; k < tree.nodes.Count; k++) {
						if (link == tree.nodes [k]) {
							isFound = true;
							WriteBytes (System.BitConverter.GetBytes (k), toOut);
							break;
						}
					}
					if (!isFound) {
						Debug.Log ("DID NOT FIND!!!");
					}
				}
			}
		}
		byte[] bytes = new byte[toOut.Count];
		for (int i = 0; i < toOut.Count; i++) {
			bytes [i] = toOut [i];
		}
		GetName (bytes, ref name1, ref name2);
		string name = "Monsters/" + name1 + "-" + name2 + ".mon";
		System.IO.FileStream f = new System.IO.FileStream (name, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write);
		f.Write (bytes, 0, bytes.Length);
		f.Close ();
		return name;
	}

	public static Monster ReadFromFile(string filename){
		System.IO.FileStream f = new System.IO.FileStream (filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
		Monster m = new Monster (true);
		byte[] bytes = new byte[8];
		f.Read (bytes, 0, 4);
		m.fitness = System.BitConverter.ToSingle (bytes, 0);
		f.Read (bytes, 0, 4);
		int icount = System.BitConverter.ToInt32 (bytes, 0);
		List<Instruction> il = new List<Instruction> ();
		for (int i = 0; i < icount; i++) {
			f.Read (bytes, 0, 4);
			int n = System.BitConverter.ToInt32 (bytes, 0);
			f.Read (bytes, 0, 4);
			float s = System.BitConverter.ToSingle (bytes, 0);
			il.Add (new Instruction (n, s));
		}
		m.set = new InstructionSet (il);
		f.Read (bytes, 0, 4);
		int ncount = System.BitConverter.ToInt32 (bytes, 0);
		List<MonsterTreeNode> mtnl = new List<MonsterTreeNode> ();
		int[,] links = new int[ncount, 20];
		for (int i = 0; i < ncount; i++) {
			CubeTreeNode mtn = new CubeTreeNode ();
			f.Read (bytes, 0, 4);
			mtn.parent = System.BitConverter.ToInt32 (bytes, 0);
			f.Read (bytes, 0, 4);
			mtn.scale.x = System.BitConverter.ToSingle (bytes, 0);
			f.Read (bytes, 0, 4);
			mtn.scale.y = System.BitConverter.ToSingle (bytes, 0);
			f.Read (bytes, 0, 4);
			mtn.scale.z = System.BitConverter.ToSingle (bytes, 0);
			for (int j = 0; j < 20; j++) {
				f.Read (bytes, 0, 4);
				links[i, j] = System.BitConverter.ToInt32 (bytes, 0);
			}
			mtnl.Add (mtn);
		}
		//Now actually link them
		for (int i = 0; i < ncount; i++) {
			MonsterTreeNode mtn = mtnl [i];
			for (int j = 0; j < 20; j++) {
				if (links[i, j] == -1) {
					mtn.children [j] = null;
				} else {
					mtn.children [j] = mtnl [links [i, j]];
				}
			}
		}
		MonsterTree mt = new MonsterTree ();
		mt.nodes = mtnl;
		mt.root = mtnl [0];
		mt.monster = m;
		m.tree = mt;
		f.Close ();
		return m;
	}
}
