﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLoop : MonoBehaviour {

	public static MonsterLoop instance = null;
	private static readonly int GENERATION_SIZE = 1000; //TODO: Change me
	private static readonly int INITIAL_MONSTER_TREE_DEPTH = 3;

	private static readonly int MIN_REPRODUCTION_POOL_SIZE = 2;

	private static readonly float FITNESS_REPRODUCTION_CUTOFF = 100; //TODO: Change me

	private static readonly float FITNESS_WRITEOUT_CUTOFF = 200; //TODO: Change me

	private static readonly float FITNESS_EVALUATION_TIME = 3; // Seconds TODO: Change me



	private List<Monster> generation;
	private List<Monster> reproduce;
	int currentMonster;

	void Awake(){
		if (instance == null){
			instance = this;
		}
		else{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		InitializeGeneration();
	}

	void InitializeGeneration(){
		generation = new List<Monster>();
		for (int i = 0; i < GENERATION_SIZE; ++i){
			generation.Add(new Monster(INITIAL_MONSTER_TREE_DEPTH));
		}
		currentMonster = 0;
	}

	void RunGeneration(){
		for (; currentMonster < generation.Count; ++currentMonster){
			RunEvaluation(generation[currentMonster]);
			float fitness = generation[currentMonster].fitness;
			if (fitness >= FITNESS_REPRODUCTION_CUTOFF){
				reproduce.Add(generation[currentMonster]);
			}
			if (fitness >= FITNESS_WRITEOUT_CUTOFF){
				//TODO: monster write
			}
		}
		Reproduce();
		MutateNewGeneration();
		currentMonster = 0;
	}

	void Reproduce(){
		if (reproduce.Count < MIN_REPRODUCTION_POOL_SIZE){
			InitializeGeneration();
			return;
		}
		generation.Clear();
		for(int i = 0; i < GENERATION_SIZE; ++i){
			Monster parent1 = null;
			Monster parent2 = null;
			do{
				parent1 = reproduce[Random.Range(0, reproduce.Count)];
				parent2 = reproduce[Random.Range(0, reproduce.Count)];
			}while(parent1 == parent2);
			Monster child = parent1.Breed(parent2);
			generation.Add(child);
		}
		reproduce.Clear();
	}

	void MutateNewGeneration(){
		foreach(Monster monster in generation){
			monster.Mutate();
		}
	}

	void RunEvaluation(Monster monster){
		//Load empty eval scene
		monster.GenerateMonster();
		//Let it go until time cutoff
		//Evaluate
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
