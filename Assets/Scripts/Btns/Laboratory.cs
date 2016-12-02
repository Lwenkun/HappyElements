using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class Laboratory : MonoBehaviour {

    public AudioClip btnEfx;

	public Button lab_btn;

	// Use this for initialization
	void Start () {
		lab_btn.onClick.AddListener (ClickBtn);
	}

	void ClickBtn(){
        SoundManager.instance.playSingle(btnEfx);
		SceneManager.LoadScene (3, LoadSceneMode.Single);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
