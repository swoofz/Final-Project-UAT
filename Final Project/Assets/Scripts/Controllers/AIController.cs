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
    private int lives;
    private float attDamage;
    private bool isGrounded;
    private GameObject target;
    private float jumpCount;
    private float cnt, cntD;
    private float hitDirection;
    private float jumpTimer;

    private float direction;

    // Ai State Varaibles
    private enum Options { Idle, GoTO, Attack }
    private Options option;
    private string opt;
    private bool detect, canSee;
    private bool attacking, shoot;

	// Use this for initialization
	void Start () {
        cnt = 0.8f;
        jumpTimer = 1f;
        rb = GetComponent<Rigidbody2D>();
        lives = GameManager.instance.AILives;
        ChangeOptions(Options.Idle);
        //pawn = gameObject.GetComponentInChildren<Pawn>();
        SetUpCircleCollider();
    }

	// Update is called once per frame
	void Update () {

        if (pawn != null) {
            isGrounded = pawn.IsGrounded();
        } else {
            pawn = gameObject.GetComponentInChildren<Pawn>();
            jumpCount = pawn.jumps;
        }

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

        if (jumpCount > 0) {
            RandomJump();
        }

        if (pawn != null) {
            aiState = ChangeAIState();
            pawn.ChangeAnimationState(aiState);
            GetHitDirection();
            pawn.MoveDirection(direction);
        }
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
                    GameManager.instance.playerDamageTaken += attDamage;
                    this.hitTarget.TakeDamage(attDamage, hitDirection, hitTarget);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Boundary") {
            lives -= 1;
            GameManager.instance.AILives = lives;
            pawn.damagePercentage = 0;
            GameManager.instance.AIDamageTaken = 0;
            rb.velocity = Vector2.zero;
            if (lives < 0 || GameManager.instance.playerLives < 0) {
                Destroy(gameObject);
            } else {
                transform.position = new Vector3(0, 30, 0);
            }
        }
    }

    void SetUpCircleCollider() {
        circle = GetComponent<CircleCollider2D>();
        circle.radius = 50;
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
            direction = .5f;
        } else if (vector.x < -2.9f) {
            direction = -.5f;
        } else {
            direction = 0;
        }
    }

    void Attack() {
        // Possible to attack
        int num = Random.Range(1, 3);
        if (pawn.GetComponent<Knight>()) {
            num = 1;
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

    void RandomJump() {
        jumpTimer -= Time.deltaTime;
        if (jumpTimer < 0) {
            int num = Random.Range(1, 11);
            Jump(num);
        }
    }

    void Jump(int num) {
        if (num % 2 == 0) {
            pawn.Jump(1);
            jumpCount -= 1;
        }

        jumpTimer = 1f;
    }
}
