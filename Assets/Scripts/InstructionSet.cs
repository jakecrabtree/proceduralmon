using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionSet {
	//Represents an instruction set of a monster
	List<Instruction> instructionSet;

    public InstructionSet() {
        instructionSet = new List<Instruction>();
    }
    
    public void addInstruction(Instruction i) {
        instructionSet.Add(i);
    }
    
    public void setInstruction(int index, Instruction i) {
        instructionSet.Remove(i);
        instructionSet.Insert(index, i);
    }

    public void getInstruction(int index){
      //  instructionSet.Get
    }

    public int getCount(){
        return instructionSet.Count;
    }
}
