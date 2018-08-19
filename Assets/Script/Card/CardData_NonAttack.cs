﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData_NonAttack : CardData {
	public CardData_NonAttack(){}


    public override CardAbilityType GetCardAbilityType()
    {
        return CardAbilityType.Util;
    }
}

public class CardData_Reload : CardData_NonAttack{
	public CardData_Reload()
    {
        SetData();
	}

    protected override void SetData()
    {
        cardName = CardDatabase.reloadNamePath;
        spritePath = CardDatabase.reloadSpritePath;
        cardExplain = CardDatabase.reloadInfoPath;
    }
    protected override void CardActive(){
        player.CurrentHp -= 1; 
		PlayerControl.instance.ReLoadDeck ();
	}
}
