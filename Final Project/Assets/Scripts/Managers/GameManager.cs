using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public enum Characters { Knight, FemaleNinja, Robot, Adverturer }     // Create an enum for character can be

    public static GameManager instance;     // Create a singleton Game Manager

    [Header("Lives")]
    public int playerLives;     // Create a variable for player lives
    public int AILives;         // Create a variable for AI lives
    private int livesP, livesAI;

    [Header("End Scenes")]
    public string winScreen;    // Create a variable to show the win screen if win
    public string loseScreen;   // Create a variable to show the lose screen if lose

    [HideInInspector] public List<Transform> startLocation;
    [HideInInspector] public float playerDamageTaken, AIDamageTaken;
    [HideInInspector] public int randomNum = 1000;

    private string player1, ai;
    private Characters AI;
    private bool gameOver;
    private CreateCharacter create;

    void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Use this for initialization
    void Start() {
        create = GetComponent<CreateCharacter>();
        livesP = playerLives;
        livesAI = AILives;
    }

    // Update is called once per frame
    void Update() {

        if (playerLives < 0 || AILives < 0) {
            gameOver = true;
        }

        if (gameOver) {
            // Show the game over Scene
            if (playerLives < 0) {
                // Show Win Screne
                ChangeScenes(loseScreen);
            }
            if (AILives < 0) {
                // Show the Lose Screne
                ChangeScenes(winScreen);
            }

            ResetGame();
        }

	}

    void ResetGame() {
        playerLives = livesP;
        AILives = livesAI;
        playerDamageTaken = 0;
        AIDamageTaken = 0;
        randomNum = 1000;
        gameOver = false;
    }

    void PickAI() {
        // Pick a random character
        int randomNum = Random.Range(0, 4);

        AI = ( Characters )randomNum;
        ai = AI.ToString();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        instance.PickAI();
        instance.create.Create(instance.player1, true, "Player");
        instance.create.Create(instance.ai, false, "AI");
    }

    // Public scene functions
    public void ChangeScenes(string scene) {
        SceneManager.LoadScene(scene);
    }

    public void LoadArena(string scene) {
        SceneManager.LoadScene(scene);
        SceneManager.sceneLoaded += OnSceneLoaded;
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

    public void IfPlayer(GameObject moveForwardButton) {
        if (instance.player1 != null) {
            moveForwardButton.SetActive(true);
        }
    }
}
