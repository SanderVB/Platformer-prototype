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
        try { GetComponent<Slider>().value = FindObjectOfType<PlayerController>().GetPlayerHealth(); }
        catch {Debug.LogWarning("Script tried to access Player script while no player character is in the scene"); }
    }
}
