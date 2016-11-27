using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR

using UnityEditor.Sprites;
#endif

public class MenuControl : MonoBehaviour {

	public GameObject money_text;
	private int currentMoney = 8182;
	Text text_money;
	// Use this for initialization
	void Start () {
		text_money = money_text.GetComponent<Text> ();
		text_money.text = currentMoney.ToString ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
