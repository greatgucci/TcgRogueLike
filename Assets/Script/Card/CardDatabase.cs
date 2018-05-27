﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase {
	public static CardData GetCardData(int cardIndex){

		return new CardData_Normal (cardIndex);
	}

	public const string cardResourcePath = "Card/";
	public const string cardObjectPath = "Card/CardBase";
	public static readonly string[] cardSpritePaths = {
		"card_reload",
		"card_sword",
		"card_heal"
	};
	public static readonly string[] cardNames = {
		"Re:제로",
		"짱짱쎈 칼",
		"짱짱 회복"
	};
}
