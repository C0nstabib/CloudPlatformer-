using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrow : MonoBehaviour
{
    int timesWatered;
    [SerializeField] GameObject giantFlower;
    void Start()
    {
        timesWatered = 0;
        giantFlower.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(timesWatered == 4)
        {
            gameObject.SetActive(false);
            giantFlower.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "WaterAttack")
        {
            timesWatered++;
            gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x +0.5f, gameObject.transform.localScale.y +0.5f);
            gameObject.transform.position = new Vector2(gameObject.transform.position.x +0.3f, gameObject.transform.position.y +0.3f);
        }
    }

}

