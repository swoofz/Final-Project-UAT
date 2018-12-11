﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Pawn {

    // Note:: Hard Hitter cause not much animations to work with when comes to attack

    public override void ChangeAnimationState(string state) {
        Animator anim = GetComponent<Animator>();

        if (state == "JumpAtt" ) {
            anim.Play(state);
        } else {
            base.ChangeAnimationState(state);
        }
    }

    public override bool IsGrounded() {
        // Knight is grounded 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 3f);
        if (hit.collider != null) {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "Platform") {
                return true;
            }
        }

        return base.IsGrounded();
    }

    public override void Attack(string attState, float damage) {
        if (attState == "Attack") {
            // Do Basic Attack
        }
        if (attState == "JumpAttack") {
            // Do a Jump Attack
        }
        if (attState == "ChargeAtt") {
            // Do a Charge Attack
        }
    }

    // Attack stuff below
    void Attack(float damage) {
        // Do Attack Actions
    }

    void JumpAttack(float damage) {
        // Do Jump Attack Actions
    }

    void ChargeAttack(float damage) {
        // Charge Attack then send the damage
    }

}
