using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTree {
	MonsterTreeNode root;

	public MonsterTree breed(MonsterTree tree){
		return this;
	}
}

public class MonsterTreeNode {
	MonsterTreeNode[] children = new MonsterTreeNode[20];
	GameObject obj;
}
