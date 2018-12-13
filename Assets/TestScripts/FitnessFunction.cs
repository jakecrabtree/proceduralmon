using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization;
using System.IO;

public class FitnessFunction : MonoBehaviour {
    //public Transform creature;
    public Rigidbody rb;
    public float timeScale = 1;
    public float period = 10;
    public float timeLeft;
    public float distanceTravelled;
    public float maxDistance = 0;
    public float endVel = 0;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
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
        distanceTravelled = Vector3.Distance(new Vector3(0, 0, 0), transform.position);
        if(distanceTravelled > maxDistance)
        {
            maxDistance = distanceTravelled;
        }

        if(timeLeft <= period * 0.20f)
        {
            endVel = rb.velocity.magnitude;
        }
    }



    // Weight's the creature's value based on it's max distance from origin (measured from center of mass) and average end velocity (measured by last 20% of the period)
    float weightedValue(float maxDist, float endVel)
    {
        return 0;
    }

    /*
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
    */
}
