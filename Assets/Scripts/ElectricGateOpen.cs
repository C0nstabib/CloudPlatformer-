using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricGateOpen : MonoBehaviour
{
    [SerializeField] GameObject openGate;
    [SerializeField] GameObject closedGate;
    bool activated;
    void Start()
    {
        closedGate.SetActive(true);
        openGate.SetActive(false);
        activated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ThunderAttack" && activated == false)
        {
            closedGate.SetActive(false);
            openGate.SetActive(true);
            activated = true;
        }
    }
}
