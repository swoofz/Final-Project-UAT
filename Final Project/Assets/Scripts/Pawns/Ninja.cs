﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Pawn {

    public GameObject kunia;

    private GameObject bullets;

    public override void ChangeAnimationState(string state) {
        Animator anim = GetComponent<Animator>();

        if (state == "Slide" || state == "Throw" || state == "JumpAtt" || state == "JumpThrow" || state == "Glide") {
            anim.Play(state);
        } else {
            base.ChangeAnimationState(state);
        }
    }

    public override bool IsGrounded() {
        // Ninja is grounded 
        Transform feet = transform.GetChild(5);
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

    public override void TakeDamage(float damage, float direction, string hitSpot) {
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
    }

    public override void Shoot() {
        GameObject shotLocation = null;
        if (GameObject.Find("Bullets") == null) {
            bullets = new GameObject("Bullets");
        } else {
            bullets = GameObject.Find("Bullets");
        }

        foreach (Transform child in transform) {
            if (child.name == "Kunai Location") {
                shotLocation = child.gameObject;
            }
        }

        GameObject clone = Instantiate(kunia, shotLocation.transform.position, shotLocation.transform.rotation);
        clone.transform.parent = bullets.transform;
    }

}
