using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TxtNum : BaseUIBehaviour {

    private Text txtNum;
	// Use this for initialization
	void Awake () {
        txtNum = GetComponent<Text>();
	}
	
    public void ShowNum(int count) {
        txtNum.text = count.ToString();
    }
}