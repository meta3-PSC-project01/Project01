using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    //검출용 콜라이더
    private CircleCollider2D circleCollider = default;

    //시야 거리
    //circle 콜라이더의 반지름에 대입
    public float radius = default;

    //0~360도만 입력 가능
    [Range(0f, 360f)]
    //원하는 시야 각도
    public float sightAngleRange = 90f;
    private float sightAngleHalfRange = 0f;

    //정면시야를 기준으로 추가적으로 회전시켜줄 각도를 저장하는 변수
    //벡터up (0,1), y축 양수쪽 라인을 기준삼아 시계방향/반시계방향으로 진행되는 각도
    //즉 0 ~ 360를 -180 ~ 180으로 본 각도.
    [Range(-180f, 180f)]
    public float sightRotateToZ = -90f;

    //디버그 모드용
    public bool onDebug = false;


    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = radius;

        sightAngleHalfRange = sightAngleRange * 0.5f;
    }


    //회전할때 주로 사용.
    public void RotateAngleZ(float angle)
    {
        sightRotateToZ = angle;
    }

    // 입력한 -180~180의 값을 Up Vector 기준 Local Direction으로 변환
    // y축의 양수 방향(0,1) 이던 시작선을 -> y축의 양수방향에서 입력한 sightRotateToZ만큼 회전시켜서 시작선을 그린다.
    private Vector3 GetAngleToUpVector(float sightRotateToZ)
    {
        //angleInDegree : 원하는 각도
        //transform.eulerAngles.z 현재 회전중인 각도

        //기존 회전 인식 -> 오른쪽 회전(시계방향)이 양수로 표현
        //eulerAngles.z -> 오른쪽 회전(시계방향)이 음수로 표현 

        // ex) z축 -10도(기존좌표 10도) + 원하는 각도 10도 -> 20도에서 시작
        float radian = (sightRotateToZ - transform.eulerAngles.z) * Mathf.Deg2Rad;

        //x(밑변),y(높이),r(대각선),t(각도)로 이뤄진 삼각형에서 y=r*sin(t), x=r*cos(t) 이지만
        //해당 함수는 y축을 밑변으로 해서 각도를 그리기 때문에 이를 바꿔서 vector 를 만들었다.
        return new Vector3(Mathf.Sin(radian), Mathf.Cos(radian), 0f);
    }


    //플레이어가 접촉한 동안 계속 추적
    //추적 완료후 해당 오브젝트를 disable 시킨다
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Vector2 myPosition = transform.position;
            Vector2 targetPosition = other.transform.position;

            //내(몬스터) 위치를 기준으로 현재 상대방(플레이어)의 방향
            Vector2 dir = (targetPosition - myPosition).normalized;
            //내(몬스터)가 바라보는 시작선
            Vector2 lookDir = GetAngleToUpVector(this.sightRotateToZ);

            //시작선과 상대방의 방향사이의 각도를 구함
            float angle = Vector3.Angle(lookDir, dir);

            //시작선에서 계산된 각도는 +/- 기준으로 2가지 범위가 될수 있으므로
            //반으로 나눈 값과 비교한다.
            if (angle <= sightAngleHalfRange)
            {
                //타겟 지정 완료
                //한번 타겟이 지정되면 맵을 벗어날때까지 쫓아온다.
                EnemyBase tmp = GetComponentInParent<EnemyBase>();
                tmp.target = other.GetComponent<PlayerMove>();

                //타겟이 지정되면 해당 기능을 쓸 필요가 없기때문에 비활성화 시킨다.
                gameObject.SetActive(false);
                //Debug.Log("인식완료");
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (onDebug)
        {
            sightAngleHalfRange = sightAngleRange * 0.5f;

            Vector3 originPos = transform.position;

            Gizmos.DrawWireSphere(originPos, radius);

            Vector3 horizontalRightDir = GetAngleToUpVector(-sightAngleHalfRange + this.sightRotateToZ);
            Vector3 horizontalLeftDir = GetAngleToUpVector(sightAngleHalfRange + this.sightRotateToZ);
            Vector3 lookDir = GetAngleToUpVector(this.sightRotateToZ);

            Debug.DrawRay(originPos, horizontalLeftDir * radius, Color.cyan);
            Debug.DrawRay(originPos, lookDir * radius, Color.green);
            Debug.DrawRay(originPos, horizontalRightDir * radius, Color.cyan);
        }
    }
}
