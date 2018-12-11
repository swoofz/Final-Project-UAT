using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {

    private CharacterState playerState;             // Create a varaible to hold our state
    private float direction;                // Create a varaible to store the direction
    private Rigidbody2D rb;                 // Create a varaible to store our Rigibody component

    private bool attack;
    private float cnt, cntD;


	// Use this for initialization
	void Start () {
        pawn = gameObject.GetComponentInChildren<Pawn>();
        rb = GetComponent<Rigidbody2D>();
        cnt = 0.35f;
    }
	
	// Update is called once per frame
	void Update () {
        playerState = ChangeState();
        direction = Input.GetAxis("Horizontal");
        pawn.ChangeAnimationState(playerState.ToString());

        if (pawn.IsGrounded()) {
            rb.gravityScale = 0;
            if (rb.velocity.y < 0) {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        } else {
            rb.gravityScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && pawn.IsGrounded()) {
            if (Input.GetKey(KeyCode.S)) {          // Jump Down
                pawn.Jump(-1, rb);
            } else {                                // Jump Up
                pawn.Jump(1, rb);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            attack = true;
            cntD = cnt;
        }

        if (attack) {
            direction = 0;
            cntD -= Time.deltaTime;
            if (cntD < 0) {
                attack = false;
            }
        }


        pawn.MoveDirection(direction);
    }

    //void Damage() {
    //    // the effect of taking damage (idea)
    //    if (Input.GetKeyDown(KeyCode.E)) {
    //        rb.AddForce(new Vector2(0.5f, 0.7f) * 500);
    //    }
    //    if (Input.GetKeyDown(KeyCode.Q)) {
    //        rb.AddForce(new Vector2(-0.5f, 0.7f) * 500);
    //    }
    //}

    CharacterState ChangeState() {
        if (direction == 0) {
            playerState = CharacterState.Idle;
        }

        if (direction != 0) {
            playerState = CharacterState.Run;
        }

        if (attack) {
            playerState = CharacterState.Attack;
        }

        if (!pawn.IsGrounded()) {
            playerState = CharacterState.Jump;
        }

        return playerState;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (attack) {
            Debug.Log(collision.gameObject.name);
        }
    }
}
