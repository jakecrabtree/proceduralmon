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

	public static readonly float SEEXUAL_REPRO_CHANCE = 0.4f;
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
		if (rand <= 1.0f/* TODO: return to sexual repro chance*/){
			Dictionary<MonsterTreeNode, int> parent1Map;
			Dictionary<MonsterTreeNode, int> parent2Map;
			MonsterTree childTree = this.tree.Breed(other.tree, out parent1Map, out parent2Map);
			InstructionSet remappedSet1 = RemapInstructionSet(this, childTree, parent1Map);
			InstructionSet remappedSet2 = RemapInstructionSet(other, childTree, parent2Map);
			InstructionSet childSet = remappedSet1.Asexual();//.Breed(remappedSet2);
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

	public void GenerateMonster(){
		tree.generateMonster();
	}
    
    public void GenerateMonsterAtPosition(Vector3 pos) {
        tree.generateMonsterAtPosition(pos);
    }

	public void WriteToFile(){

	}

	public void ReadFromFile(){

	}
}
