using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Pawn {


    public override bool IsGrounded() {
        // Robot is grounded 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2.8f);
        if (hit.collider != null) {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "Platform") {
                return true;
            }
        }

        return base.IsGrounded();
    }

}
