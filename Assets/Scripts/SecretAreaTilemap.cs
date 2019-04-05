using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretAreaTilemap : MonoBehaviour {
    [SerializeField] [Range(1,10)] float transparencyFactor = 3;
    float alphaFactor;
    bool secretAreaActive = false;
    Tilemap thisTilemap;
    Color originalColor, secretColor;

    private void Start()
    {
        alphaFactor = 1 / transparencyFactor;
        thisTilemap = GetComponent<Tilemap>();
        originalColor = thisTilemap.color;
        secretColor = new Color(thisTilemap.color.r, thisTilemap.color.g, thisTilemap.color.b, alphaFactor);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !secretAreaActive)
        {
            Debug.Log("in het geheim");
            Debug.Log(alphaFactor);
            secretAreaActive = true;
            thisTilemap.color = secretColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && secretAreaActive)
        {
            Debug.Log("uit het geheim");
            secretAreaActive = false;
            thisTilemap.color = originalColor;
        }
    }

}
