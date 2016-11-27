using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BtnPause : BaseUIBehaviour {

    public SettingPanel settingPanel;
    private Button btnPause;

    private bool paused;

    void Awake() {
        settingPanel = GameObject.Find("SettingPanel").GetComponent<SettingPanel>();
        btnPause = GetComponent<Button>();
        btnPause.onClick.AddListener(delegate() {
            OnPauseClick();
        });
    }

    private void OnPauseClick() {

        if (paused)
        {
            Time.timeScale = 1f;
            paused = false;
            return;
        }
        settingPanel.Show();
        Time.timeScale = 0f;
        paused = true;
    }   
}
