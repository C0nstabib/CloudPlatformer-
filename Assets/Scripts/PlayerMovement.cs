using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
 
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private float speedMultiplier;
    Rigidbody2D rb;
    public Vector2 currentRespawn;
    [SerializeField] int deathCount;
    [SerializeField] int BreezeBadges;
    public Groundcheck groundScript;
    [SerializeField] GameObject airPuffAttack;
    [SerializeField] GameObject thunderChargeRing;
    [SerializeField] GameObject thunderAttack;
    [SerializeField] GameObject waterJugRespawn;
    [SerializeField] GameObject enemyRespawn;
    
    [SerializeField] Transform waterAttackPoint;
    [SerializeField] GameObject projectilePrefab;
    public bool waterAttackStart;

    [SerializeField] float KBForce;
    public float KBCounter;
    public float KBTotalTime;

    public bool KnockFromRight;

    [SerializeField] Sprite standSprite;
    [SerializeField] Sprite crouchSprite;

    [SerializeField] public int waterMeter;
    [SerializeField] public int waterMax = 100;
    [SerializeField] int jumpCost = 20;

    bool overWater;
    float poolTimer;

    bool floatStart;
    float floatTimer;
    
    bool crouching;
    public bool mustCrouch;
    public bool exitCrouch;
    bool canJump;
    bool airAttackStart;
    bool thunderAttackStart;
    [SerializeField] bool thunderCharged;
    bool charging;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        waterMeter = waterMax;
        poolTimer = 1f;
        floatTimer = 0.5f;
        currentRespawn = Vector2.zero;
        BreezeBadges = 0;
        crouching = false;
        canJump = true;
        airPuffAttack.SetActive(false);
        thunderChargeRing.SetActive(false);
        thunderAttack.SetActive(false);
    }
    public void Update()
    {
       if(waterMeter == 0)
        {
            Respawn();
            for (int i = 0; i < enemyRespawn.transform.childCount; ++i)
            {
                enemyRespawn.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        if(KBCounter <= 0)
        {
            rb.velocity = new Vector2(moveInput.x * speedMultiplier, rb.velocity.y);
            Jump();
            WaterChecks();
            RefillWaterPool();

            if (moveInput.x < 0)
            {
                transform.eulerAngles = new Vector3(0f, 180f, 0f);

            }
            else if (moveInput.x > 0)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);

            }

            if (floatStart && waterMeter > 5)
            {
                rb.velocity = new Vector2(rb.velocity.x, -0.5f);

                floatTimer -= 0.5f * Time.deltaTime * 2;
                if (floatTimer <= 0)
                {
                    waterMeter -= 5;
                    floatTimer = 0.5f;
                }
            }
            if (floatStart && groundScript.groundCheck)
            {
                floatStart = false;
            }
            if (thunderCharged)
            {
                thunderChargeRing.SetActive(true);
            }
            else
            {
                thunderChargeRing.SetActive(false);
            }

            if (exitCrouch)
            {
                exitCrouch = false;
                crouching = false;
                gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
                gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                gameObject.GetComponent<SpriteRenderer>().sprite = standSprite;
            }
        }
        else
        {
            if (KnockFromRight)
            {
                rb.velocity = new Vector2(-KBForce+1, KBForce);
            }
            else if (!KnockFromRight)
            {
                rb.velocity = new Vector2(KBForce-1, KBForce);
            }
            KBCounter -= Time.deltaTime;
        }
    }
       
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump()
    {
        if (Input.GetKeyDown("space") && !floatStart && canJump)
        {
            if (!groundScript.groundCheck && waterMeter > 20)
            {
                rb.velocity = new Vector2(0, 7);
                waterMeter -= jumpCost;
            }
            else if (groundScript.groundCheck)
            {
                rb.velocity = new Vector2(0, 7);
            }
        }
        else if (Input.GetKeyDown("space") && floatStart && canJump)
        {
            floatStart = false;
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
            gameObject.GetComponent<SpriteRenderer>().sprite = standSprite;
            if (!groundScript.groundCheck && waterMeter > 20)
            {
                rb.velocity = new Vector2(0, 7);
                waterMeter -= jumpCost;
            }
            else if (groundScript.groundCheck)
            {
                rb.velocity = new Vector2(0, 7);
            }
        }

    }
    public void Attack(InputAction.CallbackContext context)
    {
       
        
        if (context.started && !crouching && waterMeter > 30 && !airAttackStart && !thunderAttackStart && !waterAttackStart)
        {
            
            StartCoroutine(thunderCharging());
           
        }
        else if (context.canceled && !crouching && waterMeter > 30  && charging && !waterAttackStart)
        {
            StopCoroutine(thunderCharging());
            charging = false;
        }
        else if (context.canceled && !crouching && waterMeter > 30 && !airAttackStart && !thunderAttackStart && thunderCharged && !waterAttackStart)
        {
            waterMeter -= 20;
            thunderCharged = false;
            StartCoroutine(thunderWait());
        }
        else if (context.canceled && waterMeter <= 30 && thunderCharged)
        {
            thunderCharged = false;
        }

        if (context.canceled && !crouching && waterMeter > 10 && !airAttackStart && !thunderAttackStart && !thunderCharged && !charging && !waterAttackStart)
        {
            waterMeter -= 10;
            StartCoroutine(airPuffWait());
        }
        if (context.canceled && crouching && waterMeter > 10 && !airAttackStart && !thunderAttackStart && !thunderCharged && !charging && !waterAttackStart)
        {
            waterMeter -= 5;
           StartCoroutine(waterAttackFunction());
        }
    }
    IEnumerator thunderCharging()
    {
        charging = true;
        yield return new WaitForSeconds(1.5f);
        if (charging)
        {
            charging = false;
            thunderCharged = true;
        }
        else
        {
            thunderCharged = false;
        }
    }
    IEnumerator airPuffWait()
    {
        airAttackStart = true;
        airPuffAttack.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        airPuffAttack.SetActive(false);
        airAttackStart = false;
    }
    IEnumerator thunderWait()
    {
        thunderAttackStart = true;
        thunderAttack.SetActive(true);
        yield return new WaitForSeconds(1f);
        thunderAttack.SetActive(false);
        thunderAttackStart = false;
    }

    public IEnumerator waterAttackFunction()
    {
        waterAttackStart = true;
        Instantiate(projectilePrefab, waterAttackPoint.position, waterAttackPoint.rotation);
        yield return new WaitForSeconds(0.1f);
        waterAttackStart = false;
    }

    public void FloatInput(InputAction.CallbackContext context)
    {
        if(context.performed  && !groundScript.groundCheck && waterMeter > 5)
        {
            waterMeter -= 5;
            floatStart = true;
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.25f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 0.5f);
            gameObject.GetComponent<SpriteRenderer>().sprite = crouchSprite;
        }
        else if(context.canceled || groundScript.groundCheck)
        {
            floatStart = false;
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
            gameObject.GetComponent<SpriteRenderer>().sprite = standSprite;
        }
    }
    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed && groundScript.groundCheck)
        {
            crouching = true;
            canJump = false;
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.25f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 0.5f);
            gameObject.GetComponent<SpriteRenderer>().sprite = crouchSprite;
        }
        else if ((context.canceled || !groundScript.groundCheck) && !mustCrouch)
        {
            crouching = false;
            canJump = true;
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
            gameObject.GetComponent<SpriteRenderer>().sprite = standSprite;
        }
        

    }
    public void RefillWaterPool()
    {
        if (overWater && waterMeter < waterMax )
        {
            poolTimer -= 1 * Time.deltaTime * 4;
            if (poolTimer <= 0)
            {
                waterMeter += 10;
                poolTimer = 1;
            }
        }
    }
    public void WaterChecks()
    {
        if (waterMeter > waterMax)
        {
            waterMeter = waterMax;
        }
        else if (waterMeter < 0)
        {
            waterMeter = 0;
        }
    }
    public void Respawn()
    {
        transform.position = currentRespawn;
        waterMeter = waterMax;
        deathCount++;
        
    }

    
    
    //Water refill from pool
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "waterPool" && waterMeter < waterMax)
        {
            if (!overWater)
            {
                waterMeter += 10;
                overWater = true;
            }
        }
        else if (other.gameObject.tag == "sparklePool" && waterMeter < waterMax)
        {
            currentRespawn = new Vector2(other.transform.position.x, other.transform.position.y + 1);
            for (int i = 0; i < waterJugRespawn.transform.childCount; ++i)
            {
                waterJugRespawn.transform.GetChild(i).gameObject.SetActive(true);
            }
            if(!overWater)
            {
                waterMeter += 10;
                overWater = true;
            }
        }
        else if (other.gameObject.tag == "sparklePool")
        {
            currentRespawn = new Vector2(other.transform.position.x, other.transform.position.y + 1);
            for (int i = 0; i < waterJugRespawn.transform.childCount; ++i)
            {
                waterJugRespawn.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        if (other.gameObject.tag == "GBreeze")
        {
            BreezeBadges += 1;
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "waterJug")
        {
            waterMeter += 40;
            other.gameObject.SetActive(false);
        }
    }
    void OnTriggerExit2D(Collider2D other1)
    {
        if (other1.gameObject.tag == "waterPool" || other1.gameObject.tag == "sparklePool" )
        {
            overWater = false;
        }
    }


}
