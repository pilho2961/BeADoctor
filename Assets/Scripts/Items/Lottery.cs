using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery : Items
{
    protected override void Interact()
    {
        // Inventory로 정보를 보내줌

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!player.interacting)
            {
                if (PlayerStatManager.GetInstance.StatValues["Money"] < Mathf.Abs(itemData.recoverValue))
                {
                    return;
                }

                PlayerStatManager.GetInstance.ResulfOfPlayerAction(itemData.recoverType.ToString(), itemData.recoverValue);
                PlayerStatUI.instance.UpdateGauge();

                print("구매완료");
                inventory.AddItem(itemData);
                //LeanPool.Despawn(gameObject);
                PopdownInteraction();
            }
        }
    }

    protected override void PopupInteraction()
    {
        base.PopupInteraction();

        if (PlayerStatManager.GetInstance.StatValues["Money"] < Mathf.Abs(itemData.recoverValue))
        {
            interactGuideText.text = "금액이 부족합니다.";
        }
        else
        {
            interactGuideText.text = "[G] 구매하기";
        }
    }
}
