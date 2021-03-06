﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public delegate void EventTileCallBack();

/// <summary>
/// UI MANAGER 
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    Text remainText;
    Text floorText;
    Text roomDebugText;
    private void Awake()
    {
        instance = this;
        mapUI = transform.Find("MapUI").GetComponent<MapUI>();
        hpUI = transform.Find("StatusUI").Find("HpUI").GetComponent<HpUI>();
        akashaUI = transform.Find("StatusUI").Find("AkashaUI").GetComponent<AkashaUI>();
        gameOverUI = transform.Find("GameOverUI").GetComponent<GameOverUI>();
        saveUI = transform.Find("SaveUI").GetComponent<SaveUI>();
        cardInfoPanel = transform.Find("CardInfoPanel").GetComponent<CardInfoPanel>();
        deckEdit = transform.Find("Deck").Find("DeckEdit").GetComponent<DeckEditUI>();
        deckCheck = transform.Find("Deck").Find("DeckCheck").GetComponent<DeckCheckUI>();
        menuUI = transform.Find("Menu").GetComponent<MenuUI>();
        endTurnButton = transform.Find("JoyStick").Find("Akasha").gameObject;

        hand = transform.Find("HandCards").Find("HandOffSet").Find("Hand").GetComponent<HandManager>();
        textUI = transform.Find("TextUI").GetComponent<TextUI>();
        buffUI = transform.Find("Anim").Find("Buff").GetComponent<BuffUI>();
        uianimations = transform.Find("Anim").GetComponent<UIAnim>();

        remainText = transform.Find("StatusUI").Find("CardRemain").GetComponentInChildren<Text>();
        floorText = transform.Find("Frame").Find("floor").GetComponentInChildren<Text>();
        roomDebugText = transform.Find("Frame").Find("roomDebugText").GetComponent<Text>();
        roomDebugText.gameObject.SetActive(false); //Change: 방번호 창 비활성화해둠
    }

    TextUI textUI;
    AkashaUI akashaUI;
    HpUI hpUI;
    GameOverUI gameOverUI;
    DeckEditUI deckEdit;
    DeckCheckUI deckCheck;
    MapUI mapUI;
    HandManager hand;
    BuffUI buffUI;
    CardInfoPanel cardInfoPanel;
    UIAnim uianimations;
    GameObject endTurnButton;
    public UIAnim uiAnim
    {
        get { return uianimations; }
    }
    SaveUI saveUI;
    MenuUI menuUI;
    #region Status
    public void HpUpdate(int currentHp_ , int fullHp)
    {
        hpUI.HpUpdate(currentHp_, fullHp);
    }
    public void AkashaUpdate(int current, int maxAks)
    {
        akashaUI.AkashaUpdate(current, maxAks);
    }

    #endregion

    #region Decks
    public void DeckEditUIOn()
    {
        if (!GameManager.instance.IsInputOk)
            return;

        deckEdit.On();
    }
    public void DeckEditUIOff()
    {
        deckEdit.Off();
    }
    public void DeckCheckUIOn()
    {
        if (!GameManager.instance.IsInputOk)
            return;

        deckCheck.On();
    }
    public void DeckCheckUIOff()
    {
        deckCheck.Off();
        GameManager.instance.IsInputOk = true;
    }
    #endregion

    #region Map
    /// <summary>
    /// 맵 이미지에 텍스쳐 설정, 크기 설정
    /// </summary>
    public void SetMapTexture(Texture2D texture, Vector2Int size)
    {
        mapUI.SetMapTexture(texture, size);
    }
    /// <summary>
    /// 미니맵 움직이기 target 포지션으로 코루틴써서 옮김
    /// </summary>
    public void MoveMiniMap(Vector3 origin, Vector3 target)
    {
        mapUI.MoveMiniMap(origin, target);
    }


    public void OpenFullMap()
    {
        mapUI.OpenFullMap();
    }
    public void CloseFullMap()
    {
        mapUI.CloseFullMap();
    }
    #endregion

    public void StatusTextUpdate()
    {
        buffUI.TextUpdate();
    }
    #region TextUI
    public void ShowTextUI(string[] s, EventTileCallBack cb)
    {
        textUI.StartText(s, cb);
    }
    public void TextUIGoNext()
    {
        textUI.GoNext();
    }
    #endregion


    #region CardInfo
    /// <summary>
    /// 해당 카드데이터 InfoPanel열기
    /// </summary>
    public void CardInfoPanel_On(Card data)
    {
        cardInfoPanel.gameObject.SetActive(true);
        cardInfoPanel.SetCard(data);
    }
    /// <summary>
    /// Unkwnon으로 열기
    /// </summary>
    public void CardInfoPanel_On()
    {
        cardInfoPanel.gameObject.SetActive(true);
        cardInfoPanel.SetUnknown();
    }
    public void CardInfoPanel_Off()
    {
        cardInfoPanel.gameObject.SetActive(false);
    }
    #endregion

    public void HideHand()
    {
        hand.HideHand();
    }

    public void UnhideHand()
    {
        hand.UnhideHand();
    }

    public void EndTurnButtonOn()
    {
        endTurnButton.SetActive(true);
    }
    public void EndTurnButtonOff()
    {
        endTurnButton.SetActive(false);
    }

    public void SaveUIOn(voidFunc OnSave)
    {
        saveUI.SaveUIOn(OnSave);
    }
    public HandManager GetHand()
    {
        return hand;
    }
    public void GameOverUIOn()
    {
        gameOverUI.On();
    }

    public void MenuUIOn()
    {
        SoundDelegate.instance.PlayMono(MonoSound.BUTTONTITLE);
        menuUI.On();
    }

    /// <summary>
    /// 귀환 버튼
    /// </summary>
    public void ReturnButton()
    {
        PlayerControl.instance.ReturnToStart();
    }
    /// <summary>
    /// 현재 덱의 남아있는 카드 수
    /// </summary>
    /// <param name="count"></param>
    public void DeckCont(int count)
    {
        remainText.text = "" + count;
    }

    public void FloorCount(int count)
    {
        floorText.text = count + "F";
    }
    public void RoomDebugText(string s,bool making = false)
    {
        if(making)
        {
            roomDebugText.text = "만드는중 : " + s;
        }
        else
        {
            roomDebugText.text = "방이름 : " + s;
        }
    }
    public void DebugButton()
    {
        uiAnim.ShowAnim("robot");
    }
}
