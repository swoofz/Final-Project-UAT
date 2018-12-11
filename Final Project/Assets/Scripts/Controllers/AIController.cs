using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller {


    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        pawn = gameObject.GetComponentInChildren<Pawn>();
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += Vector3.right * Time.deltaTime;

        if (pawn.IsGrounded()) {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        } else {
            rb.gravityScale = 1;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Weapon") {
            Pawn targetPawn = collision.gameObject.transform.parent.GetComponent<Pawn>();
            Debug.Log(targetPawn.speed);
            // pawn.takeWeapondamage
            rb.AddForce(new Vector2(-0.8f,1f) * 100);
        }
    }
}
