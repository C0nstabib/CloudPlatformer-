using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public PlayerMovement playerScript;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] public int maxHealth = 20;
    public int enemyHealth;

    private void Start()
    {
        enemyHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript.KBCounter = playerScript.KBTotalTime;
            if(collision.transform.position.x <= transform.position.x)
            {
                playerScript.KnockFromRight = true;
            }
            else
            {
                playerScript.KnockFromRight = false;
            }
            playerScript.waterMeter -= 30;
        }

        if (collision.gameObject.CompareTag("AirPuff"))
        {
            enemyHealth -= 15;
            StartCoroutine(DamageColour());
        }
        else if (collision.gameObject.CompareTag("ThunderAttack"))
        {
            enemyHealth -= 30;
            StartCoroutine(DamageColour());
        }
        else if (collision.gameObject.CompareTag("WaterAttack"))
        {
            enemyHealth -= 5;
            StartCoroutine(DamageColour());
        }
    }
    private void Update()
    {
        if(enemyHealth <= 0)
        {
            StartCoroutine(DeathColour());
        }
    }
    IEnumerator DamageColour()
    {
        spriteRenderer.color = new Color(0.9150943f, 0.4359647f, 0.4359647f);
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = new Color(1,1,1);
    }
    IEnumerator DeathColour()
    {
        spriteRenderer.color = new Color(0.9150943f, 0.4359647f, 0.4359647f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1, 1, 1);
        gameObject.SetActive(false);
    }
}
