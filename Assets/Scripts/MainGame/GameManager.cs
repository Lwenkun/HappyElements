using UnityEngine;
using System.Collections;
using System.Net.Mail;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEngine.Networking.NetworkSystem;
using System.Text;
using UnityEngine.EventSystems;
using System;
using System.Threading;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security.Principal;
using UnityEngine.SocialPlatforms.Impl;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour {

    private GameRecord.UserRecord user;
    private GameRecord.PhoneRecord phoneInUse;
    private GameRecord.ScoresRecord scoresRecord;

    private float timeLeft;
    private float totalTime;

    private int errorCount = 0;

    private int eliLevel = 0;
    
    private bool firstTurn = true;
    private bool isGameOver = false;

    public static GameManager instance;

    public UI ui;

    private int[] eliNumList;
    private int[] eliTotalNumList;

    private int scores = 0;
    private int coins = 0;

    private int costPerError;
    private int scorePerEli;

    void Awake() {

        Screen.SetResolution (Screen.width, -1, true);

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        user = GameRecord.getInstance().userRecord;
        phoneInUse = GameRecord.getInstance().phoneInUse;
        scoresRecord = GameRecord.getInstance().scoresRecord;

        ui = new UI();

        eliTotalNumList = new int[5];

        timeLeft = totalTime = 30 + (phoneInUse.elementRecords[Element.TYPE_BATTERY].level - 1) * 5.0f;

        costPerError = (20 - phoneInUse.elementRecords[Element.TYPE_GPU].level * 2);
        scorePerEli = (10 + (phoneInUse.elementRecords[Element.TYPE_CPU].level - 1) * 2);

    }

	// Use this for initialization
	void Start () {
        ui.imgTimeOut.Hide();
        ui.settingPanel.Hide();
        StartChecking();
	}

	// Update is called once per frame
	void Update () {

        if (isGameOver)
            return;

        ShowBatteryState();
        
        HandleTouchEvent();

        Element first = GameArea.instance.First;
        Element second = GameArea.instance.Second;

        //如果之后只有一个物体被选中，那么它就是焦点，将光标移至那里
        if (first != null && second == null)
        {
            GameArea.instance.MoveSelector(new Vector3(first.Pos.x, first.Pos.y), true);
        }

        //只有当用户选择两个元素时才进行消除的判断
        if (first != null && second != null)
        {

            //检查元素是否相邻，不相邻什么也不做，只处理焦点和光标
            if (CheckForSwapingEarly(first, second))
            {
                //先将两个元素在 map 中的位置交换，实际位置并没有变化
                SwapMapPosition(first, second);
                //如果当前不能消除，那么把在 map 中的位置交换回去，并且播放往复运动的平移动画
                if (!GameArea.instance.AnalyzeForCurrent())
                {
                    Debug.Log("oh no, you cannot do this");
                    OnError();
                    SwapMapPosition(first, second);
                    GameArea.instance.StartSwapAnimation(first, second, true);
                }
                else
                {
                    //能够消除，先播放动画，然后开始真正的消除
                    GameArea.instance.StartSwapAnimation(first, second, false);
                    StartChecking();
                }

                //如果相邻，无论是否能够交换，都将焦点取消
                GameArea.instance.First = null;
                GameArea.instance.Second = null;

            }
            else
            {
                //如果不相邻，那么将焦点转移到第二次选中的元素
                GameArea.instance.First = second;
                GameArea.instance.Second = null;
            }

            //如果有焦点，将光标移至焦点处，否则隐藏
            if (GameArea.instance.First == null)
            {
                //将光标移出游戏区域外，并且让其隐藏
                GameArea.instance.MoveSelector(new Vector3(-2, -2), false);   
            }
            else
            {
                //将光标移至焦点处
                GameArea.instance.MoveSelector(new Vector3(first.Pos.x, first.Pos.y), true);
            }
        }

    }

    /**
     * 如果交换后不能消除会回调此方法
     * currErrorCount 目前为止消除错误的次数总数
     */
    private void OnError() {
        errorCount++;
        scores -= costPerError;
        ui.txtError.ShowNum(errorCount);
        ui.txtScore.ShowNum(scores);
    }

    private void ShowBatteryState() {
        
        if (timeLeft < -1)
            return;

        timeLeft -= Time.deltaTime;

        float fraction = timeLeft / totalTime;

        ui.imgBatteryState.ShowBatteryState(fraction);

        if (fraction < 0)
            GameOver();

    }


    /**
     * 适配触屏手机的点击事件
     **/
    private void HandleTouchEvent() {
        
        #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        RaycastHit hit;

        if (Input.touchCount > 0) {
            Touch myTouch = Input.touches[0];
            Ray ray = Camera.main.ScreenPointToRay(myTouch.position);
            if (Physics.Raycast(ray, out hit)) {
                hit.transform.gameObject.SendMessage("OnMouseDown");
            }
        }
        #endif
    }

    private void SwapMapPosition(Element first, Element second) {
        GameArea.instance.SwapMapPosition(first, second);
    }

    private bool CheckForSwapingEarly(Element first, Element second) {
        return GameArea.instance.CheckForSwapingEarly(first, second);
    }

    private void GameOver(){
        //存储分数金币
        user.scores = scores;
        user.coins += coins;
        scoresRecord.insertScore(user.scores);
        //存储经验值
        for (int i = 0; i < 5; i++)
        {
            phoneInUse.elementRecords[i].exp += eliTotalNumList[i] * 20;
        }
        GameRecord.getInstance().Save();
        isGameOver = true;

        StartCoroutine(ui.imgTimeOut.ShowTimeOut(delegate()
                {
                    SceneManager.LoadScene(0, LoadSceneMode.Single);
                }));
    }


    private void StartChecking() {
        StartCoroutine(Eliminate());
    }


    private IEnumerator Eliminate() {

        //第一次消除暂停 1s
        if (firstTurn)
        {
            firstTurn = false;
            yield return new WaitForSeconds(1.0f);
        }

        //分析当前的 map 有没有可消除的对象，如果没有，那么就跳出循环
        while (GameArea.instance.AnalyzeForCurrent())
        {
            //如果有物体还在移动，不执行检查和消除的动作
            while (! GameArea.instance.isStatic())
                yield return null;
            
            //没有物体在移动，检查并消除
            GameArea.instance.Check();
            GameArea.instance.Adjust();
            //注意，这里的掉落对于物体在 map 中的位置是同步的，即立马发生变化；对于物体的实际位置来说是异步的，即随着 Update 变化
            GameArea.instance.Fall();

            OnTurnOver();

        }

        //如果用户不能通过交换元素消除，那么游戏结束
        if (! GameArea.instance.AnalyzeIfSwap())
        {
            Debug.Log("game over !!!");
        }

        OnEliminationPaused();

        yield return null;
    }

    /**
     * 单次或连续消除时，每轮消除完毕回调此方法
     **/
    private void OnTurnOver() {
        //先更新各种数据
        eliLevel++;
        UpdateEliNumList();
        UpdateEliTotalNumList();
        CalScoresAndCoins();
       //再更新UI
        ui.imgLevel.ShowLevel(eliLevel);
      //  ShowLevel();
        ShowTotalEliNum();
        ui.txtScore.ShowNum(scores);
      //  ShowScores();
    }

    private void UpdateEliNumList() {
        eliNumList = GameArea.instance.GetEliNumList();
    }

    private void UpdateEliTotalNumList() {
        for (int i = 0; i < 5; i++)
        {
            eliTotalNumList[i] += eliNumList[i];
        }
    }


    private void ShowTotalEliNum() {
        ui.txtScoreCPU.ShowNum(eliTotalNumList[Element.TYPE_CPU]);
        ui.txtScoreGPU.ShowNum(eliTotalNumList[Element.TYPE_GPU]);
        ui.txtScoreRAM.ShowNum(eliTotalNumList[Element.TYPE_RAM]);
        ui.txtScoreBattery.ShowNum(eliTotalNumList[Element.TYPE_BATTERY]);
        ui.txtScoreDisplay.ShowNum(eliTotalNumList[Element.TYPE_DISPLAY]);
    }

    private void CalScoresAndCoins() {
        int deltaScore = 0;
        for (int i = 0; i < 5; i++)
        {
            deltaScore += scorePerEli * eliNumList[i];
        }
        scores += deltaScore;
        coins += (int) (deltaScore * (1.5 + (phoneInUse.elementRecords[Element.TYPE_RAM].level - 1) * 0.1));
    }

    //暂时不能消除时回调此方法
    private void OnEliminationPaused() {
        Debug.Log("game pause");
        //清空连续消除的次数
        eliLevel = 0;
    }


}
