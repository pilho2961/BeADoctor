using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Items
{
    protected override void RandomizingValueOfItem()
    {
        // ����ġ�� �ο��� �������� ����
        int RandomBonus = Random.Range(0, 10000);
        itemData.recoverValue += RandomBonus;
    }
}
