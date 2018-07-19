﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData_Magic : CardData {
	public CardData_Magic(){}
	public CardData_Magic(int index,Player pl) : base(index,pl){

	}


	public override void CardActive (){

	}
}

public class CardData_Reload : CardData_Magic{
	public CardData_Reload(){}
	public CardData_Reload(int index,Player pl) : base(index,pl){
		cardExplain = "체력을 "+ reloadAmount + "잃고 내 덱을 처음 상태로 복구합니다.";
	}

    int reloadAmount=1;
	public override void CardActive(){
        player.GetDamage(reloadAmount); 
		PlayerControl.instance.ReLoadDeck ();
	}

	public override CardAbilityType GetCardAbilityType (){
		return CardAbilityType.Util;
	}
	public override string GetCardAbilityValue (){
		return "RE";
	}
}

public class CardData_Bandage : CardData_Magic{
	public CardData_Bandage(){}
	public CardData_Bandage(int index,Player pl) : base(index,pl){
		cardExplain = "자신의 hp를" + healAmount + "만큼 회복합니다.";
		effectType = CardEffectType.Heal;
	}

	int healAmount = 3;

	public override void CardActive (){
        player.GetDamage(-healAmount);
		EffectDelegate.instance.MadeEffect (effectType, PlayerControl.instance.PlayerObject);
    }

	public override CardAbilityType GetCardAbilityType (){
		return CardAbilityType.Heal;
	}
	public override string GetCardAbilityValue (){
		return healAmount.ToString();
	}
}
public class CardData_Portion : CardData_Magic
{
    public CardData_Portion(int index, Player pl) : base(index, pl)
    {
        cardExplain = "모든 hp를 회복합니다.";
        effectType = CardEffectType.Heal;
    }
    public override void CardActive()
    {
        player.GetDamage(-999);
        EffectDelegate.instance.MadeEffect(effectType, PlayerControl.instance.PlayerObject);
    }

    public override CardAbilityType GetCardAbilityType()
    {
        return CardAbilityType.Heal;
    }
    public override string GetCardAbilityValue()
    {
        return "All";
    }
}
public class CardData_Tumble : CardData_Magic
{
    int cardNum = 3;

    public CardData_Tumble() { }
    public CardData_Tumble(int index, Player pl) : base(index, pl)
    {
        cardExplain = "카드 " + cardNum + "장을 드로우 합니다.";
        effectType = CardEffectType.Heal;
    }

    public override void CardActive()
    {
        Routine del = DrawCards;
        CoroutineDelegate.instance.StartRoutine(del);
    }
    IEnumerator DrawCards()
    {
        for (int i = 0; i < cardNum; i++)
        {
            PlayerControl.instance.MagicDraw();
            yield return new WaitForSeconds(0.1f);
        }
    }
    public override CardAbilityType GetCardAbilityType()
    {
        return CardAbilityType.Util;
    }
    public override string GetCardAbilityValue()
    {
        return cardNum.ToString();
    }
}