using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionSet {
	//Represents an instruction set of a monster
	List<Instruction> instructionSet;

    public Monster monster;

    public InstructionSet() {
        instructionSet = new List<Instruction>();
    }
    
    public InstructionSet(int numNodes) { //a constructor that creates a set of random instructions for numNodes
		instructionSet = new List<Instruction>();
        int numInstructions = Random.Range (1, numNodes * 20);
        for(int i=0; i<numInstructions; i++) {
            int node = Random.Range(1, numNodes);
            int speed = Random.Range(-1000, 1000);
            instructionSet.Add(new Instruction(node, speed));
        }
    }
    
    public void addInstruction(Instruction i) {
        instructionSet.Add(i);
    }
    
    public void setInstruction(int index, Instruction i) {
        instructionSet.Remove(i);
        instructionSet.Insert(index, i);
    }

    public Instruction getInstruction(int index){
        return instructionSet[index];
    }

    public int getCount(){
        return instructionSet.Count;
    }
    
    public void instructionSetGraft(int node, InstructionSet list) {
        for (int i = 0; i < list.instructionSet.Count; i++){
            Instruction inst = list.instructionSet[i];
			if(inst.getNode() == node) {
                instructionSet.Add(inst);
            }
		}
    }
    
    public void instructionSetCrossover(InstructionSet other, int crossoverPoint) { //passed in
        instructionSet.RemoveRange(crossoverPoint, instructionSet.Count - crossoverPoint);
        int min = crossoverPoint < instructionSet.Count ? crossoverPoint : instructionSet.Count;
		other.instructionSet.RemoveRange(0, min);
		instructionSet.AddRange(other.instructionSet);
    }
    
    public void removeNode(int node) {
        for (int i = 0; i < instructionSet.Count; i++){
            Instruction inst = instructionSet[i];
			if(inst.getNode() == node) {
                instructionSet.Remove(inst);
                return;
            }
		}
    }
}
