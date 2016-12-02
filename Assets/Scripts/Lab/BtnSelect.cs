using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Security.Cryptography.X509Certificates;
using System.Resources;
using System.CodeDom;
#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine.Networking;

public class BtnSelect : MonoBehaviour {
    

    public AudioClip buyEfx;
    public AudioClip selectEfx;

    private static List<BtnSelect> btns = new List<BtnSelect>();

    private Text money;
    private GameRecord gameRecord;
    private GameRecord.PhoneRecord phoneRecord;

    public Sprite[] btnStateImage;

    private const int INDEX_SELECTED = 0x00;
    private const int INDEX_UNSELECTED = 0x01;
    private const int INDEX_NOTHAVE = 0x02;

    public int price;
    public int type;

    public const int STATE_SELECTED = 0x00;
    public const int STATE_UNSELECTED = 0x01;
    public const int STATE_NOTHAVE = 0x02;

    private int state;

    private Button btn;
    private Image img;
	// Use this for initialization
	void Awake () {
        btn = GetComponent<Button>();
        img = GetComponent<Image>();
        btn.onClick.AddListener(delegate() {
            OnClick();
        });
        money = GameObject.Find("money_text").GetComponent<Text>();
        gameRecord = GameRecord.getInstance();
        InitState();
	}

    private void Register() {
        btns.Add(this);
    }

    private void Unregister() {
        btns.Remove(this);
    }

    private void InitState() {
        
        phoneRecord = gameRecord.userRecord.GetPhoneByType(type);
        if (phoneRecord == null)
        {
            state = STATE_NOTHAVE;
        }
        else
        {
            Register();
            if (gameRecord.phoneInUse.type == type)
            {
                state = STATE_SELECTED;
            }
            else
            {
                state = STATE_UNSELECTED;
            }
        }

        ChangeBtnState(state);

        Debug.Log("type: " + type + ", state: " + state);
       
    }

    private void OnClick() {
        if (state == STATE_SELECTED)
        {
            Debug.Log("type: " + type + ", this phone is already in use");
            return;
        }
        else if (state == STATE_UNSELECTED)
        {
            gameRecord.SetPhoneInUse(phoneRecord);
            ChangeBtnState(STATE_SELECTED);
            UnselectedOthers();
            SoundManager.instance.playSingle(selectEfx);
            Debug.Log("type: " + type + ", you select this phone");
        }
        else if (state == STATE_NOTHAVE)
        {
            if (HasEnoughMoney())
            {
                gameRecord.userRecord.coins -= price;
                money.text = gameRecord.userRecord.coins.ToString();
                ChangeBtnState(STATE_UNSELECTED);
                phoneRecord = new GameRecord.PhoneRecord(type, false);
                gameRecord.userRecord.AddPhone(phoneRecord);
                Register();
                SoundManager.instance.playSingle(buyEfx);
                Debug.Log("type: " + type + ", congratulation, you bought a new phone successfully");
            }
            else
            {
                Debug.Log("type: " + type + ", sorry, but you don't have enough money to buy this phone");
            }

        }
        gameRecord.Save();

    }

    public bool HasEnoughMoney() {
        return gameRecord.userRecord.coins >= price;
    }


    public void ChangeBtnState(int state) {
        this.state = state;
        Sprite nextState = null;
        switch (state)
        {
            case STATE_NOTHAVE:
                nextState = btnStateImage[INDEX_NOTHAVE];
                break;
            case STATE_SELECTED:
                nextState = btnStateImage[INDEX_SELECTED];
                break;
            case STATE_UNSELECTED:
                nextState = btnStateImage[INDEX_UNSELECTED];
                break;
        }
        if (nextState != null)
        {
            img.sprite = nextState;
        }

    }

    private void UnselectedOthers() {
        foreach (BtnSelect b in btns)
        {
            if (b == this)
                continue;
            b.ChangeBtnState(STATE_UNSELECTED);
            Debug.Log("other btns' state changed");
        }
    }

    void OnDestroy() {
        Unregister();
    }
}