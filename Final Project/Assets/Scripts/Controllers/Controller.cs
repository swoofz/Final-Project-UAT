﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour {

    public enum CharacterState { Idle, Run, Attack, Slide, Jump, Shoot, JumpAtt }   // Create an enum for character states
    public enum Characters { Knight, Ninja, Robot, Adverture }                      // Create an enum for character can be

    public Pawn pawn;       // Create a variable to hold this controller pawn
}
