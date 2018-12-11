using System.Collections;
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
}
