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

    // Same code from MenuController script
    public void LoadGame()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.Load();

            string sceneName = GameManager.instance.LevelNum == GameManager.Level.LevelOne ? "LevelOne" : "LevelTwo";
            SceneManager.LoadScene(sceneName);

            SceneManager.sceneLoaded += OnGameSceneLoaded;
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if ((scene.name == "LevelOne" || scene.name == "LevelTwo") && GameManager.instance != null)
        {
            SceneManager.sceneLoaded -= OnGameSceneLoaded;

            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                player.transform.position = GameManager.instance.playerTransform;
            }
        }
    }
}
