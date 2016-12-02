using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnPause : BaseUIBehaviour {

    public SettingPanel settingPanel;
    private Image imgBtn;
    private Button btnPause;
    public Sprite imgPause;
    public Sprite imgRestart;

    private bool paused;

    void Awake() {
        imgBtn = GetComponent<Image>();
        settingPanel = GameObject.Find("SettingPanel").GetComponent<SettingPanel>();
        btnPause = GetComponent<Button>();
        btnPause.onClick.AddListener(delegate() {
            OnPauseClick();
        });
    }

    private void OnPauseClick() {

        if (paused)
        {
            imgBtn.sprite = imgPause;
            Time.timeScale = 1f;
            paused = false;
            return;
        }
        imgBtn.sprite = imgRestart;
        settingPanel.Show();
        Time.timeScale = 0f;
        paused = true;
    }   
}
