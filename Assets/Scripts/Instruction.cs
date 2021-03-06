using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction {
	//Represents a single instruction of a monster
	int jointNumber; 
    float speed;
    
    //instruction[0] = the joint number
    //instruction[1] = speed

    
    public Instruction(int num, float sp) {
        jointNumber = num;
        speed = sp;
    }
    
    public void setNode(int num) {
        jointNumber = num;
    }
    
    public void setSpeed(float num) {
        speed = num;
    }
    
    public int getNode() {
        return jointNumber;
    }
    
    public float getSpeed() {
        return speed;
    }
    
    //Returns a deep copy of an instruction
    public Instruction copy(){
        return new Instruction(jointNumber, speed);
    }

    public static Instruction RandomInstruction(int minNode, int maxNode, float minSpeed = 100, float maxSpeed = 150){
        int sign = Random.value < .5? 1 : -1;
        return new Instruction(Random.Range(minNode, maxNode), sign*Random.Range(minSpeed, maxSpeed));
    }
}
