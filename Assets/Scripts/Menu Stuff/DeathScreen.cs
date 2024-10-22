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

    public void LoadGame() //clicking the load button will load saved data
    {
        SceneManager.LoadScene("LevelOne");
        // Ensure we load the game after the scene has fully loaded
        SceneManager.sceneLoaded += OnGameSceneLoaded;
    }

    public void LoadMenu() //clicking the menu button will load the main menu
    {
        SceneManager.LoadScene("Menu");
    }

    private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LevelOne" && GameManager.instance != null)
        {
            GameManager.instance.Load();
            SceneManager.sceneLoaded -= OnGameSceneLoaded; // Unsubscribe to prevent multiple calls
        }
    }

}
