﻿using Arch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Card_Special : Card
{
    public Card_Special(CardData cardData)
    {
        index = cardData.index;
        name = cardData.name;
        cost = cardData.cost;
        cardType = CardType.S;
        val1 = cardData.val1;
        val2 = cardData.val2;
        val3 = cardData.val3;
        info = cardData._info;
        spritePath = cardData.spritePath;
    }
    public override void OnCardReturned()
    {
        base.OnCardReturned();
        player.GetDamage(1);
    }
    public override void CardReturnCallBack(Card data)
    {
        base.CardReturnCallBack(data);
        if(PlayerControl.instance.hand.isOnHand(this))
        {
            cost -= 1;
        }
    }
    public override void UpgradeReset()
    {
        isUpgraded = false;
        val1 = Database.GetCardData(index).val1;
    }
    public virtual void CostReset()
    {
        cost = Database.GetCardData(index).cost;
    }
}

public class Card_Reload : Card_Special
{
    public Card_Reload(CardData cd) : base(cd)
    {

    }

    public override void CardReturnCallBack(Card data)
    {
    }
    protected override void CardActive()
    {
        PlayerControl.player.GetDamage(1);
        PlayerControl.instance.ReLoadDeck();
    }
}

//2마리 이상 적을 타격 시 체력 2 회복
public class Card_RedGrasp : Card_Special
{

    public Card_RedGrasp(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 1, Figure.SQUARE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));
        }
    }

    protected override void CardActive()
    {
        if(TileUtils.IsEnemyInRange(player.currentTile,1,Figure.SQUARE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 1, Figure.SQUARE);
            if(enemies.Count>=val2)
            {
                player.GetHeal(val3);
            }
            for (int i = 0; i < enemies.Count; i++)              
                {
                    DamageToTarget(enemies[i], val1);
                }       
        }
    }
}


//2마리 이상 적을 타격 시 AKS 5 회복
public class Card_BlueGrasp : Card_Special
{
    public Card_BlueGrasp(CardData cd) :base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 1, Figure.SQUARE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));
        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 1, Figure.SQUARE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 1, Figure.SQUARE);
            if (enemies.Count >= val2)
            {
                PlayerData.AkashaGage += val3;
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
    }
}

//기존 패 버리고 같은 장수 다시 드로우
public class Card_TimeFrog : Card_Special
{
    public Card_TimeFrog(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 1, Figure.SQUARE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 1, Figure.SQUARE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 1, Figure.SQUARE);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        PlayerData.AkashaGage += val3;
        int hand = PlayerControl.instance.hand.CurrentHandCount;
        for(int i=0; i<hand;i++)
        {
            PlayerControl.instance.hand.ReturnCard();
        }
        for (int i = 0; i<hand; i++)
        {
            PlayerControl.instance.NaturalDraw();
        }
    }
}

//1턴 피해면역
public class Card_CrimsonCrow : Card_Special
{
    public Card_CrimsonCrow(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.CROSS);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.CROSS))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.CROSS);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        PlayerData.AkashaGage += val3;
        PlayerControl.playerBuff.UpdateBuff(BUFF.IMMUNE,val2);
    }
}

//덱에 남은 카드가 5장 이하일 시 패널티 삭제
public class Card_Bishop : Card_Special
{
    public Card_Bishop(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.Diagonal);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.Diagonal))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.Diagonal);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }

        PlayerControl.playerBuff.UpdateBuff(BUFF.AKASHA, val3);
        if (PlayerControl.instance.deck.DeckCount<=val2)
        {
            PlayerControl.playerBuff.EraseDeBuff();
        }
    }
}

public class Card_WolfBite : Card_Special
{
    public Card_WolfBite(CardData cd) : base(cd)
    {

    }
    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 2)));

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y - 2)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = GetRange();

        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(GetRange()))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(GetRange());
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        player.GetDamage(val3);
    }
}

public class Card_BearClaw : Card_Special
{
    public Card_BearClaw(CardData cd) : base(cd)
    {

    }

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y - 2)));

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y - 2)));


        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = GetRange();

        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(GetRange()))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(GetRange());
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        player.GetDamage(val3);
    }
}

public class Card_Justice : Card_Special
{
    public Card_Justice(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.EMPTYSQUARE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.EMPTYSQUARE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.EMPTYSQUARE);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        player.GetHeal(val3);
    }
}

public class Card_WindCat : Card_Special
{
    public Card_WindCat(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = GetRange();
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {
        List<Tile> targetTiles = GetRange();

        if (TileUtils.IsEnemyInRange(targetTiles))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(targetTiles);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        MoveToRandom();
        PlayerControl.playerBuff.UpdateBuff(BUFF.MOVE,val3);
    }

    private void MoveToRandom()
    {
        List<Tile> UnExplore = new List<Tile>(player.currentRoom.GetTileToList());

        while(UnExplore.Count>1)
        {
            int rand = UnityEngine.Random.Range(0, UnExplore.Count);

            if (UnExplore[rand].IsStandAble(player))
            {
                player.MoveTo(UnExplore[rand].pos);
                return;
            }
            else
            {
                UnExplore.RemoveAt(rand);
            }
        }
        Debug.Log("랜덤이동 실패");
    }

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x +1, y +2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x+1, y+1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y+1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x-1, y+1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x-2, y+1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x-1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x+1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x-1, y-1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y-1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x+1, y-1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x+2, y-1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x-1, y-2)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
}

public class Card_HalfMask : Card_Special
{
    public Card_HalfMask(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = GetRange();
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {
        List<Tile> targetTiles = GetRange();

        if (TileUtils.IsEnemyInRange(targetTiles))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(targetTiles);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }else
        {
            player.GetHeal(val3);
        }

    }

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x -2, y- 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y + 2)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
}
public class Card_BlackThunder : Card_Special
{

    public Card_BlackThunder(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = GetRange();
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD,player, targetTiles[i]));
        }
    }

    protected override void CardActive()
    {
        List<Tile> targetTiles = GetRange();

        if (TileUtils.IsEnemyInRange(targetTiles))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(targetTiles);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }

    }

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y - 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x +1, y-1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x +1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y +1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y + 1)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y - 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y + 2)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y - 2)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    public override void CardReturnCallBack(Card data)
    {
        base.CardReturnCallBack(data);
        UpgradeThis();
    }
}
public class Card_PoisonSnail : Card_Special
{
    public Card_PoisonSnail(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.SQUARE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.SQUARE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.SQUARE);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        
        player.SetHp(val2);
        PlayerControl.playerBuff.UpdateBuff(BUFF.CARD,val3);
        PlayerControl.playerBuff.UpdateBuff(BUFF.AKASHA,val3);
    }
}
public class Card_Shield : Card_Special
{
    public Card_Shield(CardData cd) : base(cd)
    {
        isDirectionCard = true;
    }

    public override void CardEffectPreview()
    {
        if (PlayerControl.instance.SelectedDirCard == this)
        {
            List<Tile> targetTiles = TileUtils.CrossRange(player.currentTile, 1);
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.DIR, player, targetTiles[i]));
            }
        }
        else
        {
            List<Tile> targetTiles = GetRange();
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));
            }
            targetTiles = GetRange2();
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.DIR, player, targetTiles[i]));
            }
        }

    }

    protected override void CardActive(Direction dir)
    {
        List<Tile> targetTiles = GetRange3(dir);

        if (TileUtils.IsEnemyInRange(targetTiles))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(targetTiles);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }

        Vector2Int d = Vector2Int.zero;
        switch (dir)
        {
            case Direction.NORTH:
                d = Vector2Int.up;
                break;
            case Direction.EAST:
                d = Vector2Int.right;
                break;
            case Direction.SOUTH:
                d = Vector2Int.down;
                break;
            case Direction.WEST:
                d = Vector2Int.left;
                break;
        }
        player.MoveTo(player.pos + d * -1);

    }

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    private List<Tile> GetRange2()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x -1, y)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    private List<Tile> GetRange3(Direction dir)
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        switch (dir)
        {
            case Direction.EAST:
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y)));
                break;
            case Direction.NORTH:
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y + 1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y + 2)));
                break;
            case Direction.SOUTH:
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y - 1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y - 2)));
                break;
            case Direction.WEST:
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y)));
                break;
        }


        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
}

public class Card_Rush : Card_Special
{
    public Card_Rush(CardData cd) : base(cd)
    {
        isDirectionCard = true;
    }

    public override void CardEffectPreview()
    {
        if(PlayerControl.instance.SelectedDirCard == this)
        {
            List<Tile> targetTiles = TileUtils.CrossRange(player.currentTile,2);
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));
            }

            targetTiles = GetRange4();
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.DIR, player, targetTiles[i]));
            }
        }
        else
        {
            List<Tile> targetTiles = GetRange();
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));
            }
            targetTiles = GetRange2();
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.DIR, player, targetTiles[i]));
            }
        }

    }

    protected override void CardActive(Direction dir)
    {
        List<Tile> targetTiles = GetRange3(dir);

        if (TileUtils.IsEnemyInRange(targetTiles))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(targetTiles);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }

        Vector2Int d = Vector2Int.zero;
        switch (dir)
        {
            case Direction.NORTH:
                d = Vector2Int.up;
                break;
            case Direction.EAST:
                d = Vector2Int.right;
                break;
            case Direction.SOUTH:
                d = Vector2Int.down;
                break;
            case Direction.WEST:
                d = Vector2Int.left;
                break;
        }
        player.MoveTo(player.pos + d * 3);
    
}

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    private List<Tile> GetRange2()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 3, y)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    private List<Tile> GetRange3(Direction dir)
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        switch(dir)
        {
            case Direction.EAST:
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 1, y)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 2, y)));
                break;
            case Direction.NORTH:
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y+1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y+2)));
                break;
            case Direction.SOUTH:
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y-1)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x, y-2)));
                break;
            case Direction.WEST:
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 1, y)));
                targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 2, y)));
                break;
        }


        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
    private List<Tile> GetRange4()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 3, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y+3)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y-3)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }
}
public class Card_Mist : Card_Special
{
    public Card_Mist(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.CIRCLE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {


        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.CIRCLE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.CIRCLE);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        foreach (Card c in PlayerData.Deck)
        {
            if (c.Type == CardType.A)
            {
                for (int i = 0; i < val2; i++)
                {
                    c.UpgradeThis();
                }
            }
        }
    }
}
public class Card_WormHole : Card_Special
{
    public Card_WormHole(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.CIRCLE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {


        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.CIRCLE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.CIRCLE);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        foreach (Card c in PlayerData.Deck)
        {
            if (c.Type == CardType.P)
            {
                for (int i = 0; i < val2; i++)
                {
                    c.UpgradeThis();
                }
            }
        }
        player.GetHeal(val3);
    }
}

public class Card_Plant : Card_Special
{
    public Card_Plant(CardData cd) : base(cd)
    {
        isDirectionCard = true;
    }

    public override void CardEffectPreview()
    {
        if (PlayerControl.instance.SelectedDirCard == this)
        {
            List<Tile> targetTiles = GetRange();
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.DIR, player, targetTiles[i]));

            }
        }
        else
        {
            List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.CIRCLE);
            for (int i = 0; i < targetTiles.Count; i++)
            {
                ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

            }
        }
    }

    protected override void CardActive()
    {


        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.CIRCLE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.CIRCLE);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        foreach (Card c in PlayerData.Deck)
        {
            if (c.Type == CardType.V)
            {
                for (int i = 0; i < val2; i++)
                {
                    c.UpgradeThis();
                }
            }
        }
    }

    protected override void CardActive(Direction dir)//CardType : T
    {
        Vector2Int d = Vector2Int.zero;
        switch (dir)
        {
            case Direction.NORTH:
                d = Vector2Int.up;
                break;
            case Direction.EAST:
                d = Vector2Int.right;
                break;
            case Direction.SOUTH:
                d = Vector2Int.down;
                break;
            case Direction.WEST:
                d = Vector2Int.left;
                break;
        }

        player.MoveTo(player.pos + d * val3);
    }

    private List<Tile> GetRange()
    {
        List<Tile> targetTiles = new List<Tile>();
        int x = (int)player.pos.x; int y = (int)player.pos.y;

        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x + 3, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x - 3, y)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y+3)));
        targetTiles.Add(GameManager.instance.CurrentRoom().GetTile(new Vector2Int(x , y-3)));

        for (int i = targetTiles.Count - 1; i >= 0; i--)
        {
            if (targetTiles[i] == null)
            {
                targetTiles.RemoveAt(i);
            }
        }
        return targetTiles;
    }

}

public class Card_Stamp : Card_Special
{
    public Card_Stamp(CardData cd) : base(cd)
    {

    }

    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.CIRCLE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.CIRCLE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.CIRCLE);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        foreach (Card c in PlayerData.Deck)
        {
            if (c.Type == CardType.S)
            {
                for (int i = 0; i < val2; i++)
                {
                    c.UpgradeThis();
                }
            }
        }
        PlayerData.AkashaGage += val3;
    }
}
public class Card_Needle : Card_Special
{
    public Card_Needle(CardData cd) : base(cd)
    {

    }
    
    public override void CardEffectPreview()
    {
        List<Tile> targetTiles = TileUtils.Range(player.currentTile, 2, Figure.CIRCLE);
        for (int i = 0; i < targetTiles.Count; i++)
        {
            ranges.Add(EffectDelegate.instance.MadeEffect(RangeEffectType.CARD, player, targetTiles[i]));

        }
    }

    protected override void CardActive()
    {
        if (TileUtils.IsEnemyInRange(player.currentTile, 2, Figure.CIRCLE))
        {
            List<Enemy> enemies = TileUtils.GetEnemies(player.currentTile, 2, Figure.CIRCLE);
            for (int i = 0; i < enemies.Count; i++)
            {
                DamageToTarget(enemies[i], val1);
            }
        }
        foreach (Card c in PlayerData.Deck)
        {
            for (int i = 0; i < val2; i++)
            {
                c.UpgradeThis();
            }
        }
        PlayerData.AkashaGage += val3;
        player.GetHeal(val3);
    }
}