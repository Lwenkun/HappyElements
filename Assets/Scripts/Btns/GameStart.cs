using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour {

	public Button start_btn;

	// Use this for initialization
	void Start () {
		start_btn.onClick.AddListener (ClickToLoad);
	}
		
	void ClickToLoad(){
		SceneManager.LoadScene (1, LoadSceneMode.Single);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
