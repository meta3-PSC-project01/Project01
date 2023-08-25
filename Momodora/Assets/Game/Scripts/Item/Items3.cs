using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items3 : Items
{
    public Items3()
    {
        Init();
    }

    public override void Init()
    {
        itemName = "�ƽ�Ʈ�� ����";
        effect = "����ȿ�� : ���� ����߸��� �δϰ� �� �谡 �˴ϴ�.";
        explanation[0] = "�����Ǿ� ���� ����.";
        explanation[1] = "����� ũ�� �÷��ݴϴ�";
        explanationX = 4;
        itemImage = 2;
        type = ItemType.DURATION;
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
