using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D playerRigidbody = default;
    private SpriteRenderer playerRenderer = null;
    private Animator animator = default;
    private Collider2D[] attackCollider;

    public GameObject arrowPrefab;

    private Vector2 attackSize = default;
    private Vector2 attackVector = default;
    private Vector2 arrowVector = default;

    private float moveForce = default;     // 캐릭터가 움직일 힘 수치
    private float rollForce = default;
    private float xInput = default;     // 수평 움직임 입력값
    private float zInput = default;     // 수직 움직임 입력값
    private float xSpeed = default;     // 수평 움직임 최종값
    private float zSpeed = default;     // 수직 움직임 최종값
    private float rSpeed = default;
    private float jumpForce = default;
    private float[] jSpeed = new float[2];

    private int jumpCount = default;
    private int isMlAttack = default;

    private bool jumping = false;
    private bool jumpingForce = false;
    public bool flipX = false;
    private bool rollFlipX = false;
    private bool isRolled = false;
    private bool rollingSlow = false;
    private bool isGrounded = false;
    private bool isCrouched = false;
    private bool isLadder = false;
    private bool isAirAttacked = false;
    private bool isBowed = false;
    private bool isAirBowed = false;
    private bool isCrouchBowed = false;
    private bool[] mlAttackConnect = new bool[2];
    public bool lookAtInventory = false;

    private int test = 0;

    void Awake()
    {
             // { 변수 값 선언
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        attackSize = new Vector2(2f, 2f);

        moveForce = 10f;
        rollForce = 9f;
        xInput = 0f;
        zInput = 0f;
        xSpeed = 0f;
        zSpeed = 0f;
        rSpeed = 0f;
        jSpeed[0] = 0f;
        jSpeed[1] = 0f;
        jumpForce = 2f;

        jumpCount = 0;
        isMlAttack = 0;
             // } 변수 값 선언
    }     // End Awake()

    void Update()
    {
        if (ItemManager.instance.lookAtInventory == true) { return; }

        xInput = Input.GetAxis("Horizontal");     // 수평 입력값 대입
        zInput = Input.GetAxis("Vertical");     // 수직 입력값 대입

        if (isRolled == true)
        {
            if (rollingSlow == false)
            {
                if (rollFlipX == false)
                {
                    //rSpeed += rollForce;     // 수평 입력을 유지한만큼 값이 증가
                    playerRigidbody.AddForce(new Vector2(rollForce, 0f));
                    test += 1;
                }
                else
                {
                    //rSpeed += rollForce;     // 수평 입력을 유지한만큼 값이 증가
                    playerRigidbody.AddForce(new Vector2(-rollForce, 0f));
                    test += 1;
                }
            }
        }
        else
        {
            if (isCrouched == false && isBowed == false && isCrouchBowed == false && isMlAttack == 0)
            {
                xSpeed = xInput * moveForce;     // 수평 입력을 유지한만큼 값이 증가
                Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);     // 수평, 수직 입력값만큼 플레이어 이동 좌표 설정
                playerRigidbody.velocity = newVelocity;
                //Debug.LogFormat("이동 힘값 : {0}", xSpeed);
            }
        }

        if (isRolled == false)
        {
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
        }

        if (Input.GetKeyDown(KeyCode.A) && jumpCount < 2 && isLadder == false && isAirAttacked == false && isBowed == false)
        {
            jSpeed[0] = 5;
            jSpeed[1] = 5;
            jumpCount += 1;
            jumping = true;
            jumpingForce = true;
        }

        if (Input.GetKey(KeyCode.A) && jumping == true && jumpingForce == true && isLadder == false && isAirAttacked == false && isBowed == false)
        {
            if (jumpCount == 1)
            {
                jSpeed[0] += jumpForce;
                Vector2 newVelocity = new Vector2(0f, jSpeed[0]);
                playerRigidbody.velocity = newVelocity;

                if (jSpeed[0] >= 100f)
                {
                    jSpeed[0] = 100f;
                    jumpingForce = false;
                }
            }
            else if (jumpCount == 2)
            {
                jSpeed[1] += jumpForce;
                Vector2 newVelocity = new Vector2(0f, jSpeed[0]);
                playerRigidbody.velocity = newVelocity;

                if (jSpeed[1] >= 100f)
                {
                    jSpeed[1] = 100f;
                    jumpingForce = false;
                }
            }
            
            //Debug.LogFormat("점프 힘값 : {0}", jSpeed);

            //    어느정도가다보면 velocity의 상한을 정해준다.
            //    else if (Input.GetMouseButtonDown(0) && 0 < playerRigid.velocity.y)
            //    {
            //        playerRigid.velocity = playerRigid.velocity * 1f;
            //    }
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            jumpingForce = false;
        }

        if (Input.GetKeyDown(KeyCode.Q) && isRolled == false && rollingSlow == false && isGrounded == true)
        {
            playerRigidbody.velocity = Vector2.zero;
            rollFlipX = flipX;
            xSpeed = 0f;
            xInput = 0f;
            isRolled = true;
            test = 0;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            playerRigidbody.velocity = Vector2.zero;
            xSpeed = 0f;
            xInput = 0f;
            isCrouched = true;
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
                    playerRigidbody.velocity = Vector2.zero;
                    xSpeed = 0f;
                    xInput = 0f;

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

        if (Input.GetKeyDown(KeyCode.T) && ItemManager.instance.lookAtInventory == false)
        {
            ItemManager.instance.lookAtInventory = true;
            ItemManager.instance.GetComponent<Inventory>().enabled = true;
            ItemManager.instance.inventoryUi.SetActive(true);
            Time.timeScale = 0f;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ItemManager.instance.GetComponent<Inventory>().GetItem("등가의 훈장");
            ItemManager.instance.GetComponent<Inventory>().GetItem("세공 반지");
            ItemManager.instance.GetComponent<Inventory>().GetItem("아스트랄 부적");
            ItemManager.instance.GetComponent<Inventory>().GetItem("초롱꽃");
            Debug.Log("아이템 획득!");
        }

        animator.SetBool("Ground", isGrounded);
        animator.SetBool("Roll", isRolled);
        animator.SetBool("Crouch", isCrouched);
        animator.SetBool("AirAttack", isAirAttacked);
        animator.SetBool("Bow", isBowed);
        animator.SetBool("AirBow", isAirBowed);
        animator.SetBool("CrouchBow", isCrouchBowed);
        animator.SetInteger("MlAttack", isMlAttack);
        animator.SetInteger("Run", (int)xSpeed);
    }

    public bool IsJump()
    {
        return (isGrounded == false && jumpCount < 2 && isLadder == false && isAirAttacked == false && isBowed == false);
    }

    //public bool isAttack()
    //{
    //    return;
    //}
   
    public void PlayerRollingSlow()
    {
        rollingSlow = true;
        //rSpeed = 0f;
        //playerRigidbody.velocity *= 0.5f;
        Debug.Log(test);
    }

    public void PlayerRollingEnd()
    {
        //rSpeed = 0f;
        isRolled = false;
        rollingSlow = false;
        Debug.Log(test);
    }

    IEnumerator RollStartCheck()
    {
        yield return new WaitForSeconds(0.2f);
        ItemManager.instance.lookAtInventory = false;
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
            arrow.GetComponent<ArrowMove>().arrowRenderer.flipX = true;
            arrow.GetComponent<ArrowMove>().flipX = true;
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
            arrow.GetComponent<ArrowMove>().arrowRenderer.flipX = true;
            arrow.GetComponent<ArrowMove>().flipX = true;
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
            arrow.GetComponent<ArrowMove>().arrowRenderer.flipX = true;
            arrow.GetComponent<ArrowMove>().flipX = true;
        }
    }

    public void PlayerCrouchBowEnd()
    {
        isCrouchBowed = false;
    }

    public void PlayerMlAttack()
    {
        if (isMlAttack == 3)
        {
            if (flipX == false)
            {
                Vector2 attackMoveVelocity = new Vector2(+20f, 0f);
                playerRigidbody.velocity = attackMoveVelocity;

                attackVector = new Vector2(playerRigidbody.position.x + 2f, playerRigidbody.position.y);
            }
            else
            {
                Vector2 attackMoveVelocity = new Vector2(-20f, 0f);
                playerRigidbody.velocity = attackMoveVelocity;

                attackVector = new Vector2(playerRigidbody.position.x - 2f, playerRigidbody.position.y);
            }
        }
        else
        {
            if (flipX == false)
            {
                Vector2 attackMoveVelocity = new Vector2(+15f, 0f);
                playerRigidbody.velocity = attackMoveVelocity;

                attackVector = new Vector2(playerRigidbody.position.x + 2f, playerRigidbody.position.y);
            }
            else
            {
                Vector2 attackMoveVelocity = new Vector2(-15f, 0f);
                playerRigidbody.velocity = attackMoveVelocity;

                attackVector = new Vector2(playerRigidbody.position.x - 2f, playerRigidbody.position.y);
            }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Floor") && 0.7f < collision.contacts[0].normal.y)
        {
            isGrounded = true;
            
            if (jumping == true)
            {
                jumpingForce = false;
                jumping = false;
                jumpCount = 0;
                jSpeed[0] = 0f;
                jSpeed[1] = 0f;
            }

            if (isAirBowed == true) { isAirBowed = false; }

            if (isAirAttacked == true) { isAirAttacked = false; }
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
                jumpCount = 0;
                playerRigidbody.velocity = Vector2.zero;
                playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
                playerRigidbody.gravityScale = 0f;
                Debug.Log("사다리를 잡았다");
            }
        }
    }

    // 아이템 획득시 실행 함수 (1)
    //public void GetItem(string name)
    //{
    //    itemManager.GetComponent<Inventory>().Division(name);
    //}
}