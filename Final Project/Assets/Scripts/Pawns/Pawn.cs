using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer), typeof(Animator))]
public class Pawn : MonoBehaviour {

    public float speed;     // Create a variable to controller character's speed



    public virtual void MoveDirection(float direction) {
        // Move right or left
        GameObject target = transform.parent.gameObject;
        target.transform.position += Vector3.right * direction * speed * Time.deltaTime;
        ChangeSpriteDirection(direction);
    }

    public virtual void ChangeSpriteDirection(float direction) {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (direction != 0) {
            if (direction > 0) {
                sr.flipX = false;
            } else {
                sr.flipX = true;
            }
        }
    }

    public virtual void Jump(float jumpForce) {
        // Go up or down
    }

    public virtual void Attack(float damage) {
        // Do nothing from this component
    }

    public virtual void ChangeAnimationState(string state) {
        // Handle animation in spefic component that inheriet from Pawn
    }

    public virtual bool IsGrounded() {
        // find the what makes this true

        return false;
    }
}
