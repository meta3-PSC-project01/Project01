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
        itemName = "초롱꽃";
        effect = "발동효과 : 쓸 때마다 HP 를 소량 회복합니다";
        explanation[0] = "회복력이 있는 꽃이 썩었습니다.";
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
