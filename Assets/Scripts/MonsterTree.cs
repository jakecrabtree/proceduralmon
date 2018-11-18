using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTree {
	MonsterTreeNode root;
	public MonsterTree breed(MonsterTree tree){
		return this;
	}
	private class MonsterTreeNode {
		public MonsterTreeNode[] children = new MonsterTreeNode[20];
		public GameObject obj;
	}
}
