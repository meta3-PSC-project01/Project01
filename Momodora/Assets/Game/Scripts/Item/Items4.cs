using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items4 : Items
{
    public Items4()
    {
        Init();
    }

    public override void Init()
    {
        itemName = "�ʷղ�";
        effect = "�ߵ�ȿ�� : �� ������ HP �� �ҷ� ȸ���մϴ�";
        explanation[0] = "ȸ������ �ִ� ���� ������ϴ�.";
        explanationX = 3;
        itemImage = 3;
        type = ItemType.ACTIVE;
    }

    public override void Print()
    {
        Debug.LogFormat(itemName);
        Debug.LogFormat(effect);
        for (int i = 0; i < explanationX; i++)
        {
            Debug.LogFormat(explanation[i]);
        }
    }
}
