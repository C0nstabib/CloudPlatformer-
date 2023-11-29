using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterProjPhysics : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] Rigidbody2D rb;
    void Start()
    {
        rb.velocity = new Vector3(transform.right.x * 0.15f, 0.3f, 0f) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Terrain") || collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
