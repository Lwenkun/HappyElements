using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
	
	public Button map_1_btn;
	// Use this for initialization
	void Start () {
		map_1_btn.onClick.AddListener (ClickToStart);
	}

	void ClickToStart () {
		SceneManager.LoadScene (2, LoadSceneMode.Single);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
