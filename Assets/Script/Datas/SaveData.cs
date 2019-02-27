﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System;
using System.Text;

[Serializable]
public class SaveData1
{
    public SaveData1()
    {
        //default value
        cardUnlockData = new Dictionary<int, bool>();
        diaryUnlockData = new Dictionary<int, bool[]>();
        monsterKillData = new Dictionary<int, int>();
        stageArriveData = new Dictionary<int, bool>();
        achiveUnlockData = new Dictionary<int, bool>();
        isGetEnding = false;
        gameOverNum = 0;
        isSet = false;
    }
    public float bgmValue;
    public float fxValue;
    public float UIValue;
    public Dictionary<int, bool> cardUnlockData;
    public Dictionary<int, bool> achiveUnlockData;
    public Dictionary<int, bool[]> diaryUnlockData; // [0] 일지 해금여부, [1] 새로운 일지인지 여부
    public Dictionary<int, int> monsterKillData;
    public Dictionary<int, bool> stageArriveData; //TODO: 미구현
    public bool isGetEnding;
    public uint gameOverNum;
    public bool isSet;

}

/// <summary>
/// Datas that will be used for save & load
/// </summary>
public static class SaveData
{
    private static SaveData1 saveData;
    private static string Ext = ".dat";
    private static string FileName = "save";
    private static string Path = Application.persistentDataPath;

    #region FirstSetUp
    public static void FirstSetUp()
    {
        bool loadComplete = JsonLoad(FileName, Path);
        if (saveData == null)
            saveData = new SaveData1();
        if (saveData.isSet)
        {
            InitCardUnlockDatas();
            InitAchiveUnlockDatas();
            InitDiaryUnlockDatas();
            InitMonsterKillDatas();
            InitStageArriveDatas();
            return;
        }
        else
        {
            SetBgmValue(1f);
            SetFxValue(1f);
            SetUIValue(1f);
            InitCardUnlockDatas();
            InitAchiveUnlockDatas();
            InitDiaryUnlockDatas();
            InitMonsterKillDatas();
            InitStageArriveDatas();
            saveData.isSet = true;
        }

        Debug.Log(JsonConvert.SerializeObject(saveData));
        
        if (!loadComplete)
        {
            JsonSave(saveData, FileName, Path);
        }
    }
    #endregion

    #region defaultSetter
    private static void SetCardUnlockData(int i, bool isUnlock)
    {
        if (!saveData.cardUnlockData.ContainsKey(i))
        {
            Debug.LogWarning("키가 딕셔너리에 존재하지 않습니다.");
            //saveData.cardUnlockData.Add(i, isUnlock);
            return;
        }
        saveData.cardUnlockData[i] = isUnlock;
    }
    private static void SetAchiveUnlockData(int i, bool isUnlock)
    {
        if (!saveData.achiveUnlockData.ContainsKey(i))
        {
            Debug.LogWarning("키가 딕셔너리에 존재하지 않습니다.");
            //saveData.achiveUnlockData.Add(i, isUnlock);
            return;
        }
        saveData.achiveUnlockData[i] = isUnlock;
    }
    private static void SetDiaryUnlockData(int i, bool isUnlock)
    {
        if (!saveData.diaryUnlockData.ContainsKey(i))
        {
            Debug.LogWarning("키가 딕셔너리에 존재하지 않습니다.");
            //saveData.diaryUnlockData.Add(i, new bool[] { isUnlock, false });
            return;
        }
        saveData.diaryUnlockData[i][0] = isUnlock;
        if (isUnlock == true) saveData.diaryUnlockData[i][1] = true;
        else saveData.diaryUnlockData[i][1] = false;
    }
    private static void SetMonsterKillData(int i, int value)
    {
        if (!saveData.monsterKillData.ContainsKey(i))
        {
            Debug.LogWarning("키가 딕셔너리에 존재하지 않습니다.");
            //saveData.monsterKillData.Add(i, value);
            return;
        }
        saveData.monsterKillData[i] = value;
    }
    private static void SetStageArriveData(int i, bool isUnlock)
    {
        if (!saveData.stageArriveData.ContainsKey(i))
        {
            Debug.LogWarning("키가 딕셔너리에 존재하지 않습니다.");
            saveData.stageArriveData.Add(i, isUnlock);
            //return;
        }
        saveData.stageArriveData[i] = isUnlock;

    }

    private static void InitCardUnlockDatas()
    {
        foreach (KeyValuePair<int, CardData> pair in Database.cardDatas)
        {
            if (saveData.cardUnlockData.ContainsKey(pair.Key)) continue;
            saveData.cardUnlockData.Add(pair.Key, false);
        }
    }
    private static void InitAchiveUnlockDatas()
    {
        foreach (KeyValuePair<int, AchiveData> pair in Database.achiveDatas)
        {
            if (saveData.achiveUnlockData.ContainsKey(pair.Key)) continue;
            saveData.achiveUnlockData.Add(pair.Key, false);
        }
    }
    private static void InitDiaryUnlockDatas()
    {
        foreach (KeyValuePair<int, DiaryData> pair in Database.diaryDatas)
        {
            if (saveData.diaryUnlockData.ContainsKey(pair.Key)) continue;
            saveData.diaryUnlockData.Add(pair.Key, new bool[] { false, false });
        }
    }
    private static void InitMonsterKillDatas()
    {
        foreach (KeyValuePair<int, MonsterData> pair in Database.monsterDatas)
        {
            if (saveData.monsterKillData.ContainsKey(pair.Key)) continue;
            saveData.monsterKillData.Add(pair.Key, 0);
        }
    }
    private static void InitStageArriveDatas()
    {

    }

    #endregion

    #region ForDebug
    public static void SetDiaryAllTrue()
    {
        foreach (KeyValuePair<int, DiaryData> pair in Database.diaryDatas)
        {
            SetDiaryUnlockData(pair.Key, true);
        }
    }
    public static void SetCardAllTrue()
    {
        foreach (KeyValuePair<int, CardData> pair in Database.cardDatas)
        {
            SetCardUnlockData(pair.Key, true);
        }
    }
    #endregion

    #region Getter
    public static bool GetIsSet()
    {
        return saveData.isSet;
    }
    public static float GetBgmValue()
    {
        return saveData.bgmValue;
    }
    public static float GetFxValue()
    {
        return saveData.fxValue;
    }
    public static float GetUIValue()
    {
        return saveData.UIValue;
    }
    public static bool GetCardUnlockData(int i)
    {
        return saveData.cardUnlockData[i];
    }
    public static bool GetAchiveUnlockData(int i)
    {
        return saveData.achiveUnlockData[i];
    }
    public static bool[] GetDiaryUnlockData(int i)
    {
        if (!saveData.diaryUnlockData.ContainsKey(i))
            Debug.Log("Key" + i);
        return saveData.diaryUnlockData[i];
    }
    public static int GetMonsterKillData(int i)
    {
        return saveData.monsterKillData[i];
    }
    public static bool GetStageArriveData(int i)
    {
        return saveData.stageArriveData[i];
    }
    public static int CardDataCount { get { return saveData.cardUnlockData.Count; } }
    public static int AchiveDataCount { get { return saveData.achiveUnlockData.Count; } }
    public static int DiaryDataCount { get { return saveData.diaryUnlockData.Count; } }
    public static int MonsterDataCount { get { return saveData.monsterKillData.Count; } }
    public static int StageDataCount { get { return saveData.stageArriveData.Count; } }
    #endregion

    public static void SetBgmValue(float value)
    {
        saveData.bgmValue = value;
        if (SoundDelegate.instance != null)
            SoundDelegate.instance.BGMSound = saveData.bgmValue;
    }
    public static void SetFxValue(float value)
    {   
        fxValue = value;
        SoundDelegate.instance.EffectSound = fxValue;//효과음 조절
    }
    public static void SetUIValue(float value)
    {   //TODO: UI 투명도 조절
        saveData.UIValue = value;
    }

    /// <summary>
    /// 몬스터를 죽일 시 콜
    /// </summary>
    /// <param name="i"> DB상의 몬스터 번호 </param>
    public static void killMonster(int i)
    {
        saveData.monsterKillData[i]++;
        if (saveData.monsterKillData[i] - 1 > 0) return; //버그시 확인 필

        for (int idx = 1; idx <= Database.achiveDatas.Count; idx++)
        {
            if (Database.achiveDatas[idx].type == "kill" && int.Parse(Database.achiveDatas[idx].condition) == i)
            {
                GetAchivement(idx);
            }
        }
    }

    /// <summary>
    /// 아래 함수를 제외한 특수한 업적달성시 콜 할 것
    /// </summary>
    /// <param name="i">업적번호</param>
    public static void GetAchivement(int i)
    {
        saveData.achiveUnlockData[i] = true;
        if (Database.achiveDatas[i].reward.Length != 0)
        {
            SetDiaryUnlockData(int.Parse(Database.achiveDatas[i].reward), true);
        }
        if (Database.achiveDatas[i].cardReward.Length != 0)
        {
            SetCardUnlockData(int.Parse(Database.achiveDatas[i].cardReward), true);
        }
    }



    /// <summary>
    /// 랜덤하게 카드를 언락하는 함수
    /// </summary>
    public static void GetRandomCard()
    {
        List<int> unobtainedCards = new List<int>();
        foreach (KeyValuePair<int, CardData> pair in Database.cardDatas)
        {
            if (GetCardUnlockData(pair.Key) == false)
                unobtainedCards.Add(pair.Key);
        }
        int randomIdx = UnityEngine.Random.Range(0, unobtainedCards.Count);
        SetCardUnlockData(unobtainedCards[randomIdx], true);

    }

    /// <summary>
    /// 스테이지 도달 시 콜
    /// </summary>
    /// <param name="i">도달한 층</param>
    public static void ArriveStage(int i)
    {
        SetStageArriveData(i, true);
        for (int idx = 1; idx <= Database.achiveDatas.Count; idx++)
        {
            if (Database.achiveDatas[idx].type == "floor" && int.Parse(Database.achiveDatas[idx].info) == i)
            {
                GetAchivement(idx);
            }
        }
    }

    /// <summary>
    /// 엔딩 달성 시 콜
    /// </summary>
    public static void GetEnding()
    {
        saveData.isGetEnding = true;
        for (int idx = 1; idx <= Database.achiveDatas.Count; idx++)
        {
            if (Database.achiveDatas[idx].type == "ending")
            {
                GetAchivement(idx);
            }
        }
        SceneManager.LoadScene("EndingScene");
    }

    /// <summary>
    /// 게임오버 시 콜
    /// </summary>
    public static void GetGameOver()
    {
        saveData.gameOverNum++;
        for (int idx = 1; idx <= Database.achiveDatas.Count; idx++)
        {
            if (Database.achiveDatas[idx].type == "gameover" && int.Parse(Database.achiveDatas[idx].info) <= saveData.gameOverNum)
            {
                GetAchivement(idx);
            }
        }
    }


    public static bool CheckNew()
    {
        for (int i = 1; i < saveData.diaryUnlockData.Count; i++)
        {
            if (saveData.diaryUnlockData[i][1] == true) return true;
        }
        return false;
    }

    public static void ChangeNewToOld(int i)
    {
        saveData.diaryUnlockData[i][1] = false;
    }

    #region For Save/Load
    public static void LoadFile()
    {
    }

    public static byte[] DataToByte()
    {
        string json = JsonConvert.SerializeObject(saveData);
        return Encoding.UTF8.GetBytes(json); ;
    }
    public static void ByteToData(byte[] byteArr)
    {
        string json = Encoding.UTF8.GetString(byteArr);
        Debug.Log("loaded save: " + json);
        if (json.Length == 0)
        {
            Debug.Log("0 lenth data from cloud, exit");
            return;
        }
        saveData = JsonConvert.DeserializeObject<SaveData1>(json);
        return;
    }

    public static bool JsonSave(SaveData1 data, string filename, string path)
    {
        string json = JsonConvert.SerializeObject(data);
        Encoding.UTF8.GetBytes(json);
        FileStream file = new FileStream(path + "/" + filename + Ext, FileMode.Create);
        if (file == null)
        {
            Debug.LogError("file Create Error!");
            file.Close();
            return false;
        }
        StreamWriter writer = new StreamWriter(file);
        writer.Write(json);

        writer.Close();
        Debug.Log("SaveComplete" + json);
        return true;
    }

    public static bool JsonLoad(string filename, string path)
    {
        FileStream file = new FileStream(path + "/" + filename + Ext, FileMode.OpenOrCreate);
        StreamReader reader = new StreamReader(file);
        string json = reader.ReadToEnd();
        if (file.Length == 0)
        {
            Debug.Log("make new savefile");
            reader.Close();
            return false;
        }
        saveData = JsonConvert.DeserializeObject<SaveData1>(json);
        reader.Close();
        Debug.Log("SaveFile Loaded" + json);
        return true;
    }
    #endregion
}
