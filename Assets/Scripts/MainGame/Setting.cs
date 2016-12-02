using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;

public class Setting {

    private const string SFX_STATE = "sfx_state";
    private const string BGM_STATE = "bgm_state";
    private const string IS_FIRST_PALY = "is_first_play";

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

//    public static bool IsFirstPlay {
//        get {
//            if (PlayerPrefs.GetInt(IS_FIRST_PALY) != 100)
//            {
//                PlayerPrefs.SetInt(IS_FIRST_PALY, 100);
//                return true;
//            }
//            return false;
//        }
//    }


}
