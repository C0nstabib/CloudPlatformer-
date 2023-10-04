using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    [SerializeField] private float speedMultiplier;
    Rigidbody2D rb;
    
    [SerializeField] private bool groundJump;
    [SerializeField] int waterMeter;
    [SerializeField] private int waterMax = 100;
    [SerializeField] private int jumpCost = 20;
   
    private bool overWater;
    float poolTimer;

    bool floatStart;
    float floatTimer;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        waterMeter = waterMax;
        poolTimer = 1f;
        floatTimer = 0.5f;
    }
    public void Update()
    {
       
        rb.velocity = new Vector2(moveInput.x * speedMultiplier, rb.velocity.y);
        if (Input.GetKeyDown("space") && waterMeter > 20 && !floatStart)
        {
            rb.velocity = new Vector2(0, 7);
            if (!groundJump)
            {
                waterMeter -= jumpCost;
            }
        }

        if (overWater && waterMeter < waterMax)
        {
            poolTimer -= 1 * Time.deltaTime * 4;
            if(poolTimer <= 0)
            {
                waterMeter += 10;
                poolTimer = 1;
            }
        }

        if(waterMeter > waterMax)
        {
            waterMeter = waterMax;
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

    public void Float(InputAction.CallbackContext context)
    {
        if(context.performed  && !groundJump)
        {
            floatStart = true;
        }
        else if(context.canceled)
        {
            floatStart = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundJump = true;
        }
        
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundJump = false;
        }
    }
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
