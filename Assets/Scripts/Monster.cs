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
			MonsterTree childTree = this.tree.Breed(other.tree);
			InstructionSet childSet = new InstructionSet(childTree.NodeCount()); //TODO: replace with set.Breed(other.set);
			return new Monster(childTree, childSet);
		}else{
			return Asexual();
		}
	}

	public Monster Asexual(){
		MonsterTree childTree = this.tree.Asexual();
		InstructionSet childSet = new InstructionSet(childTree.NodeCount()); //TODO: replace with set.Asexual();
		return new Monster(childTree, childSet);	
	}

	public void Mutate(){
		tree.Mutate();
		//TODO: add set.Mutate();
	}

	public void GenerateMonster(){
		tree.generateMonster();
	}

	public void WriteToFile(){

	}

	public void ReadFromFile(){

	}
}
