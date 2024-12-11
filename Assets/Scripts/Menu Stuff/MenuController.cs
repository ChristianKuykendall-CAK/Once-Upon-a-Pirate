using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button startButton;
    public Button loadButton;
    public Button controlsButton;
    public Button endButton;

    void Start()
    {
        // Create listeners for buttons
        startButton.onClick.AddListener(StartGame);
        loadButton.onClick.AddListener(LoadGame);
        controlsButton.onClick.AddListener(ControlsScreen);
        endButton.onClick.AddListener(EndGame);
    }

    public void StartGame() // Starts game
    {
        if (GameManager.instance != null)
        {
            // Reset game for a new game
            GameManager.instance.health = 100;
            GameManager.instance.ammo = 5;
            GameManager.instance.coin = 0;
            GameManager.instance.LevelNum = GameManager.Level.LevelOne; // Default Level 1
        }

        SceneManager.LoadScene("Intro Story");
    }

    public void LoadGame()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.Load(); // Loads saved game state
            string sceneName = GameManager.instance.LevelNum == GameManager.Level.LevelOne ? "LevelOne" : "LevelTwo";
            SceneManager.LoadScene(sceneName);

            // Ensures the saved position is applied after the scene is loaded
            SceneManager.sceneLoaded += OnGameSceneLoaded;
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Runs only for LevelOne or LevelTwo
        if ((scene.name == "LevelOne" || scene.name == "LevelTwo") && GameManager.instance != null)
        {
            // Unsubscribe to avoid duplicate calls
            SceneManager.sceneLoaded -= OnGameSceneLoaded;

            // Find the player in the scene
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                // Set the player's position to the saved position
                player.transform.position = GameManager.instance.playerTransform;
            }
        }
    }

    public void ControlsScreen() // Opens the controls screen
    {
        SceneManager.LoadScene("Controls");
    }

    public void EndGame() // Closes the game
    {
        Application.Quit();
        Debug.Log("Game has been exited.");
    }
}
