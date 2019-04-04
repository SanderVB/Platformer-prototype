using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Slider>().maxValue = FindObjectOfType<PlayerController>().GetPlayerHealth();
    }
    private void Update()
    {
        GetComponent<Slider>().value = FindObjectOfType<PlayerController>().GetPlayerHealth();
    }
}
