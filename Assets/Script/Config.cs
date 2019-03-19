﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LevelSetting
{
    public int battleRoomNum;
    public int eventRoomNum;
    public bool bossRoom;
    public bool endRoom;
}
public class Config : MonoBehaviour
{
    public static Config instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("Deck Settings")]
    public int HandMax = 5;



    [Header("StartFloor")]
    public int floorNum = 1;
    [Header("Level Settings")]
    public LevelSetting[] LevelSettings;

    [Header("TestMode")]
    public bool RoomTestMode;
    public RoomType TestRoomType;
    public string TestRoomName;
}
