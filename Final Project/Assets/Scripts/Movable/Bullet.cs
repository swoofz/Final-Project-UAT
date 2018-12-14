using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float bulletSpeed;
    public float damage;


	// Update is called once per frame
	void Update () {
        transform.position += transform.right * bulletSpeed * Time.deltaTime;
	}

    void OnTriggerEnter2D(Collider2D other) {
        string hitTarget = other.gameObject.name;

        if (other.gameObject.transform.parent != null) {
            if (other.gameObject.transform.parent.GetComponent<Pawn>() != null) {
                Pawn target = other.gameObject.transform.parent.GetComponent<Pawn>();

                if (hitTarget == "Head" || hitTarget == "Body" || hitTarget == "Legs") {
                    if (target != null) {
                        target.TakeDamage(damage, transform.right.x, hitTarget);
                        if (target.transform.parent.tag == "Player") {
                            GameManager.instance.playerDamageTaken += damage;
                        }
                        if (target.transform.parent.tag == "AI") {
                            GameManager.instance.AIDamageTaken += damage;
                        }

                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag != "AI") {
            Destroy(gameObject);
        }
    }
}
