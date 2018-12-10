using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCreatePlayer : MonoBehaviour {

    public GameObject picked;

    private GameObject character;

	// Use this for initialization
	void Start () {
        CreatePlayer("Player1");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreatePlayer(string name) {
        character = new GameObject(name);
        character.AddComponent<PlayerController>();

        GameObject pawn = Instantiate(picked, character.transform.position, character.transform.rotation);
        pawn.transform.parent = character.transform;
    }
}
