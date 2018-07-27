﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Debuffs
{
    protected int turn;
    protected GameObject effect;
    public virtual void CountTurn()
    {
        turn--;
        if(turn <= 0)
        {
            PlayerControl.instance.EraseDebuff();
        }
    }

    public abstract void Active();
    public abstract void OnDestroy();

}
public class Debuff_Move : Debuffs
{
    public override void Active()
    {
        turn = 3;
        PlayerControl.instance.isMoveAble = false;
        effect = EffectDelegate.instance.MadeEffect(StatusEffectType.Spider, PlayerControl.Player);
    }
    public override void OnDestroy()
    {
        PlayerControl.instance.isMoveAble = true;
        EffectDelegate.instance.DestroyEffect(effect);
    }
}
public class Debuff_Draw : Debuffs
{
    public override void Active()
    {
        PlayerControl.instance.isDrawAble = false;
        turn = 3;
        //TODO : MAKE EFFECT
    }
    public override void OnDestroy()
    {
        PlayerControl.instance.isDrawAble = true;
        //TODO : ERASE EFFECT
    }
}