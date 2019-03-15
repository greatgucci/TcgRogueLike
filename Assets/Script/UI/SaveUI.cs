﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUI : MonoBehaviour
{
    Vector3 offPos = new Vector3(0, -5000, 0);
    float originVolume;

    public void SaveUIOn()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        originVolume = SoundDelegate.instance.BGMSound;
        SoundDelegate.instance.BGMSound = 0;
    }

    public void OnYesButtonDown()
    {
        InGameSaveManager.WriteAndSave
            (GameManager.instance.CurrentMap.Floor,
            PlayerControl.player.GetHp,
            CardsToNumber(PlayerControl.instance.DeckManager.Deck),
            CardsToNumber(PlayerControl.instance.DeckManager.AttainCards),
             GameManager.instance.BuildSeed,
                GameManager.instance.Pablus,
                GameManager.instance.Xynus);

        SaveManager.SaveAll();
        LoadingManager.LoadScene("Levels/MainMenu");      
    }
    public void OnNoButtonDown()
    {
        transform.localPosition = offPos;
        SoundDelegate.instance.BGMSound = originVolume;
    }

    private List<int> CardsToNumber(List<Card> datas)
    {
        List<int> numbers = new List<int>();
        for (int i = 0; i < datas.Count; i++)
        {
            numbers.Add(datas[i].Index);
        }
        return numbers;
    }
}