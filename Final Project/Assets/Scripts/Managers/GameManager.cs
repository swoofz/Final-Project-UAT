using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    /*****************************************************************************************
     * How to make random character selection (probably in the gameManager Script)
     * private Character character (enum with characters)
     * private int num for random numbers to pick the character
     * 
     * num = Random.Range(0,4);
     * character = (Characters)num;
     *****************************************************************************************/

    public enum Characters { Knight, FemaleNinja, Robot, Adverturer }     // Create an enum for character can be

    public static GameManager instance;     // Create a singleton Game Manager

    [Header("Lives")]
    public int playerLives;     // Create a variable for player lives
    public int AILives;         // Create a variable for AI lives

    [Header("End Scenes")]
    public string winScreen;    // Create a variable to show the win screen if win
    public string loseScreen;   // Create a variable to show the lose screen if lose

    [HideInInspector] public List<Transform> startLocation;
    [HideInInspector] public float playerDamageTaken, AIDamageTaken;

    private string player1;
    private Characters AI;
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
                ChangeScenes(winScreen);
            }
            if (AILives < 0) {
                // Show the Lose Screne
                ChangeScenes(loseScreen);
            }
        }

	}

    void PickAI() {
        // Pick a random character
        int randomNum = Random.Range(0, 4);

        AI = ( Characters )randomNum;
    }

    // Public scene functions
    public void ChangeScenes(string scene) {
        SceneManager.LoadScene(scene);
    }

    public void ToggleScreen(GameObject screen) {
        screen.SetActive(!screen.activeSelf);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void PickCharacter(string character) {
        instance.player1 = character;
    }
}
