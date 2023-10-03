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
    [SerializeField] private bool waterPoolCollide;
    [SerializeField] private bool groundJump;
    [SerializeField] int waterMeter;
    [SerializeField] private int waterMax = 15;
    [SerializeField] private bool overWater;
    float timer;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        waterMeter = waterMax;
        timer = 1f;
    }
    public void Update()
    {
       
        rb.velocity = new Vector2(moveInput.x * speedMultiplier, rb.velocity.y);
        if (Input.GetKeyDown("space") && waterMeter > 0)
        {
            rb.velocity = new Vector2(0, 7);
            if (!groundJump)
            {
                waterMeter -= 1;
            }
        }
        if (groundJump)
        {
            
        }

        if (overWater && waterMeter < waterMax)
        {
            timer -= 1 * Time.deltaTime * 4;
            if(timer <= 0)
            {
                waterMeter += 1;
                timer = 1;
            }
        }

    }
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
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
            waterMeter += 1;
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
