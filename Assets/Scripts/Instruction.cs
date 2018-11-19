using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction {
	//Represents a single instruction of a monster
	int[] instruction; 
    
    //instruction[0] = the joint number
    //instruction[1] = x torque
    //instruction[2] = y torque
    //instruction[3] = z torque
    
    public Instruction(int num, int x, int y, int z) {
        instruction = new int[4];
        instruction[0] = num;
        instruction[1] = x;
        instruction[2] = y;
        instruction[3] = z;
    }
    
    public void setNode(int num) {
        instruction[0] = num;
    }
    
    public void setX(int num) {
        instruction[1] = num;
    }
    
    public void setY(int num) {
        instruction[2] = num;
    }
    
    public void setZ(int num) {
        instruction[3] = num;
    }
    
    public int getNode() {
        return instruction[0];
    }
    
    public int getX() {
        return instruction[1];
    }
    
    public int getY() {
        return instruction[2];
    }
    
    public int getZ() {
        return instruction[3];
    }
}
