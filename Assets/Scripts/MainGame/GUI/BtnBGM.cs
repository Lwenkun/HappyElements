using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.InteropServices.ComTypes;

public class BtnBGM : BaseUIBehaviour {

    private bool isOn;
    public Sprite[] bgmSprites;
    private Image imgBGM;

	// Use this for initialization
    void Awake () {
        isOn = Setting.BGMState;
        imgBGM = GetComponent<Image>();
        imgBGM.sprite = isOn ? bgmSprites[0] : bgmSprites[1];
	}
	
    public void ChangeBGMState() {
        isOn = !isOn;
        Setting.BGMState = isOn;
        imgBGM.sprite = isOn ? bgmSprites[0] : bgmSprites[1];
    }
}