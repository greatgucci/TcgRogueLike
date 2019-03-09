﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Damage : Enemy
{
    bool isFailed=false;

    protected override void SetActionLists()
    {
        DelayList = null;
        act = new List<Action>() { new Action(Delay), new Action(Delay), new Action(Delay), new Action(Delay), new Action(Delay), new Action(Delay), new Action(Destroy) };
    }

    List<Action> act;
    protected override void Think()
    {
        currentActionList = act;
    }

    IEnumerator Delay()
    {
        yield return null;
    }
    IEnumerator Destroy()
    {
        isFailed = true;
        currentRoom.RoomName = "fail_damage";
        DestroyThis();
        yield return null;
    }
    protected override void OnDieCallback()
    {
        if (!isFailed)
        {
            UIManager.instance.StartUIAnim(UIAnimation.Attain);
        }

        base.OnDieCallback();
    }
}
