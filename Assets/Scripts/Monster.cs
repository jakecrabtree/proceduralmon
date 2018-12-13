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

	public static readonly float ASEXUAL_CHANCE = 0.6f;
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
		if (rand <= ASEXUAL_CHANCE){
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
		Dictionary<MonsterTreeNode, int> childMap = new Dictionary<MonsterTreeNode, int>();
		for(int i = 0; i < childTree.nodes.Count; ++i){
			childMap.Add(childTree.nodes[i], i);
		}
		Dictionary<int, int> positionRemap = new Dictionary<int, int>();
		for(int i = 0; i < parent.tree.NodeCount(); ++i){
			if (childMap.ContainsKey(parent.tree.nodes[i])){
				positionRemap.Add(parent1Map[parent.tree.nodes[i]], childMap[parent.tree.nodes[i]]);
			}
			else{
				positionRemap.Add(parent1Map[parent.tree.nodes[i]], -1);
			}
		}
		for(int i = 0; i < ret.getCount(); ++i){
			Instruction instruction = ret.getInstruction(i);
			if(positionRemap[instruction.getNode()] != -1){
				instruction.setNode(positionRemap[instruction.getNode()]);
			}
			else{
				ret.removeInstructionAt(i--);
			}
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

	public void WriteToFile(){

	}

	public void ReadFromFile(){

	}
}
