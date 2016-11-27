using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Security.Cryptography.X509Certificates;

public class UI {

    public UI() {
        InitUIElement();
    }

  //  private Image imgGameOver;
    public ImgLevel imgLevel;
    public ImgTimeOut imgTimeOut;
    public ImgBatteryState imgBatteryState;

  //  private Button btnRestart;
    public Button btnPuase;
  
    public TxtNum txtScoreCPU;
    public TxtNum txtScoreGPU;
    public TxtNum txtScoreRAM;
    public TxtNum txtScoreBattery;
    public TxtNum txtScoreDisplay;
    public TxtNum txtError;
    public TxtNum txtScore;

    public SettingPanel settingPanel;


    private void InitUIElement() {

        txtScoreCPU = FindComponent<TxtNum>("TxtScoreCPU");
        txtScoreGPU = FindComponent<TxtNum>("TxtScoreGPU");
        txtScoreRAM = FindComponent<TxtNum>("TxtScoreRAM");
        txtScoreBattery = FindComponent<TxtNum>("TxtScoreBattery");
        txtScoreDisplay = FindComponent<TxtNum>("TxtScoreDisplay");
        txtScore = FindComponent<TxtNum>("TxtScore");
        txtError = FindComponent<TxtNum>("TxtError");
        btnPuase = FindComponent<Button>("BtnPause");
    
        imgLevel = FindComponent<ImgLevel>("ImgLevel");
        imgBatteryState = FindComponent<ImgBatteryState>("ImgBatteryState");
        imgTimeOut = FindComponent<ImgTimeOut>("ImgTimeOut");

        settingPanel = FindComponent<SettingPanel>("SettingPanel");
    }

    private T FindComponent<T>(string objectName){
        return GameObject.Find(objectName).GetComponent<T>();
    }

}