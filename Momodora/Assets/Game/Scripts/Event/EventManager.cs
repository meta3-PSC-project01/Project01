using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager 
{
    public Dictionary<string, MapEvent> eventCheck = new Dictionary<string, MapEvent>();
    public int eventCheck2 = default;

    public int test = default;

    /* 이벤트 체크 목록
    0 : Stage1Map3 - 보물상자
    1 : Stage1Map4 - 초롱꽃
    2 : Stage1Map7 - 솟아오르는 돌다리
    3 : Stage1Map11 - 보물상자
    4 : Stage1Map12 - 돌 엘레베이터
    5 : Stage1Map15 - 아이템 획득
    6 : Stage1Map16 - 돌 문
    7 : Stage1Map23 - 보물상자
    8 : 보스 맵 - 보스 상태
    9 : 해골 NPC 대화 상태

    */

    void Awake()
    {
        //for (int i = 0; i < 10; i++)
        //{
        //    eventCheck[i] = 0;
        //    eventCheck2 = 0;
        //    eventCheck[1] = 1;

        //    test = 5;
        //}
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    test=MapCheck("Stage1Map4");
        //    Debug.Log(test);
        //}
    }

    //public int MapCheck(string mapName)
    //{
        //int check=2;

        //if (mapName == "Stage1Map3")
        //{
        //    if (eventCheck[0] == 0)
        //    {
        //        // 보물상자 초기상태
        //    }
        //    else
        //    {
        //        // 보물상자 열린상태
        //    }

        //    check = eventCheck[0];
        //}
        //else if (mapName == "Stage1Map4")
        //{
        //    if (eventCheck[1] == 0)
        //    {
        //        // 초롱꽃 초기상태
        //    }
        //    else
        //    {
        //        // 초롱꽃 획득 상태
        //    }

        //    check = eventCheck[1];
        //}
        //else if (mapName == "Stage1Map7")
        //{
        //    if (eventCheck[2] == 0)
        //    {
        //        // 솟아오르는 돌다리 초기 상태
        //    }
        //    else
        //    {
        //        // 솟아오르는 돌다리 동작된 상태
        //    }

        //    check = eventCheck[2];
        //}
        //else if (mapName == "Stage1Map11")
        //{
        //    if (eventCheck[3] == 0)
        //    {
        //        // 보물상자 초기상태
        //    }
        //    else
        //    {
        //        // 보물상자 열린상태
        //    }

        //    check = eventCheck[3];
        //}
        //else if (mapName == "Stage1Map12")
        //{
        //    if (eventCheck[4] == 0)
        //    {
        //        // 엘레베이터 초기상태
        //    }
        //    else
        //    {
        //        // 엘레베이터 내려간 상태
        //    }

        //    check = eventCheck[4];
        //}
        //else if (mapName == "Stage1Map15")
        //{
        //    if (eventCheck[5] == 0)
        //    {
        //        // 아이템 미획득 상태
        //    }
        //    else
        //    {
        //        // 아이템 획득 상태
        //    }

        //    check = eventCheck[5];
        //}
        //else if (mapName == "Stage1Map16")
        //{
        //    if (eventCheck[6] == 0)
        //    {
        //        // 돌 문 초기 상태
        //    }
        //    else
        //    {
        //        // 돌 문 열린 상태
        //    }

        //    check = eventCheck[6];
        //}
        //else if (mapName == "Stage1Map23")
        //{
        //    if (eventCheck[7] == 0)
        //    {
        //        // 보물상자 초기상태
        //    }
        //    else
        //    {
        //        // 보물상자 열린상태
        //    }

        //    check = eventCheck[7];
        //}
        //else if (mapName == " ")   // 보스 맵
        //{
        //    if (eventCheck[8] == 0)
        //    {
        //        // 보스 초기상태
        //    }
        //    else
        //    {
        //        // 보스 죽은상태
        //    }

        //    check = eventCheck[8];
        //}
        //else if (mapName == " ")   // 해골 NPC 맵
        //{
        //    if (eventCheck[9] == 0)
        //    {
        //        // 해골 NPC 초기 상태
        //    }
        //    else
        //    {
        //        // 해골 NPC 대화 종료 상태
        //    }

        //    check = eventCheck[9];
        //}
        //else
        //{
        //    check = 2;
        //}

        //return check;
    //}
}
