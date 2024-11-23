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
        //create on click listeners for start and end buttons
        startButton.onClick.AddListener(StartGame);
        loadButton.onClick.AddListener(LoadGame);
        controlsButton.onClick.AddListener(ControlsScreen);
        endButton.onClick.AddListener(EndGame);
    }

    public void StartGame() //starts game when the start button is pressed
    {
        GameManager.instance.health = 100;
        GameManager.instance.ammo = 5;
        GameManager.instance.coin = 0;
        SceneManager.LoadScene("LevelOne");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("LevelOne");
        // Ensure we load the game after the scene has fully loaded
        SceneManager.sceneLoaded += OnGameSceneLoaded;
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LevelOne" && GameManager.instance != null)
        {
            GameManager.instance.Load();
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                // Set the player's position to the saved position in GameManager
                player.transform.position = GameManager.instance.playerTransform;
            }
            else
            {
                Debug.LogError("Player object not found in the scene!");
            }
            SceneManager.sceneLoaded -= OnGameSceneLoaded; // Unsubscribe to prevent multiple calls
        }
    }


    public void ControlsScreen() //opens the controls view screen when the controls button is pressed
    {
        SceneManager.LoadScene("Controls");
    }

    public void EndGame() //closes game when the quit button is pressed
    {
        Application.Quit();
    }
}