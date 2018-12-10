using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D), typeof(SpriteRenderer), typeof(Animator))]
public class Pawn : MonoBehaviour {

    public float speed;     // Create a variable to controller character's speed



    public virtual void MoveDirection(float direction) {
        // Move right or left
    }

    public virtual void Jump(float jumpForce) {
        // Go up or down
    }

    public virtual void Attack(float damage) {
        // Attack other players or AI's
    }

    public virtual void ChangeState(string state) {
        // change the state from the controller
    }

    public virtual bool IsGrounded() {
        // find the what makes this true

        return false;
    }



    void Update() {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }
}
