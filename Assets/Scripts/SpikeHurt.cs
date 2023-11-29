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
        if (collision.gameObject.CompareTag("Player") && !collision.gameObject.layer.Equals("Attack"))
        {
            playerScript.KBCounter = playerScript.KBTotalTime;
            if (collision.transform.position.x <= transform.position.x)
            {
                playerScript.KnockFromRight = true;
            }
            else
            {
                playerScript.KnockFromRight = false;
            }
            
            playerScript.waterMeter = playerScript.waterMeter-35;
            StartCoroutine(SpikeStun());
        }
    }
    IEnumerator SpikeStun()
    {
        yield return new WaitForSeconds(0.1f);
        if(playerScript.waterMeter >= 0)
        {
            playerObj.transform.position = resetPoint;
        }
    }
    
}
