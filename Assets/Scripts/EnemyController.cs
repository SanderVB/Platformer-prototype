using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float scaleSize = 3f;

    Rigidbody2D myRB2D;
    BoxCollider2D ledgeScanner;

	// Use this for initialization
	void Start ()
    {
        myRB2D = GetComponent<Rigidbody2D>();
        ledgeScanner = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        myRB2D.velocity = new Vector2(moveSpeed, 0f);
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Ground")
            FlipEnemy();
    }

    private void FlipEnemy()
    {
        moveSpeed = -moveSpeed;
        scaleSize = -scaleSize;
        transform.localScale = new Vector2(transform.localScale.x + (2 * scaleSize), transform.localScale.y);
    }

}
