using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public PlayerMovement playerScript;
    [SerializeField] int maxHealth = 20;
    public int enemyHealth;

    private void Start()
    {
        enemyHealth = maxHealth;
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
        }
        else if (collision.gameObject.CompareTag("ThunderAttack"))
        {
            enemyHealth -= 30;
        }
        else if (collision.gameObject.CompareTag("WaterAttack"))
        {
            enemyHealth -= 5;
        }
    }
    private void Update()
    {
        if(enemyHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
