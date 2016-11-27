using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class labBack : MonoBehaviour {

	public Button lab_back_btn;
	// Use this for initialization
	void Start () {
		lab_back_btn.onClick.AddListener (ClickToBack);
	}

	void ClickToBack (){
		SceneManager.LoadScene (0, LoadSceneMode.Single);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
