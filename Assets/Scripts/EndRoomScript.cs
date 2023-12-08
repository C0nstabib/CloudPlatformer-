using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomScript : MonoBehaviour
{
    [SerializeField] GameObject hiddenRoomCover;
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] GameObject colliders;

    private void Start()
    {
        colliders.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            hiddenRoomCover.SetActive(false);
            cameraFollow.transform.position = new Vector3(171.5f, 3, -1);
            cameraFollow.camLock = true;
            colliders.SetActive(true);
        }
    }
   
}
