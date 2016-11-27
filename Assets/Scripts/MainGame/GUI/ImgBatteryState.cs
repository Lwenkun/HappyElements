using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImgBatteryState : BaseUIBehaviour {

    private Image imgBatteryState;
    public Sprite[] batteryStates;

    void Awake() {
        imgBatteryState = GetComponent<Image>();
    }

    public void ShowBatteryState(float fraction) {
//        if (fraction < -1)
//            return;

        int batteryIndex = 0;

        if (fraction < 0f)
        {
            batteryIndex = 0;
//            GameOver();
        }
        else if (fraction < 0.25f)
        {
            batteryIndex = 1;
        }
        else if (fraction < 0.5f)
        {
            batteryIndex = 2;
        }
        else if (fraction < 0.75f)
        {
            batteryIndex = 3;
        }
        else if (fraction < 1.0f)
        {
            batteryIndex = 4;
        }

        imgBatteryState.sprite = batteryStates[batteryIndex];
    }
}
