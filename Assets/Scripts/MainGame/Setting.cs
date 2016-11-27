using UnityEngine;
using System.Collections;

public class Setting {

    private const string SFX_STATE = "sfx_state";
    private const string BGM_STATE = "bgm_state";

    public static  bool SFXState {
        get{ 
            return PlayerPrefs.GetInt(SFX_STATE) == 1;
        }
        set { 
            if (value == true)
                PlayerPrefs.SetInt(SFX_STATE, 1);
            else
                PlayerPrefs.SetInt(SFX_STATE, 0);
        }
    }

    public static bool BGMState {
        get{ 
            return PlayerPrefs.GetInt(BGM_STATE) == 1;
        }
        set {
            if (value == true)
                PlayerPrefs.SetInt(BGM_STATE, 1);
            else
                PlayerPrefs.SetInt(BGM_STATE, 0);
        }
    }
}
