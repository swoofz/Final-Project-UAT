using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AIController : Controller {

    private CharacterState AiState;
    private string aiState;
    private CircleCollider2D circle;
    private Rigidbody2D rb;
    private Pawn target;
    private float attDamage;
    private bool isGrounded;
    private float cntD; // count / count down
    private GameObject opponent;
    private float jumpCount;

    [Range(-1, 1)]
    public float direction;

    private float timer;

	// Use this for initialization
	void Start () {
        pawn = gameObject.GetComponentInChildren<Pawn>();
        rb = GetComponent<Rigidbody2D>();
        SetUpCircleCollider();
        jumpCount = pawn.jumps;
        timer = 1;
    }
	
	// Update is called once per frame
	void Update () {
        isGrounded = pawn.IsGrounded();

        if (opponent != null) { // this is my logic. where it should go
            Vector3 targetDirection = opponent.transform.position - transform.position;
            bool chase = false;
            timer -= Time.deltaTime;
            if (timer < 0) {
                int randomNum = Random.Range(0, 11);
                if (randomNum > 7) {
                    chase = true;
                } else {
                    chase = false;
                }
                timer = Random.Range(1, 3);
            }



            if (chase) {
                if (targetDirection.x >= 1) {
                    direction = 1;
                } else if (targetDirection.x <= -1) {
                    direction = -1;
                } else {
                    direction = 0;
                }
            }
        } else {
            direction = 0;
        }

        pawn.MoveDirection(direction);

        if (direction != 0) {
            ChangeState(CharacterState.Run);
            pawn.ChangeAnimationState(aiState);
        } else {
            ChangeState(CharacterState.Idle);
            pawn.ChangeAnimationState(aiState);
        }

        if (isGrounded) {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        } else {
            rb.gravityScale = 1;
        }
    }

    void ChangeState(CharacterState state) {
        AiState = state;
        aiState = AiState.ToString();
    }

    // AI Finite State Machine
    void FSM() {
        ChangeState(CharacterState.Idle);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.transform.parent != null) {
            opponent = collision.gameObject.transform.parent.gameObject;
            if (opponent.transform.parent != null) {
                if (opponent.transform.parent.tag == "Character") {
                    opponent = opponent.transform.parent.gameObject;
                }
            }
        }

        // Attack logic

        // If attacking first
        string hitTarget = collision.gameObject.name;
        attDamage = pawn.Attack();
        if (collision.gameObject.transform.parent != null) {
            if (collision.gameObject.transform.parent.GetComponent<Pawn>() != null) {
                target = collision.gameObject.transform.parent.GetComponent<Pawn>();
            }
        }
        if (hitTarget == "Head" || hitTarget == "Body" || hitTarget == "Legs") {
            if (target != null) {
                target.TakeDamage(attDamage, 123, "");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.transform.parent != null) {
            if (other.gameObject.transform.parent.gameObject.transform.parent != null) {
                if (other.gameObject.transform.parent.gameObject.transform.parent.tag == "Character") {
                    if (opponent == other.gameObject.transform.parent.gameObject.transform.parent.gameObject) {
                        opponent = null;
                    }
                }
            }
        }
    }

    void SetUpCircleCollider() {
        circle = GetComponent<CircleCollider2D>();
        circle.radius = 20;
        circle.isTrigger = true;
    }
}
