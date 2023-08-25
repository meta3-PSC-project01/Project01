using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items2 : Items
{
    public Items2()
    {
        Init();
    }

    public override void Init()
    {
        itemName = "���� ����";
        effect = "����ȿ�� : ���� �ð��� �þ�� ������ ����մϴ�.";
        explanation[0] = "Ǫ�� �� ���� ������ �߰��� ����";
        explanation[1] = "������ �����߽��ϴ�. ���� ���� ���� �߼��� ��������";
        explanation[2] = "�� ���� �����ϴ�.";
        explanationX = 5;
        itemImage = 1;
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
