using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0.6f, -10f);
    float smoothtime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    public bool camLock;

    [SerializeField] public Transform target;
   
    void FixedUpdate()
    {
        if(camLock == false)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothtime);
        }
    }
}
