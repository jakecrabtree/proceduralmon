using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterLoop : MonoBehaviour {

	public static MonsterLoop instance = null;
	private static readonly int GENERATION_SIZE = 200; 
	private static readonly int MIN_REPRODUCTION_SIZE = 40; 

	private static readonly int INITIAL_MONSTER_TREE_DEPTH = 3;

	private static readonly float FITNESS_REPRODUCTION_CUTOFF = 12; 

	private static readonly float FITNESS_WRITEOUT_CUTOFF = 50; 

	public static readonly float FITNESS_EVALUATION_TIME = 20; 

	public static readonly float FITNESS_EVALUATION_TIME_SCALE = 8f;


	private List<Monster> generation;
	private List<Monster> reproduce;
	int currentMonster;
	GameObject currentObject;

	float totalFitness = 0;

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
		Time.timeScale = FITNESS_EVALUATION_TIME_SCALE;
		StartCoroutine(RunGeneration());
	}

	void InitializeGeneration(){
		generation = new List<Monster>();
		for (int i = 0; i < GENERATION_SIZE; ++i){
			generation.Add(new Monster(INITIAL_MONSTER_TREE_DEPTH));
		}
		currentMonster = 0;
	}

	IEnumerator RunGeneration(){
		Debug.Log("New Generation");
		reproduce = new List<Monster>();
		totalFitness = 0;
		 for (; currentMonster < generation.Count; ++currentMonster){
			currentObject = generation[currentMonster].GenerateMonster();
			yield return StartCoroutine(currentObject.GetComponent<FitnessFunction>().ScoreVelocity());
			float fitness = generation[currentMonster].fitness;
			Debug.Log(fitness);
			if (fitness >= FITNESS_REPRODUCTION_CUTOFF){
				totalFitness += fitness;
				reproduce.Add(generation[currentMonster]);
			}
			if (fitness >= FITNESS_WRITEOUT_CUTOFF){
				generation[currentMonster].WriteToFile();
			}
			currentObject.GetComponent<Creature>().DestroyCreature();
		}
		Reproduce();
		MutateNewGeneration();
		currentMonster = 0;
		StartCoroutine(RunGeneration());
		yield break;
	}

	Monster SelectParent(){
		float rand = Random.Range(0, totalFitness);
		float total = 0;
		foreach(Monster parent in reproduce){
			if (rand >= total && rand < total+parent.fitness){
				return parent;
			}
			total+=parent.fitness;
		}
		return null;
	}
	void Reproduce(){
		generation.Clear();
		if (reproduce.Count < MIN_REPRODUCTION_SIZE){
			foreach(Monster monster in reproduce){
				generation.Add(monster);
			}
			while (generation.Count < GENERATION_SIZE){
				generation.Add(new Monster(INITIAL_MONSTER_TREE_DEPTH));
			}
			return;
		}
		for(int i = 0; i < GENERATION_SIZE; ++i){
			Monster parent1 = null;
			Monster parent2 = null;
			do{
				parent1 = SelectParent();
				parent2 = SelectParent();
			}while(parent1 == parent2 || parent1 == null || parent2 == null);
			Monster child = parent1.Breed(parent2);
			generation.Add(child);
		}
		reproduce.Clear();
	}

	void MutateNewGeneration(){
		//foreach(Monster monster in generation){
	//		monster.Mutate();
	//	}
	}
}
