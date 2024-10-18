using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChange : MonoBehaviour
{

    public Button startButton;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame() //starts LEVEL 2 when the start button is pressed
    {
       // SceneManager.LoadScene("Level2");
    }


}