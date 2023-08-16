using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public  Rigidbody2D playerRigidbody = default;
    public SpriteRenderer playerRenderer = null;
    public Animator animator = default;
    public Collider2D[] attackCollider;
    public GameObject arrowPrefab;

    public Vector2 attackSize = default;
    public Vector2 attackVector = default;
    public Vector2 arrowVector = default;

    public float moveForce = default;     // 캐릭터가 움직일 힘 수치
    public float rollForce = default;
    public float xInput = default;     // 수평 움직임 입력값
    public float zInput = default;     // 수직 움직임 입력값
    public float xSpeed = default;     // 수평 움직임 최종값
    public float zSpeed = default;     // 수직 움직임 최종값
    public float jSpeed = default;
    public float rSpeed = default;
    public float jumpForce = default;

    public int jumpCount = default;
    public int isMlAttack = default;
    public int playerHp;

    public bool jumping = false;
    public bool jumpingForce = false;
    public bool flipX = false;
    public bool isRolled = false;
    public bool rollingSlow = false;
    public bool isGrounded = false;
    public bool isCrouched = false;
    public bool isLadder = false;
    public bool isAirAttacked = false;
    public bool isBowed = false;
    public bool isAirBowed = false;
    public bool isCrouchBowed = false;
    public bool[] mlAttackConnect = new bool[2];

    public Rigidbody2D platformBody;
    public bool isMovingPlatform = false;

    private void Awake()
    {
        // { 변수 값 선언
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        attackSize = new Vector2(2f, 2f);

        moveForce = 5f;
        rollForce = 0.5f;

        xInput = 0f;
        zInput = 0f;
        xSpeed = 0f;
        zSpeed = 0f;
        jSpeed = 0f;
        rSpeed = 0f;
        jumpForce = 10f;

        jumpCount = 0;
        isMlAttack = 0;
        // } 변수 값 선언
    }     // End Awake()

    void Update()
    {
        xInput = Input.GetAxis("Horizontal");     // 수평 입력값 대입
        zInput = Input.GetAxis("Vertical");     // 수직 입력값 대입

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (isRolled == true && rollingSlow == false)
            {
                if (flipX == false)
                {
                    rSpeed += rollForce;     // 수평 입력을 유지한만큼 값이 증가
                    Vector3 newVelocity2 = new Vector3(rSpeed, 0f, 0f);
                    if (isMovingPlatform)
                    {
                        newVelocity2 *= 1.5f;
                    }
                    playerRigidbody.velocity = newVelocity2;
                }
                else
                {
                    rSpeed += rollForce;     // 수평 입력을 유지한만큼 값이 증가
                    Vector3 newVelocity2 = new Vector3(-rSpeed, 0f, 0f);
                    if (isMovingPlatform)
                    {
                        newVelocity2 *= 1.5f;
                    }
                    playerRigidbody.velocity = newVelocity2;
                }
            }
            else
            {
                if (isCrouched == false && isBowed == false && isCrouchBowed == false)
                {
                    xSpeed = xInput * moveForce;     // 수평 입력을 유지한만큼 값이 증가
                    
                    Vector2 newVelocity = new Vector2(xSpeed, playerRigidbody.velocity.y);     // 수평, 수직 입력값만큼 플레이어 이동 좌표 설정
                    playerRigidbody.velocity = newVelocity;
                    //Debug.LogFormat("이동 힘값 : {0}", xSpeed);
                }
            }
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

        if (Input.GetKeyDown(KeyCode.A) && jumpCount < 2 && isLadder == false && isAirAttacked == false && isBowed == false)
        {
            jSpeed = 7;
            jumpCount += 1;
            jumping = true;
            jumpingForce = true;
        }
        if (Input.GetKey(KeyCode.A) && jumpingForce)
        {
            jSpeed += .3f;
            if (jSpeed > 10f)
            {
                jSpeed = 10;
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            jumpingForce = false;
        }

        if (jumpingForce == true && isAirAttacked == false)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jSpeed);

            if (jSpeed == 10f)
            {
                jumpingForce = false;
            }
            //Debug.LogFormat("점프 힘값 : {0}", jSpeed);

            //    어느정도가다보면 velocity의 상한을 정해준다.
            //    else if (Input.GetMouseButtonDown(0) && 0 < playerRigid.velocity.y)
            //    {
            //        playerRigid.velocity = playerRigid.velocity * 1f;
            //    }
        }

        if (Input.GetKeyDown(KeyCode.Q) && isRolled == false)
        {
            isRolled = true;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isCrouched = true;
            playerRigidbody.velocity = Vector2.zero;
            xSpeed = 0f;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isCrouched = false;

            if (isCrouchBowed == true) { isCrouchBowed = false; }
        }

        if (isLadder == true && Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.A))
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.None;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRigidbody.gravityScale = 40f;
            isLadder = false;
            jumping = true;
            jumpCount += 1;
            xSpeed = -50f;
            flipX = true;
            playerRenderer.flipX = true;

            Debug.Log("사다리를 떠났다");
        }

        if (isLadder == true && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.A))
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.None;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRigidbody.gravityScale = 40f;
            isLadder = false;
            jumping = true;
            jumpCount += 1;
            xSpeed = 50f;
            flipX = false;
            playerRenderer.flipX = false;

            Debug.Log("사다리를 떠났다");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (isGrounded == true)
            {
                if (isMlAttack < 3)
                {
                    if (isMlAttack == 0)
                    {
                        isMlAttack = 1;
                    }
                    else if (isMlAttack == 1)
                    {
                        mlAttackConnect[0] = true;
                    }
                    else if (isMlAttack == 2)
                    {
                        mlAttackConnect[1] = true;
                    }
                }
            }
            else
            {
                if (isAirAttacked == false)
                {
                    isAirAttacked = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.D) && isBowed == false && isAirBowed == false && isCrouchBowed == false)
        {
            if (isCrouched == true)
            {
                isCrouchBowed = true;
            }
            else if (isGrounded == false)
            {
                isAirBowed = true;
            }
            else
            {
                isBowed = true;
                playerRigidbody.velocity = Vector2.zero;
                xSpeed = 0f;
            }
        }

        /*animator.SetBool("Ground", isGrounded);
        animator.SetBool("Roll", isRolled);
        animator.SetBool("Crouch", isCrouched);
        animator.SetBool("AirAttack", isAirAttacked);
        animator.SetBool("Bow", isBowed);
        animator.SetBool("AirBow", isAirBowed);
        animator.SetBool("CrouchBow", isCrouchBowed);
        animator.SetInteger("MlAttack", isMlAttack);
        animator.SetInteger("Run", (int)xSpeed);*/
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

    public void PlayerBowShot()
    {
        if (flipX == false)
        {
            arrowVector = new Vector2(playerRigidbody.position.x + 1.5f, playerRigidbody.position.y);
            GameObject arrow = Instantiate(arrowPrefab, arrowVector, transform.rotation);
        }
        else
        {
            arrowVector = new Vector2(playerRigidbody.position.x - 1.5f, playerRigidbody.position.y);
            GameObject arrow = Instantiate(arrowPrefab, arrowVector, transform.rotation);
        }
    }

    public void PlayerBowEnd()
    {
        isBowed = false;
    }

    public void PlayerAirBowShot()
    {
        if (flipX == false)
        {
            arrowVector = new Vector2(playerRigidbody.position.x + 1.5f, playerRigidbody.position.y);
            GameObject arrow = Instantiate(arrowPrefab, arrowVector, transform.rotation);
        }
        else
        {
            arrowVector = new Vector2(playerRigidbody.position.x - 1.5f, playerRigidbody.position.y);
            GameObject arrow = Instantiate(arrowPrefab, arrowVector, transform.rotation);
        }
    }

    public void PlayerAirBowEnd()
    {
        if (isAirBowed == true) { isAirBowed = false; }
    }

    public void PlayerCrouchBowShot()
    {
        if (flipX == false)
        {
            arrowVector = new Vector2(playerRigidbody.position.x + 1.5f, playerRigidbody.position.y);
            GameObject arrow = Instantiate(arrowPrefab, arrowVector, transform.rotation);
        }
        else
        {
            arrowVector = new Vector2(playerRigidbody.position.x - 1.5f, playerRigidbody.position.y);
            GameObject arrow = Instantiate(arrowPrefab, arrowVector, transform.rotation);
        }
    }

    public void PlayerCrouchBowEnd()
    {
        isCrouchBowed = false;
    }

    public void PlayerMlAttack()
    {
        if (flipX == false)
        {
            Vector2 attackMoveVelocity = new Vector2(+40f, 0f);
            playerRigidbody.velocity = attackMoveVelocity;

            attackVector = new Vector2(playerRigidbody.position.x + 2f, playerRigidbody.position.y);
        }
        else
        {
            Vector2 attackMoveVelocity = new Vector2(-40f, 0f);
            playerRigidbody.velocity = attackMoveVelocity;

            attackVector = new Vector2(playerRigidbody.position.x - 2f, playerRigidbody.position.y);
        }

        attackCollider = Physics2D.OverlapBoxAll(attackVector, attackSize, 0f);

        for (int i = 0; i < attackCollider.Length; i++)
        {
            if (attackCollider[i].tag == ("Enemy"))
            {
                Debug.LogFormat("{0}", attackCollider[i].name);
            }
        }
    }

    public void PlayerAirAttack()
    {
        if (flipX == false)
        {
            attackVector = new Vector2(playerRigidbody.position.x + 2f, playerRigidbody.position.y);
        }
        else
        {
            attackVector = new Vector2(playerRigidbody.position.x - 2f, playerRigidbody.position.y);
        }

        attackCollider = Physics2D.OverlapBoxAll(attackVector, attackSize, 0f);

        for (int i = 0; i < attackCollider.Length; i++)
        {
            if (attackCollider[i].tag == ("Enemy"))
            {
                Debug.LogFormat("{0}", attackCollider[i].name);
            }
        }
    }

    public void PlayerMlAttackEnd()
    {
        if (isMlAttack == 1)
        {
            if (mlAttackConnect[0] == true)
            {
                isMlAttack = 2;
                mlAttackConnect[0] = false;
            }
            else
            {
                isMlAttack = 0;
            }
        }
        else if (isMlAttack == 2)
        {
            if (mlAttackConnect[1] == true)
            {
                isMlAttack = 3;
                mlAttackConnect[1] = false;
            }
            else
            {
                isMlAttack = 0;
            }
        }
        else if (isMlAttack == 3)
        {
            isMlAttack = 0;
        }
    }

    IEnumerator SlowEnd()
    {
        yield return new WaitForSeconds(0.2f);
        rollingSlow = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Floor") && 0.7f < collision.contacts[0].normal.y)
        {
            isGrounded = true;

            if (jumping == true)
            {
                jumping = false;
                jumpCount = 0;
                jSpeed = 0f;
            }

            if (isAirBowed == true) { isAirBowed = false; }

            if (isAirAttacked == true) { isAirAttacked = false; }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor")
        {
            isGrounded = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Ladder") && Input.GetKey(KeyCode.UpArrow))
        {
            if (isLadder == false)
            {
                isLadder = true;
                jumpCount = 0;
                playerRigidbody.velocity = Vector2.zero;
                playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
                playerRigidbody.gravityScale = 0f;
                Debug.Log("사다리를 잡았다");
            }
        }
    }
}