using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public class Pawn : MonoBehaviour {

    public float speed;         // Create a variable to controller character's speed
    public float jumpForce;     // Create a variable to controller character's jumpForce
    public float attackDamage;  // Create a variable for character attack damage
    public int jumps;       // Create a variable for how many jumps and character can do

    [HideInInspector] public float damagePercentage;    // Create a varaible for character damage over time multipled


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

    public virtual void Jump(float jumpDirection) {
        Rigidbody2D rb = transform.parent.GetComponent<Rigidbody2D>();
        rb.velocity = Vector3.up * jumpDirection * jumpForce;
    }

    public virtual bool IsGrounded() {
        // Find what makes this true for each pawn

        return false;
    }

    public virtual void ChangeAnimationState(string state) {
        // Handle extra animations in spefic component that inheriet from Pawn
        Animator anim = GetComponent<Animator>();

        if (state == "Idle" || state == "Run" || state == "Jump" || state == "Attack") {
            anim.Play(state);
        }
    }

    public virtual float Attack() {
        // Return damage

        return attackDamage;
    }

    public virtual void TakeDamage(float damage, float direction, string hitSpot) {
        // Call to take damage with increase over time of getting 
        Rigidbody2D rb = transform.parent.GetComponent<Rigidbody2D>();
        Vector2 forceDirection = Vector2.zero;
        damagePercentage += damage;


        if (hitSpot == "Head") {
            forceDirection = new Vector2(0.8f * direction, -1f) * damage;
            if (IsGrounded()) {
                forceDirection = new Vector2(0.8f, .2f) * direction * damage;
            }
        }

        if (hitSpot == "Body") {
            forceDirection = new Vector2(1f, .2f) * direction * damage;
        }

        if (hitSpot == "Legs") {
            forceDirection = new Vector2(0.8f * direction, 1f) * damage;
        }

        rb.AddForce(forceDirection * damagePercentage);

        // For now every pawn take damage the same
    }

    public virtual void Shoot() {
        // For pawn that have shoot or throw
    }

}
