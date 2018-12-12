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
        Transform feet = transform.GetChild(4);
        RaycastHit2D hit = Physics2D.Raycast(feet.position, Vector2.down, .5f);
        if (hit.collider != null) {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "Platform") {
                if (hit.collider.tag == "Platform" && Input.GetKey(KeyCode.S)) {
                    return false;
                }
                return true;
            }
        }

        return base.IsGrounded();
    }

    public override float Attack() {
        // TODO:: Add a charge Attack after add charge attack animation
        return attackDamage;
    }
}
