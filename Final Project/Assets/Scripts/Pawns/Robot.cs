using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Pawn {

    public GameObject bullet, muzzle;

    private GameObject bullets;

    public override void ChangeAnimationState(string state) {
        Animator anim = GetComponent<Animator>();

        if (state == "Slide" || state == "Shoot" || state == "JumpAtt" || state == "JumpShoot" || state == "RunShoot") {
            anim.Play(state);
        } else {
            base.ChangeAnimationState(state);
        }
    }

    public override bool IsGrounded() {
        // Robot is grounded 
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

    public override void Shoot() {
        GameObject shotLocation = null;
        if (GameObject.Find("Bullets") == null) {
            bullets = new GameObject("Bullets");
        } else {
            bullets = GameObject.Find("Bullets");
        }

        foreach (Transform child in transform) {
            if (child.name == "Shoot") {
                shotLocation = child.gameObject;
            }
        }
        GameObject clone = Instantiate(muzzle, shotLocation.transform.position, shotLocation.transform.rotation);
        clone.transform.parent = bullets.transform;
        Destroy(clone, 0.4f);
        clone = Instantiate(bullet, shotLocation.transform.position, shotLocation.transform.rotation);
        clone.transform.parent = bullets.transform;
    }

}
