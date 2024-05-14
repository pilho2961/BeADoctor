using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery : Items
{
    protected override void Interact()
    {
        // Inventory�� ������ ������

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

                print("���ſϷ�");
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
            interactGuideText.text = "�ݾ��� �����մϴ�.";
        }
        else
        {
            interactGuideText.text = "[G] �����ϱ�";
        }
    }
}
