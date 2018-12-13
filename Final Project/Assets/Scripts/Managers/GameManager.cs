using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int playerLives;     // Create a variable for player lives
    public int AILives;         // Create a variable for AI lives

    private bool gameOver;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (playerLives < 0 || AILives < 0) {
            gameOver = true;
        }

        if (gameOver) {
            // Show the game over Scene
            if (playerLives < 0) {
                // Show Win Screne
            }
            if (AILives < 0) {
                // Show the Lose Screne
            }
        }

	}
}
