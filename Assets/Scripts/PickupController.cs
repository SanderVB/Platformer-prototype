using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {

    [SerializeField] int pickupValue;
    [SerializeField] AudioClip pickupSound;
    bool pickedUp = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(!pickedUp)
                Pickup();
        }
    }

    private void Pickup()
    {
        pickedUp = true;
        AudioSource.PlayClipAtPoint(pickupSound, Camera.main.transform.position);
        FindObjectOfType<GameSession>().AddToScore(pickupValue);
        Destroy(gameObject);
    }
}
