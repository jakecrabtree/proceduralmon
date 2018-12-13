using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionSet {

    public Monster monster;

	//Represents an instruction set of a monster
	List<Instruction> instructionSet;

    int numNodes;

   
    public InstructionSet() {
        instructionSet = new List<Instruction>();
    }

    public InstructionSet(List<Instruction> instructions){
        this.instructionSet = instructions;
    }
    
    public InstructionSet(int numNodes) { //a constructor that creates a set of random instructions for numNodes
		instructionSet = new List<Instruction>();
        this.numNodes = numNodes;
        int numInstructions = Random.Range (1, numNodes * 20);
        for(int i=0; i<numInstructions; i++) {
            instructionSet.Add(Instruction.RandomInstruction(1, numNodes));
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

    public void Mutate(){
        for(int i = 0; i < getCount(); i++){
            float rand = Random.Range(0.0f, 1.0f);
            if (rand <= Monster.PER_NODE_MUTATION_CHANCE){
                rand = Random.Range(0.0f, 1.0f);
                Instruction newIns = getInstruction(i);
                if (rand <= 0.6f){
                    newIns.setSpeed(newIns.getSpeed() + Random.Range(-1, 1));
                }else if (rand <= 0.8f){
                    newIns.setNode(Random.Range(1,numNodes));
                }else if (rand <= 0.9f){
                    instructionSet.Insert(i+1, Instruction.RandomInstruction(1, numNodes));
                }else if (i < getCount() - 1){
                    instructionSet.RemoveAt(i+1);
                }            
            }
        }
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
