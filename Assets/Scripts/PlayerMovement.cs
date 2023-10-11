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
    public Groundcheck groundScript;
    GameObject airPuffAttack;

    [SerializeField] Sprite standSprite;
    [SerializeField] Sprite crouchSprite;

    [SerializeField] public int waterMeter;
    [SerializeField] int waterMax = 100;
    [SerializeField] int jumpCost = 20;

    bool overWater;
    float poolTimer;

    bool floatStart;
    float floatTimer;

    bool crouching;
    bool attackStart;
    [SerializeField] bool test;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        waterMeter = waterMax;
        poolTimer = 1f;
        floatTimer = 0.5f;
        currentRespawn = Vector2.zero;
        crouching = false;
        airPuffAttack = GameObject.Find("AirPuffHitbox");
        airPuffAttack.SetActive(false);
    }
    public void Update()
    {
       if(waterMeter == 0)
        {
            Respawn();
        }
       
        rb.velocity = new Vector2(moveInput.x * speedMultiplier, rb.velocity.y);
        Jump();
        WaterChecks();
        RefillWaterPool();
        
        if (moveInput.x < 0)
        {
            this.gameObject.transform.localScale = new Vector2(-1, gameObject.transform.localScale.y);
        }
        else if (moveInput.x > 0)
        {
            this.gameObject.transform.localScale = new Vector2(1, gameObject.transform.localScale.y);
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
    }
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump()
    {
        if (Input.GetKeyDown("space") && !floatStart)
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
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.canceled && !crouching && waterMeter > 25 && !attackStart)
        {
            waterMeter -= 25;
            StartCoroutine(airPuffWait());
        }
    }
    IEnumerator airPuffWait()
    {
        attackStart = true;
        airPuffAttack.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        airPuffAttack.SetActive(false);
        attackStart = false;
    }

    public void FloatInput(InputAction.CallbackContext context)
    {
        if(context.performed  && !groundScript.groundCheck)
        {
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
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0,-0.25f);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 0.5f);
            gameObject.GetComponent<SpriteRenderer>().sprite = crouchSprite;
        }
        else if (context.canceled || !groundScript.groundCheck)
        {
            crouching = false;
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
            gameObject.GetComponent<SpriteRenderer>().sprite = standSprite;
        }
        
    }
    public void RefillWaterPool()
    {
        if (overWater && waterMeter < waterMax)
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
        transform.position = Vector3.zero;
        waterMeter = waterMax;
        deathCount++;
    }

    
    
    //Water refill from pool
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "waterPool" && waterMeter < waterMax)
        {
            waterMeter += 10;
            overWater = true;
        }
    }
    void OnTriggerExit2D(Collider2D other1)
    {
        if (other1.gameObject.tag == "waterPool")
        {
            overWater = false;
        }
    }


}
