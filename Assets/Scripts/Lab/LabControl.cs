using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor.Sprites;
#endif

using System.IO;

public class LabControl : MonoBehaviour {

	private int currentMoney = 0;//FileController.getInstance().getMoney();

	private bool[] isBought = new bool[4]; 
	private bool [] isSelected = new bool[4];

	public GameObject [] phones = new GameObject[4];
	public GameObject money_text;
	public Sprite buy;
	public Sprite select;
	public Sprite selected;
//	public Sprite [] phones_bg = new Sprite[4];

	Button [] btns = new Button[4]; 
	Text text_money;
	// Use this for initialization
	void Start () {
		isBought [0] = true;
		isSelected [0] = true;
		text_money = money_text.GetComponent<Text> ();
		text_money.text = currentMoney.ToString ();
		
        for (int count = 0; count < 4; count++) {
			btns [count] = phones [count].GetComponent<Button> ();
		}

		for (int count = 0; count < 4; count++) {
			if (isBought [count]) {
				if (isSelected [count]) {
					btns[count].GetComponent<Image> ().sprite = selected;
				} else {
					btns[count].GetComponent<Image> ().sprite = select;
				}
			} else {
				btns [count].GetComponent<Image> ().sprite = buy;
			}
		} 
		btns [0].onClick.AddListener (btn_0_function);
		btns [1].onClick.AddListener (btn_1_function);
		btns [2].onClick.AddListener (btn_2_function);
		btns [3].onClick.AddListener (btn_3_function);


	}

	void btn_0_function(){
		if (!isSelected [0]) {
			btns [0].GetComponent<Image> ().sprite = selected;

			for (int i = 0; i < 4; i++) {
				if (isSelected [i]) {
					isSelected [i] = false;
					btns [i].GetComponent<Image> ().sprite = select;
					Debug.Log (isSelected [i]);

				}
			}
			isSelected [0] = true;
		}
	}

	void btn_1_function(){

		if (isBought [1]) {
			if (!isSelected [1]) {
				btns [1].GetComponent<Image> ().sprite = selected;
			
				for (int i = 0; i < 4; i++) {
					if (isSelected [i]) {
						isSelected [i] = false;
						btns [i].GetComponent<Image> ().sprite = select;
						Debug.Log (isSelected [i]);

					}
				}
				isSelected [1] = true;
			}
		} else {
			if (currentMoney >= 10000) {
				btns [1].GetComponent<Image> ().sprite = select;
				isBought [1] = true;
				//写入数据
			}
		}

	}

	void btn_2_function(){

		if (isBought [2]) {
			if (!isSelected [2]) {
				btns [2].GetComponent<Image> ().sprite = selected;
			
				for (int i = 0; i < 4; i++) {
					if (isSelected [i]) {
						isSelected [i] = false;
						btns [i].GetComponent<Image> ().sprite = select;
					}
				}
				isSelected [2] = true;
			}
		} else {
			if (currentMoney >= 20000) {
				btns [2].GetComponent<Image> ().sprite = select;
				isBought [2] = true;
				//写入数据
			}
		}

	}

	void btn_3_function(){

		if (isBought [3]) {
			if (!isSelected [3]) {
				btns [3].GetComponent<Image> ().sprite = selected;
	
				for (int i = 0; i < 4; i++) {
					if (isSelected [i]) {
						isSelected [i] = false;
						btns [i].GetComponent<Image> ().sprite = select;
					}
				}
				isSelected [3] = true;
			}
		} else {
			if (currentMoney >= 30000) {
				btns [3].GetComponent<Image> ().sprite = select;
				isBought [3] = true;
				//写入数据
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
