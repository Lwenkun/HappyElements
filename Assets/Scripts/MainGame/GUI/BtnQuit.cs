using UnityEngine;
using System.Collections;

public class BtnQuit : BaseUIBehaviour {
    
    private SettingPanel settingPanel;

    public void _Quit() {
        if (settingPanel == null)
        {
            settingPanel = GameObject.Find("SettingPanel").GetComponent<SettingPanel>();
        }
        settingPanel.Hide();
    }
}
