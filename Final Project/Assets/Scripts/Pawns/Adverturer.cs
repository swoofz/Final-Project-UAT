using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adverturer : Pawn {

    public override bool IsGrounded() {
        // Adverturer is grounded 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 2.5f);
        if (hit.collider != null) {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "Platform") {
                return true;
            }
        }

        return base.IsGrounded();
    }


}
