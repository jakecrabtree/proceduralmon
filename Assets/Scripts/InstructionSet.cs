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

    public InstructionSet(List<Instruction> instructions){
        this.instructionSet = instructions;
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

    public InstructionSet Asexual(){
        List<Instruction> newInstructionSet = new List<Instruction>();
        for (int i = 0; i < instructionSet.Count; i++) {
            newInstructionSet.Add(instructionSet[i].copy());
        }
        return new InstructionSet(newInstructionSet);
    }

    public InstructionSet Breed(InstructionSet other){
        float type = UnityEngine.Random.Range(0.0f,1.0f);
		if(type <= Monster.CROSSOVER_CHANCE){
			//Crossover
			return Crossover(other);
		}
		else{
			//Grafting
			return Graft(other);
		}
    }

    private InstructionSet Graft(InstructionSet other) {
        List<Instruction> newInstructionSet = new List<Instruction>();
        for (int i = 0; i < instructionSet.Count; i++) {
            newInstructionSet.Add(instructionSet[i].copy());
        }
        int pos = Random.Range(0, other.instructionSet.Count);
        List<Instruction> children = new List<Instruction>();
        for (int i = pos; i < other.instructionSet.Count; ++i){
            children.Add(other.instructionSet[i].copy());
        }
        int insertionPos = (pos > newInstructionSet.Count) ? newInstructionSet.Count : pos;
        newInstructionSet.InsertRange(insertionPos, children);
        return new InstructionSet(newInstructionSet);
    }
    
    private InstructionSet Crossover(InstructionSet other){
        return Crossover(other, Random.Range(0, instructionSet.Count));
    } 

    private InstructionSet Crossover(InstructionSet other, int crossoverPoint) { //passed in
        List<Instruction> newInstructionSet = new List<Instruction>();
        for (int i = 0; i < instructionSet.Count; i++) {
            newInstructionSet.Add(instructionSet[i].copy());
        }
        List<Instruction> otherInstructionSet = new List<Instruction>();
        for (int i = 0; i < other.instructionSet.Count; i++) {
            otherInstructionSet.Add(other.instructionSet[i].copy());
        }
        newInstructionSet.RemoveRange(crossoverPoint, instructionSet.Count - crossoverPoint);
        int min = crossoverPoint < otherInstructionSet.Count ? crossoverPoint : otherInstructionSet.Count;
		otherInstructionSet.RemoveRange(0, min);
		newInstructionSet.AddRange(otherInstructionSet);
        return new InstructionSet(newInstructionSet);
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
