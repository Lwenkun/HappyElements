using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor.Sprites;
#endif
using System.IO;


public class RankControl : MonoBehaviour {
	public int arrLength = 10;
	public GameObject[] ranks = new GameObject[10];

	Text[] texts = new Text[10];

	// Use this for initialization
	void Start () {
		GameRecord gameRecord = GameRecord.getInstance();
		int [] scores = gameRecord.scoresRecord.scores;
		Debug.Log (scores);
		for (int count = 0; count < scores.Length; count++) {
			texts [count] = ranks [count].GetComponent<Text> ();
			texts[count].text = scores [count].ToString ();
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
