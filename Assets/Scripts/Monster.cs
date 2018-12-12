using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster {
	//Represents the Monster's geometry
	public MonsterTree tree;

	//Represents the Monster's instruction set
	public InstructionSet set;

	public static readonly float CROSSOVER_CHANCE = 0.5f;
	public static readonly float ASEXUAL_CHANCE = 0.6f;

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

	public void Randomize(int treeDepth){
		tree.RandomizeUntilSane(treeDepth);
		SetInstructions(new InstructionSet(tree.NodeCount()));
	}

	public Monster Breed(Monster other){
		MonsterTree tree = this.tree.Breed(other.tree);
		InstructionSet set = new InstructionSet(); //TODO: replace with set.Breed(other.set);
		return new Monster(tree, set);
	}

	public void Mutate(){
		tree.Mutate();
		//TODO: add set.Mutate();
	}

}
