using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D playerRigidbody = default;

    private float moveForce = default;     // 캐릭터가 움직일 힘 수치
    private float xInput = default;     // 수평 움직임 입력값
    private float zInput = default;     // 수직 움직임 입력값
    private float xSpeed = default;     // 수평 움직임 최종값
    private float zSpeed = default;     // 수직 움직임 최종값
    private float jInput = default;
    private float jumpForce = default;
    private int jumpCount = default;

    private void Awake()
    {
             // { 변수 값 선언
        playerRigidbody = GetComponent<Rigidbody2D>();

        moveForce = 8f;
        xInput = 0f;
        zInput = 0f;
        xSpeed = 0f;
        zSpeed = 0f;
        jInput = 0f;
        jumpForce = 200f;
        jumpCount = 0;
             // } 변수 값 선언
    }     // End Awake()

    void Update()
    {
        jInput = Input.GetAxis("Jump");
        xInput = Input.GetAxis("Horizontal");     // 수평 입력값 대입
        zInput = Input.GetAxis("Vertical");     // 수직 입력값 대입
        xSpeed = xInput * moveForce * Time.deltaTime;     // 수평 입력을 유지한만큼 값이 증가
        zSpeed = zInput * moveForce * Time.deltaTime;     // 수직 입력을 유지한만큼 값이 증가
        if (zSpeed < 0f)     // 아래 방향 입력값 확인
        {
            Debug.Log("앉은 상태");
        }
        Vector3 newvelocity = new Vector3(xSpeed, 0f, zSpeed);     // 수평, 수직 입력값만큼 플레이어 이동 좌표 설정
        transform.Translate(Vector3.right * xSpeed);     // 확인된 수치의 좌표만큼 플레이어 이동
        if (jInput > 0f)
        {
            jInput = 0f;
            PlayerJump();
        }
    }     // End Update()

    private void PlayerJump()
    {
        if (jumpCount >= 2) { return; }

        jumpCount += 1;
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.AddForce(new Vector2(0, jumpForce));
        //else if (Input.GetMouseButtonDown(0) && 0 < playerRigid.velocity.y)
        //{
        //    playerRigid.velocity = playerRigid.velocity * 1f;
        //}
    }     // End PlayerJump()

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (0.7f < collision.contacts[0].normal.y)
        {
            //isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //isGrounded = false;
    }
}
