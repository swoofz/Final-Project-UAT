using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AIController : Controller {

    private CharacterState AiState;
    private string aiState;
    private CircleCollider2D circle;
    private Rigidbody2D rb;
    private Pawn hitTarget;
    private float attDamage;
    private bool isGrounded;
    private GameObject target;
    private float jumpCount;
    private float cnt, cntD;
    private float hitDirection;

    private float direction;

    // Ai State Varaibles
    private enum Options { Idle, GoTO, Attack }
    private Options option;
    private string opt;
    private bool detect, canSee;
    private bool attacking, shoot;

	// Use this for initialization
	void Start () {
        pawn = gameObject.GetComponentInChildren<Pawn>();
        rb = GetComponent<Rigidbody2D>();
        SetUpCircleCollider();
        jumpCount = pawn.jumps;
        cnt = 0.8f;
        ChangeOptions(Options.Idle);
    }

	// Update is called once per frame
	void Update () {
        isGrounded = pawn.IsGrounded();

        if (isGrounded) {
            rb.gravityScale = 0;
            if (rb.velocity.y < 0) {
                rb.velocity = Vector2.zero;
            }
            jumpCount = pawn.jumps;
        } else {
            rb.gravityScale = 1;
        }

        // FSM Logic
        if (target != null) {
            detect = true;
            canSee = CanSee();
        } else {
            detect = false;
            canSee = false;
        }
        FSM();

        if (attacking) {
            AttackTimer();
        }

        aiState = ChangeAIState();
        pawn.ChangeAnimationState(aiState);
        GetHitDirection();
        pawn.MoveDirection(direction);
    }

    // AI Finite State Machine
    void FSM() {
        if (opt == "Idle") {
            // Do Nothing

            // if detect GoTo
            if (detect) {
                ChangeOptions(Options.GoTO);
            }
            // if see Attack
            if (canSee) {
                ChangeOptions(Options.Attack);
            }

        } else if (opt == "GoTO") {
            // go to target
            if (target != null) {
                GoToTarget();
            }

            // if see Attack
            if (canSee) {
                ChangeOptions(Options.Attack);
            }
            // if cant detect idle
            if (!detect) {
                ChangeOptions(Options.Idle);
            }
        } else if (opt == "Attack") {
            // Possible attack target
            if (target != null) {
                GoToTarget();
            }
            if (!attacking) {
                cntD = cnt;
                Attack();
            }

            // if cant detect Idle
            if (!detect) {
                ChangeOptions(Options.Idle);
            }
            // cant see GoTO
            if (!canSee) {
                ChangeOptions(Options.GoTO);
            }
        }
    }

    void ChangeOptions(Options state) {
        option = state;
        opt = option.ToString();
    }

    string ChangeAIState() {
        if (direction == 0) {
            AiState = CharacterState.Idle;
        }

        if (direction != 0) {
            AiState = CharacterState.Run;
        }

        if (attacking) {
            AiState = CharacterState.Attack;

            if (shoot) {
                AiState = CharacterState.Shoot;
                if (pawn.GetComponent<Ninja>()) {
                    AiState = CharacterState.Throw;
                }
            }
        }

        if (!isGrounded) {
            AiState = CharacterState.Jump;

            if (attacking) {
                AiState = CharacterState.JumpAtt;

                if (shoot) {
                    AiState = CharacterState.Shoot;
                    if (pawn.GetComponent<Ninja>()) {
                        AiState = CharacterState.Throw;
                    }
                }
            }
        }

        return AiState.ToString();
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.transform.parent != null) {
            GameObject initial = collision.gameObject.transform.parent.gameObject;
            if (initial.transform.tag == "Character") {
                target = initial.transform.parent.gameObject;
            }
        }

        // Attack logic

        // If attacking first
        if (attacking) {
            string hitTarget = collision.gameObject.name;
            attDamage = pawn.Attack();
            if (collision.gameObject.transform.parent != null) {
                if (collision.gameObject.transform.parent.GetComponent<Pawn>() != null) {
                    this.hitTarget = collision.gameObject.transform.parent.GetComponent<Pawn>();
                }
            }
            if (hitTarget == "Head" || hitTarget == "Body" || hitTarget == "Legs") {
                if (this.hitTarget != null) {
                    this.hitTarget.TakeDamage(attDamage, hitDirection, hitTarget);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.transform.parent != null) {
            if (other.gameObject.transform.parent.tag == "Character") {
                if (target == other.gameObject.transform.parent.transform.parent.gameObject) { 
                    target = null;
                }
            }
        }
    }

    void SetUpCircleCollider() {
        circle = GetComponent<CircleCollider2D>();
        circle.radius = 20;
        circle.isTrigger = true;
    }

    void GoToTarget() {
        Vector3 goToTarget = target.transform.position - transform.position;
        transform.right = new Vector3(goToTarget.x, 0, 0);
        GetDirection(goToTarget);
        pawn.MoveDirection(direction);
    }

    void GetDirection(Vector3 vector) {
        if (vector.x > 2.9f) {
            direction = 1;
        } else if (vector.x < -2.9f) {
            direction = -1;
        } else {
            direction = 0;
        }
    }

    void Attack() {
        // Possible to attack
        int num = Random.Range(0, 3);
        if (pawn.GetComponent<Knight>()) {
            num = Random.Range(0, 2);
        }

        if (num == 0) { // Don't Attack
            opt = "GoTo";
        }
        if (num == 1) {     // Melee Attack
            bool inRange = InRange();
            if (inRange) {
                attacking = true;
            }
        }
        if (num == 2) { // Range Attack
            pawn.Shoot();
            attacking = true;
            shoot = true;
        }
    }

    bool InRange() {
        // Find if the target is close enought to melee attack
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 3f);
        if (hit.collider != null) {
            if (hit.collider.transform.parent != null) {
                if (hit.collider.transform.parent.gameObject.tag == "Character") {
                    return true;
                }
            }
        }

        return false;
    }

    bool CanSee() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 20f); // Orgin, look diretion, distance can see
        if (hit.collider != null) {
            if (hit.collider.transform.parent != null) {
                if (hit.collider.transform.parent.gameObject.tag == "Character") {
                    return true;
                }
            }
        }

        return false;
    }

    void GetHitDirection() {
        if (direction > 0) {
            hitDirection = 1;
        }

        if (direction < 0) {
            hitDirection = -1;
        }
    }

    void AttackTimer() {
        if (isGrounded) {
            direction = 0;
        }
        cntD -= Time.deltaTime;
        if (cntD < 0) {
            attacking = false;
            shoot = false;
        }
    }
}
