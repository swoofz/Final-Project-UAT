using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Pawn {

    public override void ChangeAnimationState(string state) {
        Animator anim = GetComponent<Animator>();

        if (state == "Idle" || state == "Run" || state == "Jump") {
            anim.Play(state);
        }
    }
}
