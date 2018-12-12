using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Pawn {

    public GameObject bullet;

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
        if (GameObject.Find("Bullets") == null) {
            bullets = new GameObject("Bullets");
        } else {
            bullets = GameObject.Find("Bullets");
        }

        GameObject shotLocation = GameObject.Find("Shoot");
        GameObject clone = Instantiate(bullet, shotLocation.transform.position, shotLocation.transform.rotation);
        clone.transform.parent = bullets.transform;
    }

}
