using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 0, -10f);
    private float smoothtime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;
   
    void Update()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothtime);
    }
}