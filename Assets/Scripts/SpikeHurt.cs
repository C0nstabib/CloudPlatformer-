using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeHurt : MonoBehaviour
{
    [SerializeField] GameObject playerObj;
    public PlayerMovement playerScript;
    public Vector2 resetPoint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerScript.waterMeter = playerScript.waterMeter-35;
            if (playerScript.waterMeter >= 0)
            {
                playerObj.transform.position = resetPoint;
            }
        }
    }
    
}
