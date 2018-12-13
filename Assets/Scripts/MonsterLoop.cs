﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLoop : MonoBehaviour {

	public static MonsterLoop instance = null;
	private static readonly int INITIAL_GENERATION_SIZE = 100; //TODO: Change me
	private static readonly int INITIAL_MONSTER_TREE_DEPTH = 3;

	private static readonly float FITNESS_REPRODUCTION_CUTOFF = 100; //TODO: Change me

	private static readonly float FITNESS_WRITEOUT_CUTOFF = 200; //TODO: Change me

	private static readonly float FITNESS_EVALUATION_TIME = 3; // Seconds TODO: Change me


	private List<Monster> generation;
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
		for (int i = 0; i < INITIAL_GENERATION_SIZE; ++i){
			generation.Add(new Monster(INITIAL_MONSTER_TREE_DEPTH));
		}
		currentMonster = 0;
	}

	void RunGeneration(){

	}

	void RunEvaluation(Monster monster){
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
