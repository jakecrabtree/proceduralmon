using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeInstructionSet{
	/* 
	public InstructionSet RandomizeInstructionSet(InstructionSet iSet){
		return iSet;
	}

	public InstructionSet createDeepCopy(){
		InstructionSet iSet = new InstructionSet();
		//for(int i = 0; i < )
	}
	*/

    //No new joint/segment creation, modify existing instructions
    public InstructionSet asexualRandomization(InstructionSet iSet)
    {
        InstructionSet result = new InstructionSet();
        for(int i = 0; i < iSet.getCount(); i++)
        {
            Instruction newIns = iSet.getInstruction(i).copy();
            newIns.setSpeed(newIns.getSpeed() + Random.Range(-1, 1));
            result.addInstruction(newIns);
        }
        return result;
    }
}