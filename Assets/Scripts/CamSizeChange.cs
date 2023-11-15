using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class CamSizeChange : MonoBehaviour
{
    bool triggered;
    [SerializeField] int newCamSize;
    [SerializeField] int defaultCamSize;
    [SerializeField] CameraFollow cameraScript;
    [SerializeField] Camera cam;
    [SerializeField] GameObject caveBackground;

    private void Start()
    {
        caveBackground.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !triggered)
        {
            cam.orthographicSize = newCamSize;
            cameraScript.offset = new Vector3(0, -5, -10);
            caveBackground.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !triggered)
        {
            cam.orthographicSize = defaultCamSize;
            cameraScript.offset = new Vector3(0, 2, -10);
            triggered = true;
            
        }
    }
    
}
