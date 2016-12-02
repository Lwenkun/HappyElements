using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR

using UnityEditor.Sprites;
#endif

public class MenuControl : MonoBehaviour {

    private GameRecord gameRecord;
	private Text text_money;
	// Use this for initialization

    void Awake() {
        text_money = GetComponent<Text> ();
        gameRecord = GameRecord.getInstance();
    }

	void Start () {
        text_money.text = gameRecord.userRecord.coins.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
