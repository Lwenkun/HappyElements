using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TxtCoins : MonoBehaviour {

    private Text txtCoins;
    private GameRecord gameRecord;

	// Use this for initialization
	void Awake () {
        txtCoins = GetComponent<Text>();
        gameRecord = GameRecord.getInstance(); 
	}

    void ShowCoins() {
        txtCoins.text = gameRecord.userRecord.coins.ToString();
    }
	
}
