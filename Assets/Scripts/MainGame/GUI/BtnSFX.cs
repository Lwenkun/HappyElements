using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class BtnSFX : BaseUIBehaviour {

    private bool isOn;
    private Image imgSFX;
    public Sprite[] sfxSprites;

	// Use this for initialization
	void Awake () {
        isOn = Setting.SFXState;
        imgSFX = GetComponent<Image>();
        imgSFX.sprite = isOn ? sfxSprites[0] : sfxSprites[1];
	}
	
    public void ChangeSFXState() {
        isOn = !isOn;
        Setting.SFXState = isOn;
        imgSFX.sprite = isOn ? sfxSprites[0] : sfxSprites[1];
    }


}
