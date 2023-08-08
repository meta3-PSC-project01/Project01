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
        name = "아스트랄 부적";
        effect = "지속효과 : 적이 떨어뜨리는 부니가 두 배가 됩니다.";
        explanation[0] = "오래되어 닳은 부적.";
        explanation[1] = "행운을 크게 늘려줍니다";
        explanationX = 4;
        itemImage = 2;
        type = ItemType.DURATION;
    }

    public override void Print()
    {
        Debug.LogFormat(name);
        Debug.LogFormat(effect);
        for (int i = 0; i < explanationX; i++)
        {
            Debug.LogFormat(explanation[i]);
        }
    }
}
