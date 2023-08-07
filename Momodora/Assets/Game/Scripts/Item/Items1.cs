using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items1 : Items
{
    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        name = "아이템1";
        title = "등가의 훈장";
        effect = "지속효과 : 천천히 HP 를 회복합니다.";
        explanation[0] = "크로미니아 지방에서 온 훈장.";
        explanation[1] = "사슴을 타고 평생을 먼 지방까지";
        explanation[2] = "돌아다닌 전령들이 달고 다녔습니다.";
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
