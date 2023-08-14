using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTile : MonoBehaviour
{
    List<EventTile> eventTiles;

    //이벤트 매니저에 추가될 bool 변수
    //이벤트 종료
    bool endEvent = false;
    
    //이벤트 실행
    bool playEvent = false;

    //화살로 타격
    public bool enableArrow = true;
    //발판
    public bool enablefloor = true;
    //접근
    public bool enableAccess = false;
    //아이템
    public string accessItemName = null;

    private void Update()
    {
        if (playEvent && !endEvent)
        {
            foreach(var eventTile in eventTiles)
            {
                eventTile.PlayEvent();
                endEvent = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enableArrow && collision.collider.tag == "Arrow")
        {
            if (!playEvent)
            {
                playEvent = true;
            }
        }

        if(enablefloor && collision.collider.tag == "Player")
        {
            foreach (var contact in collision.contacts)
            {
                if(contact.point.normalized.y >= .8f)
                {
                    playEvent = true;
                }
            }
        }

        if (enableAccess && collision.collider.tag == "Player")
        {
            if (accessItemName != null)
            {
                playEvent = true;
                /*추가로 item이름 비교 필요*/
            }
        }
    }
}
