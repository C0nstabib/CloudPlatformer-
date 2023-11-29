using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseHiddenRoom : MonoBehaviour
{
    [SerializeField] GameObject hiddenRoomCover;

    private void Start()
    {
        hiddenRoomCover.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hiddenRoomCover.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hiddenRoomCover.SetActive(false);
        }
    }
}
