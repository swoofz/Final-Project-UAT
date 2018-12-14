using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCharacter : MonoBehaviour {

    public List<GameObject> Characters;

    private GameObject player, pawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Create(bool isPlayer, string name) {
        player = new GameObject(name);
        CharacterSetup(isPlayer);
    }

    void CharacterSetup(bool isPlayer) {
        if (isPlayer) {
            player.tag = "Player";
            player.AddComponent<PlayerController>();
        } else {
            player.tag = "AI";
            player.AddComponent<AIController>();
        }
    }
}
