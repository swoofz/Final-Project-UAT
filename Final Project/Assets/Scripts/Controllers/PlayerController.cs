using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller {

    private CharacterState playerState;     // Create a varaible to hold our state
    private float direction;                // Create a varaible to store the direction
    private Rigidbody2D rb;                 // Create a varaible to store our Rigibody component
    private int lives;

    private bool attacking;
    private float cnt, cntD;
    private float attDamage;
    private Pawn target;
    private int hitDirection;
    private bool isGrounded;
    private int jumpCount;
    private bool shoot;


	// Use this for initialization
	void Start () {
        pawn = gameObject.GetComponentInChildren<Pawn>();
        rb = GetComponent<Rigidbody2D>();
        lives = GameManager.instance.playerLives;
        cnt = 0.35f;
    }
	
	// Update is called once per frame
	void Update () {
        direction = Input.GetAxis("Horizontal");
        isGrounded = pawn.IsGrounded();


        if (isGrounded) {
            rb.gravityScale = 0;
            if (rb.velocity.y < 0) {
                rb.velocity = Vector2.zero;
            }
            jumpCount = pawn.jumps;
        } else {
            rb.gravityScale = 1;

            if (rb.velocity.y < -20f) {
                rb.velocity = new Vector2(rb.velocity.x, -20f);
            }
        }

        if (jumpCount > 0) {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1)) {
            attacking = true;
            cntD = cnt;

            if (Input.GetKeyDown(KeyCode.Mouse1) && !pawn.GetComponent<Knight>()) {
                pawn.Shoot();
                shoot = true;
            }

            if (pawn.GetComponent<Knight>()) {
                // do a charge Attack when made
            }
        }

        if (attacking) {
            AttackTimer();
        }

        playerState = ChangeState();
        pawn.ChangeAnimationState(playerState.ToString());
        GetHitDirection();
        pawn.MoveDirection(direction);
    }

    CharacterState ChangeState() {
        if (direction == 0) {
            playerState = CharacterState.Idle;
        }

        if (direction != 0) {
            playerState = CharacterState.Run;
        }

        if (attacking) {
            playerState = CharacterState.Attack;

            if (shoot) {
                playerState = CharacterState.Shoot;
                if (pawn.GetComponent<Ninja>()) {
                    playerState = CharacterState.Throw;
                }
            }
        }

        if (!isGrounded) {
            playerState = CharacterState.Jump;

            if (attacking) {
                playerState = CharacterState.JumpAtt;
                if (pawn.GetComponent<Adverturer>()) {
                    playerState = CharacterState.Attack;
                }

                if (shoot) {
                    playerState = CharacterState.Shoot;
                    if (pawn.GetComponent<Ninja>()) {
                        playerState = CharacterState.Throw;
                    }
                }
            }
        }

        return playerState;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (attacking) {
            string hitTarget = collision.gameObject.name;
            attDamage = pawn.Attack();

            if (collision.gameObject.transform.parent != null) {
                if (collision.gameObject.transform.parent.GetComponent<Pawn>() != null) {
                    target = collision.gameObject.transform.parent.GetComponent<Pawn>();
                }
            }

            if (hitTarget == "Head" || hitTarget == "Body" || hitTarget == "Legs") {
                if (target != null) {
                    GameManager.instance.playerDamageTaken += attDamage;
                    target.TakeDamage(attDamage, hitDirection, hitTarget);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Boundary") {
            lives -= 1;
            GameManager.instance.playerLives = lives;
            damagePercent = 0;
            rb.velocity = Vector2.zero;
            transform.position = new Vector3(0, 30, 0);
        }
    }

    void GetHitDirection() {
        if (direction > 0) {
            hitDirection = 1;
        }

        if (direction < 0) {
            hitDirection = -1;
        }
    }

    void Jump() {
        if (Input.GetKeyDown(KeyCode.Space)) {  // Jump Up
            pawn.Jump(1);
            jumpCount -= 1;
            pawn.ChangeAnimationState("Idle");
            pawn.ChangeAnimationState("Jump");
        }
    }

    void AttackTimer() {
        if (pawn.IsGrounded()) {
            direction = 0;
        }
        cntD -= Time.deltaTime;
        if (cntD < 0) {
            attacking = false;
            shoot = false;
        }
    }
}
