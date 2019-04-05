using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {

    private enum PickupType { coin, extraLife, battery}
    [SerializeField] PickupType pickupType;
    [SerializeField] AudioClip pickupSound;
    [SerializeField] int pickupValue;
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
        AudioSource.PlayClipAtPoint(pickupSound, this.transform.position);
        if (pickupType == PickupType.battery)
            FindObjectOfType<PlayerController>().ReplenishHealth(pickupValue);
        if(pickupType == PickupType.coin)
            FindObjectOfType<GameSession>().AddToScore(pickupValue);
        if (pickupType == PickupType.extraLife)
            FindObjectOfType<GameSession>().AddToLives(pickupValue);

        Destroy(gameObject);
    }
}
