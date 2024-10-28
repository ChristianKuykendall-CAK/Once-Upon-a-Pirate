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

            GameManager.instance.health = GameManager.instance.lasthealth;

            if (GameManager.instance.playerTransform != null)
            {
                // Start coroutine to set the player's position after the scene has fully loaded
                StartCoroutine(SetPlayerPositionAfterLoad(new Vector3(
                    GameManager.instance.playerPosX,
                    GameManager.instance.playerPosY,
                    GameManager.instance.playerPosZ
                )));
            }

            SceneManager.sceneLoaded -= OnGameSceneLoaded; // Unsubscribe to prevent multiple calls
        }
    }
    private IEnumerator SetPlayerPositionAfterLoad(Vector3 position)
    {
        // Wait for one frame to ensure everything loads
        yield return null;

        if (GameManager.instance.playerTransform != null)
        {
            GameManager.instance.playerTransform.position = position;
        }
    }
}
