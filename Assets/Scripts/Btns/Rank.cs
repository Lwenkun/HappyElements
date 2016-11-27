using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Rank : MonoBehaviour {

	public Button rank_btn;
	// Use this for initialization
	void Start () {
		rank_btn.onClick.AddListener (ClickToLoad);
	}
	void ClickToLoad () {
		SceneManager.LoadScene (4, LoadSceneMode.Single);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
