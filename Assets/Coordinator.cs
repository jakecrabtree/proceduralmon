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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
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
