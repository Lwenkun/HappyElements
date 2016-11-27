using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class back : MonoBehaviour {

	public Button back_btn;

	// Use this for initialization
	void Start () {
		back_btn.onClick.AddListener (ClickToBack);
	}

	void ClickToBack(){
		SceneManager.LoadScene (0, LoadSceneMode.Single);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
