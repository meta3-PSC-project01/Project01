using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//enemy관련 최상단 클래스

public class EnemyBase : MonoBehaviour
{
    //에너미 기본 컴포넌트
    private Rigidbody2D enemyRigidbody;
    private BoxCollider2D enemyCollider;
    private SpriteRenderer enemyRenderer;
    private Animator enemyAnimator;

    //공격관련 컴포넌트
    public Transform attackPosition;    //공격 생성 위치
    public Transform attackPrefab;      //투사체, 공격범위 collider등을 저장

    //탐지 컴포넌트
    //PlayerFinder - 최상단 클래스
    //PlayerFinder
    //public PlayerFinder finderAI;

    //에너미 속성
    public int enemyHp = default;
    public float enemySpeed = default;

    public bool isStun = false;     //경직
    public bool isWait = false;     //탐지못함
    public bool isDelay = false;    //공격 딜레이

    //초기화
    public virtual void Init()
    {
        enemyRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyAnimator = GetComponentInChildren<Animator>();

        enemyCollider = GetComponent<BoxCollider2D>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    //몬스터 공격시 애니메이션 재생
    public void AttackStart()
    {
    }

    //몬스터 공격
    //최하단 자식 클래스에서 구현한다.
    //애니메이션 도중에 실행된다.
    //원거리 -> 오브젝트 생성
    //근거리1 -> 매 애니메이션 마다 충돌판정 value 변경
    //근거리2 -> 적을 향해 돌진 
    public virtual void Attack()
    {
    }

    //몬스터의 콜라이더 이벤트시
    public void Touch(TestPlayer player)
    {
    }

    //해당 몬스터가 플레이어 공격 맞을시(플레이어의 ontrigger이벤트)
    //상대가 호출한다.
    public void Hit() 
    { 
    }

    //몬스터 죽을시
    public void Dead() 
    { 
    }

    //이동 함수
    //최하단 자식 클래스에서 구현한다.
    //ai에서 추적 시에 실행
    public virtual void Move(TestPlayer player)
    {
    }
}
