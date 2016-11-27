using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SocialPlatforms.Impl;
using System.Text;
using System.CodeDom.Compiler;

#if UNITY_EDITOR
using UnityEditorInternal.VersionControl;
#endif

using System;
using System.Runtime.CompilerServices;

/**
 * 这个类用来存取游戏数据，用法：
 * 首先获取 GameRecord 实例：
 * GameRecord r = GameRecord.getInstance();
 * 玩家数据：
 * UserRecord u = r.userRecord;
 * 玩家当前手机数据：
 * PhoneRecord p = r.phoneRecord;
 * 
 **/ 
public class GameRecord {

    private string saveDir;
    
    private static GameRecord instance;

    public const int TYPE_NORMAL = 0;
    public const int TYPE_VIRUS = 1;
    public const int TYPE_BATTERY = 2;
    public const int TYPE_DISPLAY = 3;
    public const int TYPE_CAMERA = 4;

    //用户数据
    public UserRecord userRecord;
    //用户当前手机
    public PhoneRecord phoneInUse;
    //分数排行
    public ScoresRecord scoresRecord;

    public class ScoresRecord
    {
        public int[] scores;

        public ScoresRecord() {
            scores = new int[10];
            for (int i = 0; i < 10; i++)
            {
                scores[i] = 0;
            }
        }

        public void insertScore(int s) {
            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i] > s)
                    continue;
                
                for (int j = scores.Length - 1; j > i; j--) {
                    scores[j] = scores[j - 1];
                }

                scores[i] = s;
                return;
            }
        }

    }

    private GameRecord() {
        //加载数据
        load();
        #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        saveDir = Application.dataPath + "/Resources";
        #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        saveDir = Application.persistentDataPath;
        #endif
    }

    public static GameRecord getInstance() {
        if (instance == null)
            instance = new GameRecord();
        return instance;
    }

    private void load() {
        
        userRecord = ReadUserRecord();
        scoresRecord = ReadScoresRecord();

        if (userRecord == null)
        {
            userRecord = UserRecord.GetDefaultUserRecord();

        }

        phoneInUse = GetPhoneInUse();

        if (scoresRecord == null)
        scoresRecord = new ScoresRecord();
         
    }

    //获取用户当前正在使用的手机，相当于 gameReocrd.phoneRecord;
    public PhoneRecord GetPhoneInUse() {
        if (phoneInUse != null)
            return phoneInUse;
        else
        {
            return userRecord.GetPhoneInUse();
        }
    }


    //存放手机的配件信息
    [Serializable]
    public class ElementRecord
    {   
        public ElementRecord(int level, int exp, int maxLevel, int performance) {
            this.level = level;
            this.exp = exp;
            this.maxLevel = maxLevel;
            this.performance = performance;
        }
        //配件等级
        public int level;
        //配件的经验
        public int exp;
        //配件的最大等级
        public int maxLevel;
        //配件的性能
        public int performance;
    }

    //存放用户数据
    [Serializable]
    public class UserRecord {
        //用户当前拥有的数据
        public List<PhoneRecord> phones;
        public int scores;
        public int coins;

        public UserRecord(PhoneRecord def, int scores, int coins) {
            phones = new List<PhoneRecord>();
            phones.Add(def);
            this.scores = scores;
            this.coins = coins;
        }

        public static UserRecord GetDefaultUserRecord() {
            return new UserRecord(PhoneRecord.GetDefaultPhoneRecord(true), 0, 0);
        }

        public PhoneRecord GetPhoneInUse() {
            foreach (PhoneRecord phone in phones)
            {
                if (phone.inUse)
                    return phone;
            }
            return null;
        }

        public void AddPhone(PhoneRecord p) {
            phones.Add(p);
        }

        public void DeletePhone(PhoneRecord phone) {
            phones.Remove(phone);
        }

        public void SetPhoneInUse(PhoneRecord phone) {
            foreach(PhoneRecord p in phones) {
                p.inUse = false;
            }
            phone.inUse = true;
        }

        public bool HasPhone(int type) {
            foreach (PhoneRecord p in phones)
            {
                if (p.type == type)
                    return true;
            }
            return false;
        }

        public PhoneRecord GetPhoneByType(int type) {
            foreach (PhoneRecord p in phones)
            {
                if (p.type == type)
                    return p;
            }
            return null;
        }
    }



    [Serializable]
    public class PhoneRecord{

        public int type;
        public bool inUse;

        public List<ElementRecord> elementRecords;

        public PhoneRecord(int type, bool inUse) {
            this.type = type;
            this.inUse = inUse;
            elementRecords = new List<ElementRecord>();
            for (int i = 0; i < 5 ; i ++) {
                elementRecords.Add(new ElementRecord(1, 0, maxLevelTable[type, i], performanceTable[type, i]));
            }
        }

        public static PhoneRecord GetDefaultPhoneRecord(bool inUse) {
            return new PhoneRecord(TYPE_NORMAL, inUse);
        }

        [NonSerialized]
        public static int[,] performanceTable = new int[,] {
            {1,1,1,1,1},
            {2,2,2,2,2},
            {3,3,3,3,3},
            {4,4,4,4,4},
            {5,5,5,5,5}
        };

        [NonSerialized]
        public static int[,] maxLevelTable = new int[,] {
            {5,5,5,5,5},
            {6,7,6,6,5},
            {5,5,5,9,5},
            {5,5,9,5,5},
            {5,10,5,5,5}
        };

    }

    public void SetPhoneInUse(PhoneRecord p) {
        phoneInUse = p;
        userRecord.SetPhoneInUse(p);
    }

    public void DeletePhone(PhoneRecord phone) {
        userRecord.DeletePhone(phone);
    }
	
    private T ReadRecord<T>(string jsonPath) {

        //string path = Application.dataPath + "/Resources/Test.json";
        if (!File.Exists(jsonPath))
        {
            return default(T);
        }

        StreamReader sr = new StreamReader(jsonPath);

        if (sr == null)
        {
            return default(T);
        }

        string json = sr.ReadToEnd();

        if (json.Length > 0)
        {
            return JsonUtility.FromJson<T>(json);
        }

        return default(T);
    }

    private void WriteRecord(object obj, string path) {
        string json = JsonUtility.ToJson(obj);
        Debug.Log("score record --> " + json);
        File.WriteAllText(path, json, Encoding.UTF8);        
        Debug.Log("save:::" + path);
    }

    private UserRecord ReadUserRecord() {
        return ReadRecord<UserRecord>(saveDir + "/UserRecord.json");
    }

    private void WriteUserRecord(UserRecord userRecord) {
        string savePath = saveDir + "/UserRecord.json";
        WriteRecord(userRecord, savePath);
        Debug.Log("save:::" + savePath);
    }

    private ScoresRecord ReadScoresRecord() {
        string path = saveDir + "/ScoresRecord.json";
        //return null;
        return ReadRecord<ScoresRecord>(path);
    }

    private void WriteScoresRecord(ScoresRecord record) {
        string path =  saveDir + "/ScoresRecord.json";    
        WriteRecord(record , path);
    }

    public void Save() {
        WriteUserRecord(userRecord);
        WriteScoresRecord(scoresRecord);
    }





}
