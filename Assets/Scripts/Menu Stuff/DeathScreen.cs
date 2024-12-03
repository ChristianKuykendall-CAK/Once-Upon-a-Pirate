using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public Button loadButton;
    public Button menuButton;

    void Start()
    {
        // Assign button actions
        loadButton.onClick.AddListener(LoadGame);
        menuButton.onClick.AddListener(LoadMenu);
    }

    public void LoadGame() // Clicking the load button will load saved data
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.Load(); // Load saved game state

            string sceneName = GameManager.instance.LevelNum == GameManager.Level.LevelOne ? "LevelOne" : "LevelTwo";
            SceneManager.LoadScene(sceneName);

            // Ensure we apply saved position after the scene is fully loaded
            SceneManager.sceneLoaded += OnGameSceneLoaded;
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    public void LoadMenu() // Clicking the menu button will load the main menu
    {
        SceneManager.LoadScene("Menu");
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure this runs only for LevelOne or LevelTwo
        if ((scene.name == "LevelOne" || scene.name == "LevelTwo") && GameManager.instance != null)
        {
            // Unsubscribe to avoid duplicate calls
            SceneManager.sceneLoaded -= OnGameSceneLoaded;

            // Find the player object in the scene
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                // Set the player's position to the saved position
                player.transform.position = GameManager.instance.playerTransform;
            }
        }
    }
}
