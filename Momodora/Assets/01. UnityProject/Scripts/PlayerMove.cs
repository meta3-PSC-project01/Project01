using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D playerRigidbody = default;
    private SpriteRenderer playerRenderer = null;
    private Animator animator = default;

    private float moveForce = default;     // 캐릭터가 움직일 힘 수치
    private float rollForce = default;
    private float xInput = default;     // 수평 움직임 입력값
    private float zInput = default;     // 수직 움직임 입력값
    private float jInput = default;
    private float xSpeed = default;     // 수평 움직임 최종값
    private float zSpeed = default;     // 수직 움직임 최종값
    private float jSpeed = default;
    private float rSpeed = default;
    private float jumpForce = default;

    private int jumpCount = default;

    private bool jumping = false;
    private bool flipX = false;
    private bool isRolled = false;
    private bool rollingSlow = false;
    private bool isGrounded = false;
    private bool isCrouched = false;
    private bool isLadder = false;

    private void Awake()
    {
             // { 변수 값 선언
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        moveForce = 8f;
        rollForce = 16f;
        xInput = 0f;
        zInput = 0f;
        jInput = 0f;
        xSpeed = 100f;
        zSpeed = 0f;
        jSpeed = 0f;
        rSpeed = 0f;
        jumpForce = 700f;

        jumpCount = 0;
             // } 변수 값 선언
    }     // End Awake()

    void Update()
    {
        xInput = Input.GetAxis("Horizontal");     // 수평 입력값 대입
        //zInput = Input.GetAxis("Vertical");     // 수직 입력값 대입

        if (isRolled == true && rollingSlow == false)
        {
            if (flipX == false)
            {
                rSpeed += rollForce;     // 수평 입력을 유지한만큼 값이 증가
                Vector3 newVelocity2 = new Vector3(rSpeed, 0f, 0f);
                //transform.Translate(Vector3.right * rSpeed);
                playerRigidbody.velocity = newVelocity2;
            }
            else
            {
                rSpeed += rollForce;     // 수평 입력을 유지한만큼 값이 증가
                Vector3 newVelocity2 = new Vector3(rSpeed, 0f, 0f);
                //transform.Translate(Vector3.right * -rSpeed);
                playerRigidbody.velocity = newVelocity2;
            }
        }
        else
        {
            xSpeed = xInput * moveForce;     // 수평 입력을 유지한만큼 값이 증가
            zSpeed = zInput * moveForce;     // 수직 입력을 유지한만큼 값이 증가
            Vector3 newVelocity = new Vector3(xSpeed, 0f, 0f);     // 수평, 수직 입력값만큼 플레이어 이동 좌표 설정
            /*transform.Translate(Vector3.right * xSpeed);*/     // 확인된 수치의 좌표만큼 플레이어 이동
            //Debug.LogFormat("이동 포스값 : {0}", xSpeed);
            playerRigidbody.velocity = newVelocity;
            Debug.LogFormat("이동 힘값 : {0}", xSpeed);
        }

        if (xSpeed > 0f)
        {
            if (flipX == true)
            {
                flipX = false;
                playerRenderer.flipX = false;
            }
        }

        if (xSpeed < 0f)
        {
            if (flipX == false)
            {
                flipX = true;
                playerRenderer.flipX = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && jumpCount < 2)
        {
            jumpCount += 1;
            jumping = true;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            jumping = false;
            jSpeed = 0f;
        }

        if (jumping == true)
        {
            jSpeed += jumpForce * Time.deltaTime;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jSpeed));

            if (jSpeed >= 30f)
            {
                jSpeed = 30f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && isRolled == false)
        {
            isRolled = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && isCrouched == false)
        {
            isCrouched = true;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) && isCrouched == true)
        {
            isCrouched = false;
        }

        animator.SetBool("Ground", isGrounded);
        animator.SetBool("Roll", isRolled);
        animator.SetBool("Crouch", isCrouched);

        // 사다리에서 나갈때 쓸 변수
        //playerRigidbody.constraints = RigidbodyConstraints2D.None;
        //playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
        
    public void PlayerRollingSlow()
    {
        if (isRolled == true)
        {
            rollingSlow = true;
            playerRigidbody.velocity *= 1f;
            StartCoroutine(SlowEnd());
        }
    }

    public void PlayerRollingEnd()
    {
        if (isRolled == true)
        {
            isRolled = false;
            rSpeed = 0f;
        }
    }

    IEnumerator SlowEnd()
    {
        yield return new WaitForSeconds(0.2f);

        rollingSlow = false;
    }

    //점프에 제한을준다
    //if(playerRigidbody.velocity.y > 500)
    //{
    //    playerRigidbody.velocity = new Vector2(0, 500);
    //}

    //    어느정도가다보면 velocity의 상한을 정해준다.
    //    else if (Input.GetMouseButtonDown(0) && 0 < playerRigid.velocity.y)
    //    {
    //        playerRigid.velocity = playerRigid.velocity * 1f;
    //    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Floor") && 0.7f < collision.contacts[0].normal.y)
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Ladder") && Input.GetKey(KeyCode.UpArrow))
        {
            if (isLadder == false)
            {
                isLadder = true;
                playerRigidbody.velocity = Vector2.zero;
                playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
                playerRigidbody.gravityScale = 0f;
                Debug.Log("사다리를 잡았다");
            }
        }
    }

    
}
