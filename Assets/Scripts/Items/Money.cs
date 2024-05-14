using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Items
{
    protected override void RandomizingValueOfItem()
    {
        // 가중치를 부여한 랜덤으로 수정
        int RandomBonus = Random.Range(0, 10000);
        itemData.recoverValue += RandomBonus;
    }
}
