using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items1 : Items
{
    public Items1()
    {
        Init();
    }

    public override void Init()
    {
        itemName = "��� ����";
        effect = "����ȿ�� : õõ�� HP �� ȸ���մϴ�.";
        explanation[0] = "ũ�ι̴Ͼ� ���濡�� �� ����.";
        explanation[1] = "�罿�� Ÿ�� ����� �� �������";
        explanation[2] = "���ƴٴ� ���ɵ��� �ް� �ٳ���ϴ�.";
        explanationX = 5;
        itemImage = 0;
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
