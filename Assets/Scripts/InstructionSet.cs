using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionSet {

    public Monster monster;

	//Represents an instruction set of a monster
	List<Instruction> instructionSet;

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
    
    public List<Instruction> instructionSetGraft(int node, InstructionSet list) {
        List<Instruction> newInstructionSet = new List<Instruction>();
        for (int i = 0; i < instructionSet.Count; i++) {
            newInstructionSet.Add(instructionSet[i]);
        }
        for (int i = 0; i < list.instructionSet.Count; i++){
            Instruction inst = list.instructionSet[i];
			if(inst.getNode() == node) {
                newInstructionSet.Add(inst);
            }
		}
        return newInstructionSet;
    }
    
    public List<Instruction> instructionSetCrossover(InstructionSet other, int crossoverPoint) { //passed in
        List<Instruction> newInstructionSet = new List<Instruction>();
        for (int i = 0; i < instructionSet.Count; i++) {
            newInstructionSet.Add(instructionSet[i]);
        }
        newInstructionSet.RemoveRange(crossoverPoint, instructionSet.Count - crossoverPoint);
        int min = crossoverPoint < instructionSet.Count ? crossoverPoint : instructionSet.Count;
		other.instructionSet.RemoveRange(0, min);
		newInstructionSet.AddRange(other.instructionSet);
        return newInstructionSet;
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
