using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {

    private CharacterState playerState;             // Create a varaible to hold our state
    private float direction;                // Create a varaible to store the direction
    private Rigidbody2D rb;                 // Create a varaible to store our Rigibody component

	// Use this for initialization
	void Start () {
        pawn = gameObject.GetComponentInChildren<Pawn>();
    }
	
	// Update is called once per frame
	void Update () {
        playerState = ChangeState();
        direction = Input.GetAxis("Horizontal");
        pawn.MoveDirection(direction);
        pawn.ChangeAnimationState(playerState.ToString());
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
        playerState = CharacterState.Idle;

        if (direction != 0) {
            playerState = CharacterState.Run;
        }

        return playerState;
    }
}
