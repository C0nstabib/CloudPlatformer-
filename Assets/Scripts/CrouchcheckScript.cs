using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchcheckScript : MonoBehaviour
{
    [SerializeField] PlayerMovement playerScript;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerScript.mustCrouch = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerScript.mustCrouch = false;
            playerScript.exitCrouch = true;
        }
    }
}
