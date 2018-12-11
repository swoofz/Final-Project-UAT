using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer), typeof(Animator))]
public class Pawn : MonoBehaviour {

    public float speed;     // Create a variable to controller character's speed
    public float jumpForce; // Create a variable to controller character's jumpForce

    public virtual void MoveDirection(float direction) {
        // Move right or left
        GameObject target = transform.parent.gameObject;
        target.transform.position += Vector3.right * direction * speed * Time.deltaTime;
        ChangeSpriteDirection(direction);
    }

    public virtual void ChangeSpriteDirection(float direction) {
        if (direction != 0) {
            if (direction > 0) {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            } else {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    public virtual void Jump(float jumpDirection, Rigidbody2D rb) {
        GameObject target = transform.parent.gameObject;
        rb.velocity = Vector3.up * jumpDirection * jumpForce;
    }

    public virtual void Attack(float damage) {
        // Do nothing from this component
    }

    public virtual void ChangeAnimationState(string state) {
        // Handle extra animations in spefic component that inheriet from Pawn
        Animator anim = GetComponent<Animator>();

        if (state == "Idle" || state == "Run" || state == "Jump" || state == "Attack") {
            anim.Play(state);
        }
    }

    public virtual bool IsGrounded() {
        // find the what makes this true for each pawn

        return false;
    }
}
