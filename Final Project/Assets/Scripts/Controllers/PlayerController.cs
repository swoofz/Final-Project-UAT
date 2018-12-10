using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {

    private CharacterState playerState;     // Create a varaible to hold our state

	// Use this for initialization
	void Start () {
        playerState = CharacterState.Idle;
        pawn = gameObject.GetComponentInChildren<Pawn>();
    }
	
	// Update is called once per frame
	void Update () {
	}
}
