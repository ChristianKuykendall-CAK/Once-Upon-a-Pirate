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
        loadButton.onClick.AddListener(LoadGame);
        menuButton.onClick.AddListener(LoadMenu);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("LevelOne");
        SceneManager.sceneLoaded += OnGameSceneLoaded;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            GameManager.instance.playerTransform = player.transform;
        }
        if (scene.name == "LevelOne" && GameManager.instance != null)
        {
            GameManager.instance.Load();

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

}
