using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody2D playerRigidbody = default;
    private SpriteRenderer playerRenderer = null;
    public SpriteRenderer[] playerDeathScreen = new SpriteRenderer[2];
    private Animator animator = default;
    private Collider2D[] attackCollider;
    public GameObject arrowPrefab;
    public Collider2D playerCollider_;
    private GameObject thinFloor;
    public Transform playerContainer;
    private GameObject monster;
    public GameObject playerUi;
    public GameObject[] playerAttackEffect = new GameObject[4];
    public ParticleSystem attackParticle;
    public AudioClip jumpAudio;
    public AudioClip melee1Audio;
    public AudioClip melee2Audio;
    public AudioClip melee3Audio;
    public AudioClip walkAudio;
    public AudioClip shootArrowAudio;
    public AudioClip hurtAudio;
    public AudioClip rollAudio;
    public AudioClip deathAudio;
    private AudioSource playerAudio = default;

    // 화살 이펙트 테스트
    public GameObject test;
    private GameObject test2;

    private Vector2 attackSize = default;
    private Vector2 attackVector = default;

    private float moveForce = default;     // 캐릭터가 움직일 힘 수치
    private float rollForce = default;
    private float xInput = default;     // 수평 움직임 입력값
    private float xSpeed = default;     // 수평 움직임 최종값
    private float rSpeed = default;
    private float[] jSpeed = new float[2];
    private float jumpForce = default;
    private float climbSpeed = default;
    private float climbDirection = default;
    private float chargeForce = default;
    private float chargeAddForce = default;
    private float chargeMax = default;
    private float playerAlpha = default;

    private int jumpCount = default;
    private int isMlAttack = default;
    public int playerHp = default;
    public int playerMaxHp = default;
    private int poisonCount = default;

    private bool jumping = false;
    private bool jumpingForce = false;
    private bool flipX = false;
    private bool isRolled = false;
    [SerializeField] private bool isGrounded = false;
    private bool isCrouched = false;
    [SerializeField] private bool isLadder = false;
    private bool isAirAttacked = false;
    private bool isBowed = false;
    private bool isChargeBowed = false;
    private bool isAirBowed = false;
    private bool isChargeAirBowed = false;
    private bool isCrouchBowed = false;
    private bool isChargeCrouchBowed = false;
    private bool[] mlAttackConnect = new bool[2];
    private bool onLadderTop = false;
    private bool onLadderBot = false;
    private bool isCharged = false;
    [SerializeField] private bool thinFloorCheck = false;
    [SerializeField] private bool forceLadder = false;
    private bool isHited = false;
    private bool hitMoveTime = false;
    private bool isPoison = false;
    private bool walkAudioCheck = false;

    public Rigidbody2D platformBody;
    public bool isMovingPlatform = false;

    void Awake()
    {
        // { 변수 값 선언
        playerRigidbody = GetComponent<Rigidbody2D>();

        playerCollider_ = transform.Find("CrashCollider").GetComponent<BoxCollider2D>();
        BoxCollider2D FloorDetectCollider = transform.Find("FloorDetectCollider").GetComponent<BoxCollider2D>();
        BoxCollider2D BorderCollider = transform.Find("BorderCollider").GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider_, FloorDetectCollider, true);
        Physics2D.IgnoreCollision(FloorDetectCollider, BorderCollider, true);
        Physics2D.IgnoreCollision(BorderCollider, playerCollider_, true);

        playerRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        attackSize = new Vector2(2f, 2f);

        playerUi = GameObject.Find("GamingUiManager");

        moveForce = 10f;
        rollForce = 10f;
        jumpForce = 0.3f;
        xInput = 0f;
        xSpeed = 0f;
        rSpeed = 0f;
        jSpeed[0] = 0f;
        jSpeed[1] = 0f;
        climbSpeed = 3f;
        climbDirection = 0f;
        chargeForce = 0f;
        chargeAddForce = 1f;
        chargeMax = 2000f;
        playerHp = 30;
        playerMaxHp = 30;
        jumpCount = 0;
        isMlAttack = 0;
        playerAlpha = 1f;
        poisonCount = 0;
        // } 변수 값 선언
    }     // End Awake()

    void Start()
    {
        playerRigidbody.velocity = new Vector2(0f, 0f);
        
    }

    void Update()
    {
        if (ItemManager.instance != null && ItemManager.instance.lookAtInventory == true) { return; }
        if (ItemManager.instance != null && ItemManager.instance.lookAtGameMenu == true) { return; }
        if (GameManager.instance != null && GameManager.instance.isDeath == true) { return; }

        //안맞은 상태
        if (hitMoveTime == false)
        {
            xInput = Input.GetAxis("Horizontal");     // 수평 입력값 대입
            
            //구르는 키 입력
            if (isRolled == true)
            {
                if (flipX == false)
                {
                    rSpeed = rollForce;
                    //이동식 플랫폼 위에 있는거 처리
                    //platform 위에 있을 경우 플랫폼 이동 속도만큼 추가로 이동해야함
                    if (isMovingPlatform)
                    {
                        playerRigidbody.velocity = new Vector2(rSpeed + platformBody.velocity.x, playerRigidbody.velocity.y);
                    }
                    else
                    {
                        playerRigidbody.velocity = new Vector2(rSpeed, playerRigidbody.velocity.y);
                    }
                }
                else
                {
                    rSpeed = rollForce;

                    if (isMovingPlatform)
                    {
                        playerRigidbody.velocity = new Vector2(-rSpeed + platformBody.velocity.x, playerRigidbody.velocity.y);
                    }
                    else
                    {
                        playerRigidbody.velocity = new Vector2(-rSpeed, playerRigidbody.velocity.y);
                    }
                }
            }
            else
            {
                //활 차징 상태 아닐경우
                if (isBowed == false)
                {
                    if (isCrouched == false && isCrouchBowed == false && isMlAttack == 0)
                    {
                        xSpeed = xInput * moveForce;
                        Vector2 newVelocity = new Vector2(xSpeed, playerRigidbody.velocity.y);     // 수평, 수직 입력값만큼 플레이어 이동 좌표 설정

                        if (xSpeed != 0 && isGrounded == true && walkAudioCheck == false)
                        {
                            walkAudioCheck = true;
                            playerAudio.clip = walkAudio;
                            playerAudio.Play();
                            StartCoroutine(WalkAudioDelay());
                        }
                        //땅에서 일어나는 velocity이동엔 플랫폼 이동을 고려해야함
                        if (isMovingPlatform)
                        {
                            playerRigidbody.velocity = new Vector2(xSpeed+platformBody.velocity.x, playerRigidbody.velocity.y);
                        }
                        else
                        {
                            playerRigidbody.velocity = newVelocity;
                        }
                    }
                }
            }
        }

        if (isRolled == false && hitMoveTime == false)
        {
            if (xSpeed > 0f) { if (flipX == true) { flipX = false; playerRenderer.flipX = false; } }

            if (xSpeed < 0f) { if (flipX == false) { flipX = true; playerRenderer.flipX = true; } }
        }

        if (forceLadder == true && isLadder == true)
        {
            climbDirection = climbSpeed * Input.GetAxisRaw("Vertical");
            if (climbDirection > 0f) { playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, climbDirection); }
            else if (climbDirection < 0f) { playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, climbDirection * 5f); }
            else
            {
                playerRigidbody.velocity = Vector2.zero;
                climbDirection = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.A) && jumpCount < 2 && isLadder == false && isAirAttacked == false && isBowed == false && hitMoveTime == false && isRolled == false && isMlAttack == 0)
        {
            if (isCrouched == true && thinFloorCheck == true && thinFloor != null) { StartCoroutine(ThinFloorEnter()); }
            else
            {
                playerAudio.clip = jumpAudio;
                playerAudio.Play();
                //2단 점프시 파워 제한
                if (jumpCount == 0) { jSpeed[0] = 7; }
                else if (jumpCount == 1) { jSpeed[1] = 7 * 0.8f; }

                jumpCount += 1;
                jumping = true;
                jumpingForce = true;
            }
        }

        if (Input.GetKey(KeyCode.A) && jumping == true && jumpingForce == true)
        {
            if (jumpCount == 1)
            {
                jSpeed[0] += jumpForce;
                if (jSpeed[0] > 12f) { jSpeed[0] = 12f; jumpingForce = false; }
            }
            //2단 점프시 파워 제한
            else if (jumpCount == 2)
            {
                jSpeed[1] += jumpForce;
                if (jSpeed[1] > 12f*0.8f) { jSpeed[1] = 12*0.8f; jumpingForce = false; }
            }
        }

        if (jumpingForce == true && isAirAttacked == false)
        {
            if (jumpCount == 1)
            {
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jSpeed[0]);
            }
            else if (jumpCount == 2)
            {
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jSpeed[1]);
            }
        }

        if (Input.GetKeyUp(KeyCode.A)) { jumpingForce = false; }

        if (Input.GetKeyDown(KeyCode.Q) && isRolled == false && isGrounded == true && isMlAttack == 0)
        {
            if (isCrouched == true) { isCrouched = false; }

            playerAudio.clip = rollAudio;
            playerAudio.Play();

            playerRigidbody.velocity = Vector2.zero;
            xSpeed = 0f;
            xInput = 0f;
            isRolled = true;
        }


        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (isLadder == true) { forceLadder = true; }
            else
            {
                playerRigidbody.velocity = Vector2.zero;
                xSpeed = 0f;
                xInput = 0f;
                isCrouched = true;
            }
        }

        if (Input.GetKey(KeyCode.DownArrow) && isLadder == true && onLadderBot == true)
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.None;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRigidbody.gravityScale = 3f;
            isLadder = false;
            forceLadder = false;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isCrouched = false;
            if (isCrouchBowed == true) { isCrouchBowed = false; }

            if (isLadder == true)
            {
                forceLadder = false;
                climbDirection = 0f;
                playerRigidbody.velocity = Vector2.zero;
            }
        }


        if (Input.GetKeyDown(KeyCode.UpArrow) && canInteract)
        {

            if (canItem)
            {
                ItemManager.instance.GetComponent<Inventory>().GetItem(GameManager.instance.currMap.GetComponent<MapEvent>().eventName);

                MapEvent _event = GameManager.instance.currMap.GetComponent<MapEvent>().Copy();
                _event.canActive = false;
                GameManager.instance.eventManager.eventCheck.Add(GameManager.instance.currMap.name.Split("(Clone)")[0], _event);

            }

            else if (canSave)
            {
                GameManager.instance.SaveBefore();
            }

            else if (canTalk)
            {

            }
        }

        else
        {
            if (isLadder == true)
            {
                forceLadder = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (isLadder == true)
            {
                forceLadder = false;
                climbDirection = 0f;
                playerRigidbody.velocity = Vector2.zero;
            }
        }

        if (isLadder == true && Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.A))
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.None;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRigidbody.gravityScale = 3f;
            isLadder = false;
            jumping = true;
            jumpingForce = true;
            jumpCount += 1;
            xSpeed = -50f;
            flipX = true;
            playerRenderer.flipX = true;
        }

        if (isLadder == true && Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.A))
        {
            playerRigidbody.constraints = RigidbodyConstraints2D.None;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            playerRigidbody.gravityScale = 3f;
            isLadder = false;
            jumping = true;
            jumpingForce = true;
            jumpCount += 1;
            xSpeed = 50f;
            flipX = false;
            playerRenderer.flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            /*
            일반공격 1타 : 1
            일반공격 2타 : 2
            일반공격 3타 : 3
            점프 공격 : 2
            화살 : 1
            충전 화살 : 2
            */

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
                    else if (isMlAttack == 1) { mlAttackConnect[0] = true; }
                    else if (isMlAttack == 2) { mlAttackConnect[1] = true; }
                }
            }
            else if (isAirAttacked == false) { isAirAttacked = true; }
        }

        if (Input.GetKeyDown(KeyCode.D) && isBowed == false && isAirBowed == false && isCrouchBowed == false && isChargeBowed == false && 
            isChargeCrouchBowed == false) { isCharged = true; }

        if (Input.GetKey(KeyCode.D) && isCharged == true)
        {
            chargeForce += chargeAddForce;
            if (chargeForce >= chargeMax)
            {
                chargeForce = chargeMax;
            }
        }

        if (Input.GetKeyUp(KeyCode.D) && isCharged == true)
        {
            if (isCrouched == true)
            {
                if (chargeForce >= chargeMax) { isChargeCrouchBowed = true; }
                else { isCrouchBowed = true; }
            }
            else if (isGrounded == false)
            {
                if (chargeForce >= chargeMax) { isChargeAirBowed = true; }
                else { isAirBowed = true; }
            }
            else if (isGrounded == true)
            {
                if (chargeForce >= chargeMax) { isChargeBowed = true; }
                else { isBowed = true; }
            }
            
            isCharged = false;
            chargeForce = 0f;
            xSpeed = 0f;
        }

        if (Input.GetKeyDown(KeyCode.W) && ItemManager.instance.activeItemNum != 0)
        {
            UseItem();
        }

        if (Input.GetKeyDown(KeyCode.T) && ItemManager.instance.lookAtInventory == false)
        {
            ItemManager.instance.lookAtInventory = true;
            ItemManager.instance.GetComponent<Inventory>().enabled = true;
            ItemManager.instance.inventoryUi.SetActive(true);
            Time.timeScale = 0f;
        }

        //테스트 코드1
        if (Input.GetKeyDown(KeyCode.I))
        {

            ItemManager.instance.GetComponent<Inventory>().GetItem("세공 반지");
            ItemManager.instance.GetComponent<Inventory>().GetItem("아스트랄 부적");
            ItemManager.instance.GetComponent<Inventory>().GetItem("초롱꽃");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (ItemManager.instance.lookAtGameMenu == false)
            {
                playerUi.GetComponent<PlayerUi>().GameMenuOn();
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            test2 = Instantiate(test, playerContainer.position + Vector3.down * .2f, Quaternion.identity);
            StartCoroutine(ArrowEffectEnd());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            attackParticle.gameObject.SetActive(true);
            if (!attackParticle.isPlaying)
            {
                attackParticle.transform.position = transform.position;
                attackParticle.Play();
            }
        }

        animator.SetBool("Ground", isGrounded);
        animator.SetBool("Roll", isRolled);
        animator.SetBool("Crouch", isCrouched);
        animator.SetBool("AirAttack", isAirAttacked);
        animator.SetBool("Bow", isBowed);
        animator.SetBool("ChargeBow", isChargeBowed);
        animator.SetBool("AirBow", isAirBowed);
        animator.SetBool("ChargeAirBow", isChargeAirBowed);
        animator.SetBool("CrouchBow", isCrouchBowed);
        animator.SetBool("ChargeCrouchBow", isChargeCrouchBowed);
        animator.SetBool("Hurt", hitMoveTime);
        animator.SetBool("Jump", jumpingForce);
        animator.SetInteger("MlAttack", isMlAttack);
        animator.SetInteger("Run", (int)xSpeed);

        if (playerHp <= 0) { StartCoroutine(PlayerDeath()); }
    }

    IEnumerator ArrowEffectEnd()
    {
        yield return new WaitForSeconds(0.5f);
        test2.gameObject.SetActive(false);
        Destroy(test2, 1f);
    }

    public void UseItem()
    {
        if (ItemManager.instance.activeItemNum == 1 && ItemManager.instance.activeItemCount[1] > 0)
        {
            if (playerHp + 10 <= playerMaxHp)
            {
                playerHp += 10;
            }
            else
            {
                playerHp = playerMaxHp;
            }
        }
    }

    //히트시에 모든 행동 bool값 초기화 된게 맞는지 확인
    public void Hit(int damage, int location)
    {
        if (isRolled == true) { return; }
        if (isHited == true) { return; }

        playerAudio.clip = hurtAudio;
        playerAudio.Play();

        playerHp -= damage;

        if (playerHp <= 0)
        {
            StartCoroutine(PlayerDeath());
        }
        else
        {
            playerRigidbody.velocity = Vector2.zero;
            xSpeed = 0f;
            jSpeed[0] = 0f;
            jSpeed[1] = 0f;
            isHited = true;
            hitMoveTime = true;
            isMlAttack = 0;
            mlAttackConnect[0] = false;
            mlAttackConnect[1] = false;
            chargeForce = 0f;

            if (location == 1) { playerRigidbody.velocity = new Vector2(-6f, 11f); }
            else if (location == -1) { playerRigidbody.velocity = new Vector2(6f, 11f); }

            playerUi.GetComponent<PlayerUi>().PlayerHpBar(playerHp);
            StartCoroutine(InvinTime());
            StartCoroutine(HitMoveTime());
        }
    }

    public void HitPoison()
    {
        if (isPoison == false)
        {
            poisonCount = 0;
            StartCoroutine(PoisonDamage());
        }
        else { poisonCount = 0; }
    }

    IEnumerator PoisonDamage()
    {
        isPoison = true;
        while (isPoison == true)
        {
            yield return new WaitForSeconds(1f);
            if (poisonCount < 10)
            {
                playerHp -= 1;
                playerUi.GetComponent<PlayerUi>().PlayerHpBar(playerHp);
                Debug.Log("독 데미지");
                Debug.Log(poisonCount);
                poisonCount += 1;
            }
            else
            {
                Debug.Log("독 데미지 종료");
                isPoison = false;
                break;
            }
        }
    }

    IEnumerator PlayerDeath()
    {
        GameManager.instance.isDeath = true;

        playerAudio.clip = deathAudio;
        playerAudio.Play();

        animator.SetTrigger("Death");
        playerDeathScreen[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        playerDeathScreen[0].gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        playerDeathScreen[1].gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < 20; i++)
        {
            playerAlpha -= 0.05f;
            playerRenderer.color = new Color(255, 255, 255, playerAlpha);
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameOverScene");
    }

    IEnumerator InvinTime()
    {
        for (int i = 0; i < 40; i++)
        {
            playerRenderer.enabled = false;
            yield return new WaitForSeconds(0.05f);
            playerRenderer.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }

        isHited = false;
    }

    IEnumerator HitMoveTime()
    {
        yield return new WaitForSeconds(0.5f);

        hitMoveTime = false;
    }

    IEnumerator WalkAudioDelay()
    {
        yield return new WaitForSeconds(0.4f);
        walkAudioCheck = false;
    }

    // Fix : 플레이어 행동 조건 요약
    public bool IsJump()
    {
        return (isGrounded == false && jumpCount < 2 && isLadder == false && isAirAttacked == false && isBowed == false);
    }

    public void PlayerRollingEnd()
    {
        rSpeed = 0f;
        isRolled = false;
        playerRigidbody.velocity = Vector2.zero;
    }

    IEnumerator RollStartCheck()
    {
        yield return new WaitForSeconds(0.2f);
        ItemManager.instance.lookAtInventory = false;
    }

    public void PlayerBowCheck()
    {
        if (isBowed == true) { PlayerBowShot(); }
        else if (isChargeBowed == true) { PlayerChargeBowShot(); }
    }

    public void PlayerBowShot()
    {      // Fix : Vector3 up
        playerAudio.clip = shootArrowAudio;
        playerAudio.Play();
        GameObject tempObject = Instantiate(arrowPrefab, playerContainer.position+Vector3.up * .5f, Quaternion.identity);
        Vector3 direction = new Vector2(Mathf.Cos((0) * Mathf.Deg2Rad), Mathf.Sin((0) * Mathf.Deg2Rad));
        if (flipX == false)
        {
            tempObject.transform.right = direction;
        }
        else
        {
            tempObject.transform.right = -direction;
        }
    }

    public void PlayerChargeBowShot()
    {
        playerAudio.clip = shootArrowAudio;
        playerAudio.Play();
        for (int i = 0; i < 3; i++)
        {
            GameObject tempObject = Instantiate(arrowPrefab, playerContainer.transform.position + Vector3.up * .5f, Quaternion.identity);
            Vector3 direction = new Vector2(Mathf.Cos((-8 + 8 * i) * Mathf.Deg2Rad), Mathf.Sin((-8 + 8 * i) * Mathf.Deg2Rad));
            if (flipX == false)
            {
                tempObject.transform.right = direction;
            }
            else
            {
                tempObject.transform.right = -direction;
            }
        }
    }

    public void PlayerBowEnd() { isBowed = false; isChargeBowed = false; }

    public void PlayerAirBowCheck()
    {
        if (isAirBowed == true) { PlayerAirBowShot(); }
        else if (isChargeAirBowed == true) { PlayerChargeAirBowShot(); }
    }

    public void PlayerAirBowShot()
    {
        playerAudio.clip = shootArrowAudio;
        playerAudio.Play();
        GameObject tempObject = Instantiate(arrowPrefab, playerContainer.transform.position + Vector3.up * .5f, Quaternion.identity);
        Vector3 direction = new Vector2(Mathf.Cos((0) * Mathf.Deg2Rad), Mathf.Sin((0) * Mathf.Deg2Rad));
        if (flipX == false)
        {
            tempObject.transform.right = direction;
        }
        else
        {
            tempObject.transform.right = -direction;
        }
    }

    public void PlayerChargeAirBowShot()
    {
        playerAudio.clip = shootArrowAudio;
        playerAudio.Play();
        for (int i = 0; i < 3; i++)
        {
            GameObject tempObject = Instantiate(arrowPrefab, playerContainer.transform.position + Vector3.up * .5f, Quaternion.identity);
            Vector3 direction = new Vector2(Mathf.Cos((-8 + 8 * i) * Mathf.Deg2Rad), Mathf.Sin((-8 + 8 * i) * Mathf.Deg2Rad));
            if (flipX == false)
            {
                tempObject.transform.right = direction;
            }
            else
            {
                tempObject.transform.right = -direction;
            }
        }
    }

    public void PlayerAirBowEnd() { isAirBowed = false; isChargeAirBowed = false; }

    public void PlayerCrouchBowCheck()
    {
        if (isCrouchBowed == true) { PlayerCrouchBowShot(); }
        else if (isChargeCrouchBowed == true) { PlayerChargeCrouchBowShot(); }
    }

    public void PlayerCrouchBowShot()
    {
        playerAudio.clip = shootArrowAudio;
        playerAudio.Play();
        GameObject tempObject = Instantiate(arrowPrefab, playerContainer.transform.position - Vector3.up * .5f, Quaternion.identity);
        Vector3 direction = new Vector2(Mathf.Cos((0) * Mathf.Deg2Rad), Mathf.Sin((0) * Mathf.Deg2Rad));
        if (flipX == false)
        {
            tempObject.transform.right = direction;
        }
        else
        {
            tempObject.transform.right = -direction;
        }
    }

    public void PlayerChargeCrouchBowShot()
    {
        playerAudio.clip = shootArrowAudio;
        playerAudio.Play();
        for (int i = 0; i < 3; i++)
        {
            GameObject tempObject = Instantiate(arrowPrefab, playerContainer.transform.position - Vector3.up * .5f, Quaternion.identity);
            Vector3 direction = new Vector2(Mathf.Cos((-8 + 8 * i) * Mathf.Deg2Rad), Mathf.Sin((-8 + 8 * i) * Mathf.Deg2Rad));
            if (flipX == false)
            {
                tempObject.transform.right = direction;
            }
            else
            {
                tempObject.transform.right = -direction;
            }
        }
    }

    public void PlayerCrouchBowEnd() { isCrouchBowed = false; isChargeCrouchBowed = false; }

    public void PlayerMlAttack()
    {
        if (isMlAttack == 1)
        {
            playerAudio.clip = melee1Audio;
            playerAudio.Play();
            if (flipX == false)
            {
                playerRigidbody.velocity = new Vector2(+4f, 0f);
                attackVector = new Vector2(playerRigidbody.position.x + 2f, playerRigidbody.position.y);
                playerAttackEffect[0].gameObject.SetActive(true);
                playerAttackEffect[0].GetComponent<AttackEffect01>().effectRenderer.flipX = false;
            }
            else
            {
                playerRigidbody.velocity = new Vector2(-4f, 0f);
                attackVector = new Vector2(playerRigidbody.position.x - 2f, playerRigidbody.position.y);
                playerAttackEffect[0].gameObject.SetActive(true);
                playerAttackEffect[0].GetComponent<AttackEffect01>().effectRenderer.flipX = true;
            }
        }
        else if (isMlAttack == 2)
        {
            playerAudio.clip = melee2Audio;
            playerAudio.Play();
            if (flipX == false)
            {
                playerRigidbody.velocity = new Vector2(+4f, 0f);
                attackVector = new Vector2(playerRigidbody.position.x + 2f, playerRigidbody.position.y);
                playerAttackEffect[1].gameObject.SetActive(true);
                playerAttackEffect[1].GetComponent<AttackEffect02>().effectRenderer.flipX = false;
            }
            else
            {
                playerRigidbody.velocity = new Vector2(-4f, 0f);
                attackVector = new Vector2(playerRigidbody.position.x - 2f, playerRigidbody.position.y);
                playerAttackEffect[1].gameObject.SetActive(true);
                playerAttackEffect[1].GetComponent<AttackEffect02>().effectRenderer.flipX = true;
            }
        }
        else if (isMlAttack == 3)
        {
            playerAudio.clip = melee3Audio;
            playerAudio.Play();
            if (flipX == false)
            {
                playerRigidbody.velocity = new Vector2(+6f, 0f);
                attackVector = new Vector2(playerRigidbody.position.x + 2f, playerRigidbody.position.y);
                playerAttackEffect[2].gameObject.SetActive(true);
                playerAttackEffect[2].GetComponent<AttackEffect03>().effectRenderer.flipX = false;
            }
            else
            {
                playerRigidbody.velocity = new Vector2(-6f, 0f);
                attackVector = new Vector2(playerRigidbody.position.x - 2f, playerRigidbody.position.y);
                playerAttackEffect[2].gameObject.SetActive(true);
                playerAttackEffect[2].GetComponent<AttackEffect03>().effectRenderer.flipX = true;
            }
        }

        attackCollider = Physics2D.OverlapBoxAll(attackVector, attackSize, 0f);

        for (int i = 0; i < attackCollider.Length; i++)
        {
            if (attackCollider[i].tag == ("Enemy"))
            {
                monster = attackCollider[i].gameObject;
                monster.GetComponentInParent<IHitControl>().Hit(5, flipX ? 1 : -1);
                if (!attackParticle.isPlaying)
                {
                    attackParticle.Play();
                }
            }
        }
    }


    public void PlayerAirAttack()
    {
        playerAudio.clip = melee2Audio;
        playerAudio.Play();
        if (flipX == false)
        {
            attackVector = new Vector2(playerRigidbody.position.x + 2f, playerRigidbody.position.y);
            playerAttackEffect[3].gameObject.SetActive(true);
            playerAttackEffect[3].GetComponent<AirAttackEffect>().effectRenderer.flipX = false;
        }
        else
        {
            attackVector = new Vector2(playerRigidbody.position.x - 2f, playerRigidbody.position.y);
            playerAttackEffect[3].gameObject.SetActive(true);
            playerAttackEffect[3].GetComponent<AirAttackEffect>().effectRenderer.flipX = true;
        }

        attackCollider = Physics2D.OverlapBoxAll(attackVector, attackSize, 0f);
        for (int i = 0; i < attackCollider.Length; i++)
        {
            if (attackCollider[i].tag == ("Enemy"))
            {
                monster = attackCollider[i].gameObject;
                monster.GetComponentInParent<IHitControl>().Hit(5, flipX ? 1 : -1);
            }
        }
    }

    public void PlayerMlAttackEnd()
    {
        if (isMlAttack == 1)
        {
            playerAttackEffect[0].gameObject.SetActive(false);
            if (mlAttackConnect[0] == true) { isMlAttack = 2; mlAttackConnect[0] = false; }
            else { isMlAttack = 0; }
        }
        else if (isMlAttack == 2)
        {
            playerAttackEffect[1].gameObject.SetActive(false);
            if (mlAttackConnect[1] == true) { isMlAttack = 3; mlAttackConnect[1] = false; }
            else { isMlAttack = 0; }
        }
        else if (isMlAttack == 3)
        {
            playerAttackEffect[2].gameObject.SetActive(false);
            isMlAttack = 0;
        }
    }

    public bool canInteract = false;
    public bool canSave = false;
    public bool canItem = false;
    public bool canTalk = false;

    public void SetInteraction(InteractObjectType type)
    {
        Debug.Log(type);
        switch (type)
        {
            //save에 필요한 데이터
            //Gamemanager.instance.currMap에서 맵 정보 추출(맵 위치)
            //Gamamanager.instance.eventManager.checkEvent? dictionary에 저장되있는 이벤트들의 현재 상태
            //item 목록
            case InteractObjectType.SAVE:
                canInteract = true;
                canSave = true;
                break;

            //아이템 획득
            //Gamemanager.instance.currMap에서 맵 정보 추출
            //Gamamanager.instance.eventManager.checkEvent? 에서 저장된 데이터 변경
            //string item name
            case InteractObjectType.ITEM:
                canInteract = true;
                canItem = true;
                break;

            //Gamemanager.instance.currMap에서 맵 정보 추출
            //Gamamanager.instance.eventManager.checkEvent? 에서 저장된 데이터 변경
            //npc 대화
            //npc 이름
            //npc 대화 스크립트
            case InteractObjectType.NPC:
                canInteract = true;
                canTalk = true;
                break;

            //모든 bool값 false 처리 
            case InteractObjectType.CLOSE:
                canInteract = false;
                canTalk = false;
                canItem = false;
                canSave = false;
                break;
        }
        //todo : update에 방향키 누를시 저장하는 코드 작성하기
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            if (collision.collider.GetComponent<EnemyBase>() != null)
            {
                Hit(1, flipX ? 1 : -1);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("ThinFloor") && jumping == true) { isGrounded = false; }

        if (collision.gameObject.tag == ("Floor") && jumping == true) { isGrounded = false; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Floor" )
        {
            playerAudio.clip = walkAudio;
            playerAudio.Play();
            isGrounded = true;
            jumpingForce = false;
            jumping = false;
            jumpCount = 0;
            jSpeed[0] = 0f;
            jSpeed[1] = 0f;
            isAirBowed = false;
            isChargeAirBowed = false;
            isAirAttacked = false;
            playerAttackEffect[3].gameObject.SetActive(false);
        }
        if ( collision.tag == "ThinFloor")
        {
            playerAudio.clip = walkAudio;
            playerAudio.Play();
            thinFloorCheck = true;
            thinFloor = collision.gameObject;
            isGrounded = true;
            jumpingForce = false;
            jumping = false;
            jumpCount = 0;
            jSpeed[0] = 0f;
            jSpeed[1] = 0f;
            isAirBowed = false;
            isChargeAirBowed = false;
            isAirAttacked = false;
            playerAttackEffect[3].gameObject.SetActive(false);
        }

        if (collision.gameObject.name == ("LadderDown")) { onLadderTop = true; }

        if (collision.gameObject.name == ("LadderBot")) { onLadderBot = true; }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == ("Ladder") && isLadder == true)
        //{
        //    forceLadder = false;
        //    isLadder = false;
        //    playerRigidbody.constraints = RigidbodyConstraints2D.None;
        //    playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        //    playerRigidbody.gravityScale = 3f;
        //    playerRigidbody.velocity = Vector2.zero;
        //}

        if (collision.gameObject.tag == ("Ladder") && Input.GetKey(KeyCode.UpArrow) && isLadder == false && onLadderTop == false)
        {
            isLadder = true;
            jumpCount = 0;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            playerRigidbody.gravityScale = 0f;
            transform.position = new Vector2(collision.gameObject.transform.position.x, transform.position.y);
            // Lerp 사용하여 부드럽게 사다리 잡기
            //Lerp<>(transform.position, collision.gameObject.transform.position.x);
        }

        if (collision.gameObject.tag == ("Ladder") && Input.GetKey(KeyCode.DownArrow) && isLadder == false && onLadderTop == true)
        {
            isLadder = true;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX;
            playerRigidbody.gravityScale = 0f;
            transform.position = new Vector2(collision.gameObject.transform.position.x, transform.position.y);
            StartCoroutine(ThinFloorEnter());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
      /*  if(collision.tag=="ThinFloor" && collision.gameObject.Equals(thinFloor.gameObject))
        {
            thinFloor = null;
        }*/

        if (collision.gameObject.name == ("LadderTop")) { onLadderTop = false; }

        if (collision.gameObject.name == ("LadderBot")) { onLadderBot = false; }
    }

    IEnumerator ThinFloorEnter()
    {
        CompositeCollider2D thinFloorCollider = thinFloor.GetComponent<CompositeCollider2D>();
        Physics2D.IgnoreCollision(playerCollider_, thinFloorCollider);

        yield return new WaitForSeconds(.3f);

        Physics2D.IgnoreCollision(playerCollider_, thinFloorCollider, false);
        thinFloorCheck = false;
        thinFloor = null;
    }
}

public enum InteractObjectType
{
    SAVE,
    ITEM,
    NPC,
    CLOSE
}