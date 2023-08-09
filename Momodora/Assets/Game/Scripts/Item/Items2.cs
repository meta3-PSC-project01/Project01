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
        name = "세공 반지";
        effect = "지속효과 : 부적 시간이 늘어나고 방어력이 상승합니다.";
        explanation[0] = "푸른 강 깊은 곳에서 발견한 돌을";
        explanation[1] = "손으로 세공했습니다. 새긴 것을 보니 중서부 제국에서";
        explanation[2] = "온 물건 같습니다.";
        explanationX = 5;
        itemImage = 1;
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
