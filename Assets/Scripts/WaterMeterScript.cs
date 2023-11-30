using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaterMeterScript : MonoBehaviour
{
    Slider waterUI;
    [SerializeField] TextMeshProUGUI waterReading;
    [SerializeField] PlayerMovement playerScript;

    private void Start()
    {
        waterUI = GetComponent<Slider>();
        waterUI.value = playerScript.waterMax;
    }
    private void Update()
    {
        waterUI.value = playerScript.waterMeter;
        waterReading.SetText(waterUI.value + "mL");
    }
}
