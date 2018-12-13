using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {
	bool isSetup = false;
    bool shouldWalk = true;
	List<GameObject> nodes = null;
	InstructionSet myInstructions;
	int pc = 0;
	long semitick = 0;
	public void nodeSetup(List<GameObject> n) {
		nodes = n;
		myInstructions = new InstructionSet (n.Count);
		isSetup = true;
	}

	public void nodeSetup(List<GameObject> n, InstructionSet set) {
		nodes = n;
		myInstructions = set;
		isSetup = true;
	}
    public void setShouldWalk(bool s) {
        shouldWalk = s;
    }
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Gets instruction and set's the JointMotor's current velocity
	void FixedUpdate () {
		if (isSetup && shouldWalk && (semitick++ & 15) == 0 && myInstructions.getCount() > 0) {
			Instruction curr = myInstructions.getInstruction (pc++);
			JointMotor jm = nodes [curr.getNode ()].transform.GetComponent<HingeJoint> ().motor;
			jm.targetVelocity = curr.getSpeed();
			nodes [curr.getNode ()].transform.GetComponent<HingeJoint> ().motor = jm;
			pc %= myInstructions.getCount ();
		}
	}

    public List<GameObject> getNodes()
    {
        return nodes;
    }
}
