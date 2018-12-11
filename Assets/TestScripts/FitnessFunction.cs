using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization;
using System.IO;

public class FitnessFunction : MonoBehaviour {
    public Transform creature;
    public float timeScale = 1;
    public float period = 10;
    public float timeLeft;
    public float distanceTravelled;

	// Use this for initialization
	void Start () {
        Time.timeScale = timeScale;
        timeLeft = period;
	}
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0) {
            //end simulation, compare to other creatures
        }
	}

    void FixedUpdate ()
    {
        distanceTravelled = Vector3.Distance(new Vector3(0, 0, 0), creature.position);
    }

    void Save()
    {
        //BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/creatureData.dat", FileMode.Open);

        CreatureData data = new CreatureData();
    }

    class CreatureData
    {
        public float distanceTravelled;
        public MonsterTree monsterTree;
    }
}
