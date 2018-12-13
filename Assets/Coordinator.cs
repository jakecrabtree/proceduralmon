using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Coordinator : MonoBehaviour {
	public bool isOver = false;
	public bool hasSelected = false;
	public int current = 0;
	public Text disp;
	public GameObject[] creatures = new GameObject[6];
	bool hasMonsters = false;
	public Monster[] monsters = new Monster[6];
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	public Monster getMonster(int id) {
		int attempts = 0;
		if (!hasMonsters) {
			string[] f = System.IO.Directory.GetFiles ("Monsters", "*.mon");
			int[] ids = new int[6];
			for(int i = 0; i < 6; i++) {
				while (attempts < 30) {
					attempts++;
					int fid = Random.Range (0, f.Length);
					bool isGood = true;
					for (int j = 0; j < i; j++) {
						if (fid == ids [j]) {
							isGood = false;
							break;
						}
					}
					if (isGood) {
						ids [i] = fid;
						bool hasMonster = true;
						try{
							return monsters[id];
							monsters [i] = Monster.ReadFromFile (f [fid]);
						} catch {
							hasMonster = false;
							Debug.Log ("Caught bad file");
							int count = 0;
							while (count < 100) {
								count++;
								bool deleted = false;
								try {
									System.IO.File.Delete (f [fid]);
									deleted = true;
								} catch {
								}
								if (deleted) {
									break;
								}
							}
							f = System.IO.Directory.GetFiles ("Monsters", "*.mon");
						}
						if (hasMonster) {
							break;
						}
					}
				}
			}
			hasMonsters = true;
		}
		Debug.Log (attempts);
		return monsters [id];
	}

	public void setOver(int winner) {
		if (!isOver) {
			isOver = true;
			if (winner == current) {
				disp.text = "You Won!";
			} else {
				disp.text = "You Lost :(";
			}
		}
	}
}
