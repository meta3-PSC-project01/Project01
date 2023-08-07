using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items4 : Items
{
    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        title = "초롱꽃";
        effect = "발동효과 : 쓸 때마다 HP 를 소량 회복합니다";
        explanation[0] = "회복력이 있는 꽃이 썩었습니다.";
        explanationX = 1;
    }

    public override void Print()
    {
        Debug.LogFormat(title);
        Debug.LogFormat(effect);
        for (int i = 0; i < explanationX; i++)
        {
            Debug.LogFormat(explanation[i]);
        }
    }
}
