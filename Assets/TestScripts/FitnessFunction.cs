using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization;
using System.IO;

public class FitnessFunction : MonoBehaviour {
    //public Transform creature;
    Monster monster;
    public Rigidbody rb;
    public float time;
    public float distanceTravelled;
    public float endDistance = 0;
    public float weightedVelocityScore = 0;
    private static readonly float TIME_STEP = 0.2f;    

    Vector3 oldPos, newPos, startPos;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        time = 0;
        StartCoroutine(ScoreVelocity());
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if(time >= MonsterLoop.FITNESS_EVALUATION_TIME) {
            //end simulation, compare to other creatures
        }
	}

    public void AssignMonster(Monster monster){
        this.monster = monster;
    }
 
    IEnumerator ScoreVelocity()
    {
        startPos = centerOfMass();
        while (time < MonsterLoop.FITNESS_EVALUATION_TIME)
        {
            oldPos = centerOfMass();
            yield return new WaitForSeconds(TIME_STEP);
            newPos = centerOfMass();
            weightedVelocityScore += calculateVelocity(oldPos, newPos, TIME_STEP).magnitude * VelocityWeight();
        }
        endDistance = Vector3.Distance(startPos, centerOfMass());
        float fitness = (weightedVelocityScore * MonsterLoop.FITNESS_EVALUATION_TIME + endDistance) / 2;
        monster.fitness = fitness;
    }


    // Weight's the creature's value based on it's max distance from origin (measured from center of mass) and average end velocity (measured by last 20% of the period)
    float weightedValue(float maxDist, float endVel)
    {
        return 0;
    }

    // Multiply later veloiescit by the time to get the weighted, add to total distance then divide by 2


    Vector3 centerOfMass()
    {
        List<GameObject> nodes = GetComponent<Creature>().getNodes();
        Vector3 centerOfMassSum = new Vector3();
        foreach (GameObject gO in nodes)
        {
            centerOfMassSum += gO.GetComponent<Rigidbody>().centerOfMass;
        }
        centerOfMassSum.y = 0;
        return centerOfMassSum / nodes.Count;
    }

    float VelocityWeight()
    {
        return time / MonsterLoop.FITNESS_EVALUATION_TIME;
    }

    Vector3 calculateVelocity(Vector3 oldPos, Vector3 newPos, float deltaTime)
    {
        return (newPos - oldPos) / deltaTime;
    }
}
